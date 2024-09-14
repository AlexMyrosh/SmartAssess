using Presentation_Layer.ViewModels;

namespace Presentation_Layer.Comparer
{
    public class CourseViewModelComparer : IEqualityComparer<CourseViewModel>
    {
        public bool Equals(CourseViewModel x, CourseViewModel y)
        {
            if (x == null || y == null)
                return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(CourseViewModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }

}
