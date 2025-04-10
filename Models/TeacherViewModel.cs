namespace PrefinalWebApplication1.Models
{
    public class TeacherViewModel
    {
        public int TeacherID { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? Department { get; set; }
        public string? EmailAddress { get; set; }
        public string? ContactNumber { get; set; }
        public List<TeacherViewModel> TeachersList { get; set; } = new List<TeacherViewModel>();
    }
}
