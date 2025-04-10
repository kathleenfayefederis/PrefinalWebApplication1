namespace PrefinalWebApplication1.Models
{
    public class CourseViewModel
    {
        public int CourseID { get; set; }

        public string? CourseName { get; set; }
        public int CourseDuration { get; set; }
        public string? CourseDepartment { get; set; }
        public List<CourseViewModel> CoursesList { get; set; } = new List<CourseViewModel>();
    }
}
