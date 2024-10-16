using AutoMapper;
using Business_Logic_Layer.Models;
using Presentation_Layer.AutoMapperProfiles.CustomResolvers;
using Presentation_Layer.ViewModels.Account;
using Presentation_Layer.ViewModels.Course;
using Presentation_Layer.ViewModels.Exam;
using Presentation_Layer.ViewModels.ExamAssessment;
using Presentation_Layer.ViewModels.UserManagement;

namespace Presentation_Layer.AutoMapperProfiles
{
    public class PresentationLayerAndBusinessLayerModelsMapper : Profile
    {
        public PresentationLayerAndBusinessLayerModelsMapper()
        {
            CreateMap<RegisterViewModel, UserModel>().ReverseMap();
            CreateMap<UserModel, AccountDetailsViewModel>().ReverseMap();
            CreateMap<ViewModels.Account.Shared.CourseViewModel, CourseModel>().ReverseMap();
            CreateMap<CourseModel, ViewModels.Course.Shared.CourseViewModel>().ReverseMap();
            CreateMap<UserModel, ViewModels.Course.Shared.UserViewModel>().ReverseMap();
            CreateMap<ViewModels.Course.Shared.ExamViewModel, ExamModel>();
            CreateMap<ExamModel, ViewModels.Course.Shared.ExamViewModel>()
                .ForMember(dest => dest.NumberOfQuestions, opt => opt.MapFrom(src => src.Questions!.Count));

            CreateMap<PaginationCourseModel, AllCoursesViewModel>().ConvertUsing<PaginationCourseModelToAllCoursesViewModelConverter>();
            CreateMap<PaginationCourseModel, AppliedCoursesViewModel>().ConvertUsing<PaginationCourseModelToAppliedCoursesViewModelConverter>();
            CreateMap<PaginationCourseModel, CourseListWithPaginationViewModel>().ConvertUsing<PaginationCourseModelToCourseListWithPaginationViewModelConverter>();
            CreateMap<CourseModel, CourseDetailsViewModel>().ConvertUsing<CourseModelToCourseDetailsViewModelConverter>();
            CreateMap<UserExamAttemptModel, TakeExamViewModel>().ConvertUsing<UserExamAttemptModelToTakeExamViewModelConverter>();
            CreateMap<List<CourseModel>, MyPassedExamsViewModel>().ConvertUsing<ListCourseModelsToMyPassedExamsViewModelConverter>();
            CreateMap<ExamModel, ExamStatisticViewModel>().ConvertUsing<ExamModelToExamStatisticViewModelConverter>();
            CreateMap<ExamModel, ExamResultViewModel>().ConvertUsing<ExamModelToExamResultViewModelConverter>();

            CreateMap<TakeExamViewModel, UserExamAttemptModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AttemptId))
                .ForMember(dest => dest.UserAnswers, opt => opt.MapFrom(src => src.QuestionAnswers));

