using Azure;
using Azure.AI.OpenAI;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Models.Enums;
using Business_Logic_Layer.Services.Interfaces;
using Microsoft.Extensions.Options;

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
            var allFeedbacks = new List<string>();
            var userExamAttemptModel = await _userExamPassService.GetByIdWithDetailsAsync(examAttemptId);
            var completionOptions = new ChatCompletionsOptions();
            completionOptions.DeploymentName = _openAiConfig.DeploymentName;
            foreach (var userAnswerModel in userExamAttemptModel.UserAnswers)
            {
                var message = $"Here is a student's response to a question. " +
                                                   $"Please provide a grade from 0 to {userAnswerModel.Question.MaxGrade} " +
                                                   $"and also give some feedback.\n\nQuestion: {userAnswerModel.Question.QuestionText}\n\n" +
                                                   $"Student's Answer: {(string.IsNullOrEmpty(userAnswerModel.AnswerText) ? "<MISS>" : userAnswerModel.AnswerText)}\n\n" +
                                                   $"Teacher's Notes: {(string.IsNullOrEmpty(userAnswerModel.Question.TeacherNoteForAssessment) ? "<MISS>" : userAnswerModel.Question.TeacherNoteForAssessment)}\n\n" +
                                                   $"If there is word <MISS> above, it means that value is null or empty." + 
                                                   $"We expect your answer in the following format: \n\nGrade: <grade>\nFeedback: <feedback>";

                completionOptions.Messages.Add(new ChatRequestUserMessage(message));
                completionOptions.MaxTokens = 1000;
                completionOptions.Temperature = 0.0f;

                var openAiResult = await _client.GetChatCompletionsAsync(completionOptions);
                var response = openAiResult.Value.Choices[0].Message.Content;
                userAnswerModel.Grade = ExtractGradeFromAiResponse(response);
                userAnswerModel.Feedback = ExtractFeedbackFromAiResponse(response);
                allFeedbacks.Add(userAnswerModel.Feedback);
            }

            var generalFeedbackPrompt = $"Could you please provide an overall feedback for the all questions that you evaluated." +
               $"We expect your answer in the following format: Feedback: <feedback>";

            completionOptions.Messages.Add(new ChatRequestUserMessage(generalFeedbackPrompt));
            completionOptions.Temperature = 0.5f;

            var overallFeedbackOpenAiResponse = await _client.GetChatCompletionsAsync(completionOptions);
            var overallFeedbackResponse = overallFeedbackOpenAiResponse.Value.Choices[0].Message.Content;
            var overallFeedback = ExtractFeedbackFromAiResponse(overallFeedbackResponse);
            userExamAttemptModel.Feedback = overallFeedback;
            userExamAttemptModel.IsAssessedByAi = true;
            userExamAttemptModel.IsExamAssessed = true;
            userExamAttemptModel.Status = ExamAttemptStatusModel.Completed;
            await _userExamPassService.UpdateAsync(userExamAttemptModel);
        }

        private int ExtractGradeFromAiResponse(string response)
        {
            // Parse grade and feedback from content
            string[] lines = response.Split('\n');

            foreach (var line in lines)
            {
                var parts = line.Split(":");
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    if (key.Equals("Grade", StringComparison.OrdinalIgnoreCase))
                    {
                        if (int.TryParse(value, out int grade))
                        {
                            return grade;
                        }
                    }
                }
            }

            return -1;
        }

        private string ExtractFeedbackFromAiResponse(string response)
        {
            var index = response.IndexOf("Feedback:");
            var result = response.Substring(index + 10);
            return result;
        }

        public async Task<string> ProvideOverallExamFeedback(List<string> allFeedbacks)
        {
            var generalFeedbackPrompt = $"Here are the feedbacks for each question:\n{string.Join("\n", allFeedbacks)}\n\n" +
                $"Based on this, please provide a general feedback." +
                $"We expect your answer in the following format: Feedback: <feedback>";

            var completionOptions = new ChatCompletionsOptions
            {
                DeploymentName = _openAiConfig.DeploymentName,
                Messages = { new ChatRequestUserMessage(generalFeedbackPrompt) },
                MaxTokens = 1000,
                Temperature = 0.3f
            };

            var openAiResult = await _client.GetChatCompletionsAsync(completionOptions);
            var response = openAiResult.Value.Choices[0].Message.Content;
            var overallFeedback = ExtractFeedbackFromAiResponse(response);
            return overallFeedback;
        }
    }
}