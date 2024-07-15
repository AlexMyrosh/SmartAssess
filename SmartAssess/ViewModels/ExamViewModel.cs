namespace Presentation_Layer.ViewModels
{
    public class ExamViewModel
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Subject { get; set; }

        public DateTime ExamStartDateTime { get; set; }

        public DateTime ExamEndDateTime { get; set; }

        public bool IsDeleted { get; set; }

        public List<ExamQuestionViewModel> Questions { get; set; }
    }
}
