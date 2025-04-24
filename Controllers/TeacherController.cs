using Microsoft.AspNetCore.Mvc;
using PrefinalWebApplication1.Models;

namespace PrefinalWebApplication1.Controllers
{
    public class TeacherController : Controller
    {
        private static List<TeacherViewModel> teachersList = new List<TeacherViewModel>();

		public IActionResult Index()
		{
			//var teacherModel = new TeacherViewModel();
			//using (var db = new StudInfoSysContext())
			//{
			//	teacherModel.TeachersList = db.Teachers.ToList();

			//}
			//return View(teacherModel);
			return View(teachersList);
		}

		[HttpPost]
		public IActionResult AddTeacher(TeacherViewModel teacherVM)
		{
			if (ModelState.IsValid)
			{
				teacherVM.TeacherID = teachersList.Count + 1;

				teachersList.Add(teacherVM);
				ModelState.Clear();

				return RedirectToAction("Index", "Teacher");
			}

			return View("Index", teachersList);
		}

		//[HttpPost]
		//      public IActionResult SaveTeacherInfo(TeacherViewModel newTeacher)
		//      {

		//      }

		//      [HttpGet]
		//      public IActionResult DeleteTeacherInfo(int teacherID)
		//      {

		//      }

		//      [HttpGet]
		//      public IActionResult EditTeacherInfo(int teacherID)
		//      {

		//      }

		//      [HttpPost]
		//      public IActionResult UpdateTeacherInfo(TeacherViewModel updatedTeacher)
		//      {

		//      }

		//      private void LoadTeacherInfo()
		//      {

		//      }
	}
}
