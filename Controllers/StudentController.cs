using Microsoft.AspNetCore.Mvc;
using PrefinalWebApplication1.Models;
using System.Diagnostics;
using System.Net.Mail;

namespace PrefinalWebApplication1.Controllers
{
    public class StudentController : Controller
    {
        private static List<StudentViewModel> studentsList = new List<StudentViewModel>();

		public IActionResult Index()
		{
			StudentViewModel model = new StudentViewModel();
			using (var db = new StudInfoSysContext())
			{
				model.StudentsList = db.Students.ToList();

			}
			return View(model);
			//return View(studentsList);
		}

		[HttpPost]
		public IActionResult AddStudentInfo(StudentViewModel studentViewModel)
		{

			StudentViewModel resp = new StudentViewModel();
			if (ModelState.IsValid)
			{
				using (var db = new StudInfoSysContext())
				{
					studentsList.Add(studentViewModel);
					ModelState.Clear();
					{
						var newStudent = new Student
						{
							LastName = studentViewModel.LastName,
							FirstName = studentViewModel.FirstName,
							MiddleName = studentViewModel.MiddleName,
							Course = studentViewModel.Course,
							Section = studentViewModel.Section,
							EmailAddress = studentViewModel.EmailAddress,
							ContactNumber = studentViewModel.ContactNumber,
						};

						db.Students.Add(newStudent);
						db.SaveChanges();
					}
				}
			}
            using (var db = new StudInfoSysContext())
            {
				resp.StudentsList = db.Students.ToList();
            }

			return View("Index", resp);
        }

		
[HttpPost]
	public IActionResult EditAccount(StudentViewEditModel studentViewEditModel)
	{
		if (ModelState.IsValid)
			{
			using (var db = new StudInfoSysContext())
			{
				Student userToEdit = db.Students.Find(studentViewEditModel.StudentID);

				if (userToEdit != null)
				{
					userToEdit.LastName = studentViewEditModel.LastName;
					userToEdit.FirstName = studentViewEditModel.FirstName;
					userToEdit.MiddleName = studentViewEditModel.MiddleName;
					userToEdit.Course = studentViewEditModel.Course;
					userToEdit.Section = studentViewEditModel.Section;
					userToEdit.EmailAddress = studentViewEditModel.EmailAddress;
					userToEdit.ContactNumber = studentViewEditModel.ContactNumber;

					db.SaveChanges();
				}
			}
			}

			StudentViewModel resp = new StudentViewModel();
			using (var db = new StudInfoSysContext())
			{
				resp.StudentsList = db.Students.ToList();
			}

		return View("Index", resp);
	}

		public IActionResult DeleteStudentInfo(StudentViewDeleteModel studentViewDeleteModel)
		{
			if (ModelState.IsValid)
			{
				using (var db = new StudInfoSysContext())
				{
					Student userToDelete = db.Students.Find(studentViewDeleteModel.StudentID);
					if (userToDelete != null)
					{
						userToDelete.IsDeleted = 1;
						db.SaveChanges();
					}
				}
			}

			StudentViewDeleteModel resp = new StudentViewDeleteModel();
			using (var db = new StudInfoSysContext())
			{
				resp.StudentsList = db.Students.ToList();
			}
			return View("Student", resp);

		}
	}
}