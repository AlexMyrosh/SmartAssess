using AutoMapper;
using Business_Logic_Layer.Models;
using Business_Logic_Layer.Models.Enums;
using Presentation_Layer.ViewModels.Enums;
using Presentation_Layer.ViewModels.ExamAssessment;
using Presentation_Layer.ViewModels.ExamAssessment.Shared;

namespace Presentation_Layer.AutoMapperProfiles.CustomResolvers;

public class ListCourseModelsToMyPassedExamsViewModelConverter : ITypeConverter<List<CourseModel>, MyPassedExamsViewModel>
{
    public MyPassedExamsViewModel Convert(List<CourseModel> source, MyPassedExamsViewModel destination, ResolutionContext context)
    {
        var result = new MyPassedExamsViewModel();
        result.TakenExamCourses = new List<TakenExamCoursesViewModel>(source.Count);
        foreach (var course in source)
        {
            var examCourse = new TakenExamCoursesViewModel
            {
                CourseId = course.Id,
                CourseName = course.Name,
                TakenExams = new List<TakenExamsViewModel>(course.Exams.Count)
            };

            foreach (var exam in course.Exams)
            {
                var takenExam = new TakenExamsViewModel
                {
                    ExamId = exam.Id.Value,
                    ExamName = exam.Name,
                    ExamMaxPossibleExamGrade = exam.Questions.Sum(q => q.MaxGrade).Value,
                    ExamMinPassGrade = exam.MinimumPassGrade.Value,
                    FinalGradeCalculationMethod = (FinalGradeCalculationMethodViewModel)exam.FinalGradeCalculationMethod,
                    ExamAttempts = new List<ExamAttemptViewModel>(exam.UserExamAttempts.Count)
                };

                foreach (var examAttempt in exam.UserExamAttempts)
                {
                    if (examAttempt.Status != ExamAttemptStatusModel.InProgress)
                    {
                        takenExam.ExamAttempts.Add(new ExamAttemptViewModel
                        {
                            AttemptId = examAttempt.Id.Value,
                            IsExamAssessed = examAttempt.IsExamAssessed,
                            TotalGrade = examAttempt.TotalGrade.Value
                        });
                    }
                }

                if (takenExam.ExamAttempts.All(x => x.IsExamAssessed))
                {
                    if (takenExam.FinalGradeCalculationMethod == FinalGradeCalculationMethodViewModel.Average)
                    {
                        takenExam.FinalAttemptGrade = takenExam.ExamAttempts.Average(x => x.TotalGrade);
                    }
                    else if (takenExam.FinalGradeCalculationMethod == FinalGradeCalculationMethodViewModel.LastAttempt)
                    {
                        takenExam.FinalAttemptGrade = exam.UserExamAttempts.FirstOrDefault(x =>
                            x.AttemptStarterAt == exam.UserExamAttempts.Max(y => y.AttemptStarterAt)).TotalGrade;
                    }
                    else
                    {
                        takenExam.FinalAttemptGrade = exam.UserExamAttempts.Max(x => x.TotalGrade);
                    }
                }

                examCourse.TakenExams.Add(takenExam);
            }


            result.TakenExamCourses.Add(examCourse);
        }

        return result;
    }
}