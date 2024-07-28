using Azure;
using Azure.AI.OpenAI;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Services.Interfaces;
using Data_Access_Layer.UnitOfWork.Interfaces;
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
            var userExamAttemptModel = await _userExamPassService.GetByIdWithDetailsAsync(examAttemptId);
            foreach (var userAnswerModel in userExamAttemptModel.UserAnswers)
            {
                var completionOptions = new ChatCompletionsOptions
                {
                    DeploymentName = _openAiConfig.DeploymentName,
                    Messages = {
                        new ChatRequestUserMessage($"Here is a student's response to a question. " +
                                                   $"Please provide a grade from 1 to {userAnswerModel.Question.MaxGrade} " +
                                                   $"and also give some feedback.\n\nQuestion: {userAnswerModel.Question.QuestionText}\n\n" +
                                                   $"Student's Answer: {userAnswerModel.AnswerText}\n\n" +
                                                   $"Teacher's Notes: {userAnswerModel.Question.TeacherNoteForAssessment}\n\n" +
                                                   $"We expect your answer in the following format: \n\nGrade: <grade>\nFeedback: <feedback>")
                    },
                    MaxTokens = 1000,
                    Temperature = 0.0f
                };

                var openAiResult = await _client.GetChatCompletionsAsync(completionOptions);
                var response = openAiResult.Value.Choices[0].Message.Content;
                userAnswerModel.Grade = ExtractGradeFromAiResponse(response);
                userAnswerModel.Feedback = ExtractFeedbackFromAiResponse(response);
            }

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
            string[] lines = response.Split('\n');

            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ':' }, 2);

                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    if (key.Equals("Feedback", StringComparison.OrdinalIgnoreCase))
                    {
                        return value;
                    }
                }
            }

            return string.Empty;
        }
    }
}