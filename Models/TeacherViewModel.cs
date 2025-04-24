#nullable disable

namespace PrefinalWebApplication1.Models
{
    public class TeacherViewModel
    {
        public int TeacherID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Department { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
		//public List<Student> StudentsList { get; set; } = new List<Student>();
		public List<Teacher> TeachersList { get; set; } = new List<Teacher>();
    }
}
