namespace Business_Logic_Layer.Services.Interfaces
{
    public interface IOpenAiService
    {
        public Task ExamEvaluationAsync(Guid examAttemptId);
    }
}
