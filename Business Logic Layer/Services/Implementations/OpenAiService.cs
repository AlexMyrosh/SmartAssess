using Azure;
using Azure.AI.OpenAI;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Models.Enums;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.RegularExpressions;

namespace Business_Logic_Layer.Services.Implementations
{
    public class OpenAiService : IOpenAiService
    {
        private readonly OpenAIClient _client;
        private readonly OpenAiConfig _openAiConfig;
        private readonly IUserExamPassService _userExamPassService;

        public OpenAiService(IOptions<OpenAiConfig> openAiConfig, IUserExamPassService userExamPassService)
        {
            _openAiConfig = openAiConfig.Value;
            _userExamPassService = userExamPassService;
            var credentials = new AzureKeyCredential(openAiConfig.Value.ApiKey);
            _client = new OpenAIClient(new Uri(openAiConfig.Value.Uri), credentials);
        }

        public async Task ExamEvaluationAsync(Guid examAttemptId)
        {
            await _userExamPassService.SetStatusAsync(examAttemptId, ExamAttemptStatusModel.AssessingByAI);

            // Retrieve exam attempt details with associated user answers
            var userExamAttemptModel = await _userExamPassService.GetByIdWithDetailsAsync(examAttemptId);

            // Build a comprehensive prompt including all questions and student answers
            var promptBuilder = new StringBuilder();
            promptBuilder.AppendLine("You are provided with a student's responses to an exam. " +
                "For each question, provide a grade (0 to the maximum grade specified) and detailed feedback. " +
                "If the answer is completely wrong, the grade should be 0, and the feedback should indicate that the answer is incorrect. " +
                "Ensure that the feedback is in the same language as the question. " +
                "Then, give an overall feedback for the exam. " +
                "Format your response as follows:\n\nQuestion: <question number>\nGrade: <grade>\nFeedback: <feedback>\nOverall Feedback: <feedback>");

            for (int i = 0; i < userExamAttemptModel.UserAnswers.Count; i++)
            {
                var userAnswerModel = userExamAttemptModel.UserAnswers[i];
                promptBuilder.AppendLine($"\nQuestion {i + 1}: {userAnswerModel.Question.QuestionText}");
                promptBuilder.AppendLine($"Max Grade: {userAnswerModel.Question.MaxGrade}");
                promptBuilder.AppendLine($"Student's Answer: {(string.IsNullOrEmpty(userAnswerModel.AnswerText) ? "<MISS>" : userAnswerModel.AnswerText)}");
            }

            // Set up chat completion options
            var completionOptions = new ChatCompletionsOptions
            {
                DeploymentName = _openAiConfig.DeploymentName,
                MaxTokens = 3000, // Adjusted for a longer response
                Temperature = 0.5f
            };
            completionOptions.Messages.Add(new ChatRequestUserMessage(promptBuilder.ToString()));

            // Send a single request to OpenAI and get the response
            var openAiResult = await _client.GetChatCompletionsAsync(completionOptions);
            var response = openAiResult.Value.Choices.FirstOrDefault()?.Message.Content ?? string.Empty;
            var assessedResults = ParseAssessmentResponse(response);

            foreach(var result in assessedResults)
            {
                if(result.QuestionNumber == 0)
                {
                    continue;
                }

                userExamAttemptModel.UserAnswers[result.QuestionNumber - 1].Grade = result.Grade;
                userExamAttemptModel.UserAnswers[result.QuestionNumber - 1].Feedback = result.Feedback;
            }

            userExamAttemptModel.Feedback = assessedResults.FirstOrDefault(x=>x.QuestionNumber == 0)?.Feedback;

            // Update the exam attempt model with results
            userExamAttemptModel.IsAssessedByAi = true;
            userExamAttemptModel.IsExamAssessed = true;
            userExamAttemptModel.Status = ExamAttemptStatusModel.Completed;

            // Save changes to the database
            await _userExamPassService.UpdateAsync(userExamAttemptModel);
        }

        private List<AssessedResultModel> ParseAssessmentResponse(string aiResponse)
        {
            var assessedResults = new List<AssessedResultModel>();
            var overallFeedback = string.Empty;
            var questionRegex = new Regex(@"Question:\s(?<questionNumber>\d+)\s*Grade:\s(?<grade>\d+)\s*Feedback:\s(?<feedback>[\s\S]+?)(?=Question:\s\d+:|Overall Feedback:|$)", RegexOptions.Multiline);
            var matches = questionRegex.Matches(aiResponse);
            foreach (Match match in matches)
            {
                var questionNumber = int.Parse(match.Groups["questionNumber"].Value);
                var grade = int.Parse(match.Groups["grade"].Value);
                var feedback = match.Groups["feedback"].Value.Trim();

                assessedResults.Add(new AssessedResultModel
                {
                    QuestionNumber = questionNumber,
                    Grade = grade,
                    Feedback = feedback
                });
            }

            // Extract overall feedback
            var overallFeedbackMatch = Regex.Match(aiResponse, @"Overall Feedback: (?<feedback>[\s\S]+)$", RegexOptions.Multiline);
            if (overallFeedbackMatch.Success)
            {
                overallFeedback = overallFeedbackMatch.Groups["feedback"].Value.Trim();
            }

            // Add overall feedback to the result as a special entry (optional, depending on requirements)
            assessedResults.Add(new AssessedResultModel
            {
                QuestionNumber = 0, // Use 0 or a special indicator for overall feedback
                Grade = 0, // No grade for overall feedback
                Feedback = overallFeedback
            });

            return assessedResults;
        }

        private class AssessedResultModel
        {
            public int QuestionNumber { get; set; }
            public int Grade { get; set; }
            public string Feedback { get; set; }
        }
    }
}