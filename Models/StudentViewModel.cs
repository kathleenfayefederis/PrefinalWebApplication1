namespace PrefinalWebApplication1.Models
{
    public class StudentViewModel
    {
        public int StudentID { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? Course { get; set; }
        public string? Section { get; set; }
        public string? EmailAddress { get; set; }
        public string? ContactNumber { get; set; }

        public List<Student> StudentsList { get; set; } = new List<Student>();
    }
}
