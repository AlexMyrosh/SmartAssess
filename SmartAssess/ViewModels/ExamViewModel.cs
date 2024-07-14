namespace Presentation_Layer.ViewModels
{
    public class ExamViewModel
    {
        public ExamViewModel()
        {
            Questions = new List<ExamQuestionViewModel>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Subject { get; set; }

        public DateTime ExamStartDateTime { get; set; }

        public DateTime ExamEndDateTime { get; set; }

        public IEnumerable<ExamQuestionViewModel> Questions { get; set; }
    }
}