            CreateMap<UserAnswerModel, ViewModels.ExamAssessment.Shared.QuestionAnswerViewModel>()
                .ForMember(dest => dest.QuestionAnswerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question.QuestionText))
                .ForMember(dest => dest.AnswerText, opt => opt.MapFrom(src => src.AnswerText))
                .ForMember(dest => dest.QuestionMaxGrade, opt => opt.MapFrom(src => src.Question.MaxGrade))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade))
                .ForMember(dest => dest.QuestionAnswerFeedback, opt => opt.MapFrom(src => src.Feedback));

            CreateMap<ViewModels.ExamAssessment.Shared.QuestionAnswerViewModel, UserAnswerModel>()
                .ForMember(dest => dest.AnswerText, opt => opt.MapFrom(src => src.AnswerText))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade))
                .ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => src.QuestionAnswerFeedback))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.QuestionAnswerId));

            CreateMap<UserExamAttemptModel, TakenExamDetailsViewModel>()
                .ForMember(dest => dest.ExamName, opt => opt.MapFrom(src => src.Exam.Name))
                .ForMember(dest => dest.ExamDescription, opt => opt.MapFrom(src => src.Exam.Description))
                .ForMember(dest => dest.IsExamAssessed, opt => opt.MapFrom(src => src.IsExamAssessed))
                .ForMember(dest => dest.IsAssessedByAi, opt => opt.MapFrom(src => src.IsAssessedByAi))
                .ForMember(dest => dest.TotalGrade, opt => opt.MapFrom(src => src.TotalGrade))
                .ForMember(dest => dest.MaxPossibleExamGrade, opt => opt.MapFrom(src => src.Exam.Questions.Sum(q=>q.MaxGrade)))
                .ForMember(dest => dest.TakenTimeToComplete, opt => opt.MapFrom(src => src.TakenTimeToComplete))
                .ForMember(dest => dest.MinExamPassGrade, opt => opt.MapFrom(src => src.Exam.MinimumPassGrade))
                .ForMember(dest => dest.GeneralFeedback, opt => opt.MapFrom(src => src.Feedback))
                .ForMember(dest => dest.QuestionAnswers, opt => opt.MapFrom(src => src.UserAnswers));

            CreateMap<ExamModel, ViewModels.Exam.ExamViewModel>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Course.Id));

            CreateMap<ExamQuestionModel, ViewModels.Exam.Shared.QuestionViewModel>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ViewModels.Exam.ExamViewModel, ExamModel>()
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => new CourseModel
                {
                    Id = src.CourseId
                }));

            CreateMap<ViewModels.Exam.Shared.QuestionViewModel, ExamQuestionModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.QuestionId));

            CreateMap<UserModel, ViewModels.Exam.Shared.UserViewModel>();

            CreateMap<ExamModel, ExamDetailsViewModel>();
            CreateMap<UserExamAttemptModel, ViewModels.Exam.Shared.ExamAttemptViewModel>()
                .ForMember(dest => dest.AttemptId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IsExamAssessedByAi, opt => opt.MapFrom(src => src.IsAssessedByAi));

            CreateMap<UserExamAttemptModel, ViewModels.Exam.Shared.UserExamAttemptViewModel>()
                .ForMember(dest => dest.IsAssessed, opt => opt.MapFrom(src => src.IsExamAssessed))
                .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.User.LastName));

            CreateMap<UserExamAttemptModel, ExamManualEvaluationViewModel>()
                .ForMember(dest => dest.ExamAttemptId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ExamId, opt => opt.MapFrom(src => src.Exam.Id))
                .ForMember(dest => dest.ExamName, opt => opt.MapFrom(src => src.Exam.Name))
                .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.UserLastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.GeneralFeedback, opt => opt.MapFrom(src => src.Feedback))
                .ForMember(dest => dest.ExamMinimumPassGrade, opt => opt.MapFrom(src => src.Exam.MinimumPassGrade))
                .ForMember(dest => dest.UserAnswers, opt => opt.MapFrom(src => src.UserAnswers));

            CreateMap<ExamManualEvaluationViewModel, UserExamAttemptModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExamAttemptId))
                .ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => src.GeneralFeedback))
                .ForMember(dest => dest.UserAnswers, opt => opt.MapFrom(src => src.UserAnswers));

            CreateMap<List<UserModel>, AllUsersViewModel>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src));

            CreateMap<UserModel, ViewModels.UserManagement.Shared.UserViewModel>();

            CreateMap<UserModel, ViewModels.Trash.Shared.UserViewModel>()
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => $"{src.DeletedBy.FirstName} {src.DeletedBy.LastName}"));

            CreateMap<ExamModel, ViewModels.Trash.Shared.ExamViewModel>()
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => $"{src.DeletedBy.FirstName} {src.DeletedBy.LastName}"))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name));

            CreateMap<CourseModel, ViewModels.Trash.Shared.CourseViewModel>()
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => $"{src.DeletedBy.FirstName} {src.DeletedBy.LastName}"));


            CreateMap<PaginationCourseModel, ViewModels.Trash.DeletedCourseListWithPaginationViewModel>()
                .ForMember(dest => dest.Pagination, opt => opt.MapFrom(src => new ViewModels.Trash.PaginationViewModel
                {
                    TotalItems = src.TotalItems
                }))
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Items));
        }
    }
}