namespace PrefinalWebApplication1.Models
{
	public class StudentViewDeleteModel
	{
		public int StudentID { get; set; }
		public string IsDeleted { get; set; }
		public List<Student> StudentsList { get; set; } = new List<Student>();
	}
}
