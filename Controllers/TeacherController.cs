using Microsoft.AspNetCore.Mvc;
using PrefinalWebApplication1.Models;

namespace PrefinalWebApplication1.Controllers
{
    public class TeacherController : Controller
    {
        private static List<TeacherViewModel> teachersList = new List<TeacherViewModel>();
        private const string teacherInfoFolderPath = "PrototypeDB/TeacherInfoFolder";

        public TeacherController()
        {
            if (!Directory.Exists(teacherInfoFolderPath))
            {
                Directory.CreateDirectory(teacherInfoFolderPath);
            }

            LoadTeacherInfo();
        }

        public IActionResult Index()
        {
            var model = new TeacherViewModel
            {
                TeachersList = teachersList.OrderBy(teach => teach.TeacherID).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult SaveTeacherInfo(TeacherViewModel newTeacher)
        {
            if (newTeacher == null)
            {
                return RedirectToAction("Index");
            }

            var existingTeacher = teachersList.FirstOrDefault(teach => teach.TeacherID == newTeacher.TeacherID);

            if (existingTeacher != null)
            {
                existingTeacher.FirstName = newTeacher.FirstName;
                existingTeacher.MiddleName = newTeacher.MiddleName;
                existingTeacher.LastName = newTeacher.LastName;
                existingTeacher.Department = newTeacher.Department;
                existingTeacher.EmailAddress = newTeacher.EmailAddress;
                existingTeacher.ContactNumber = newTeacher.ContactNumber;
            }
            else
            {
                newTeacher.TeacherID = teachersList.Count + 1;newTeacher.TeacherID = teachersList.Any() ? teachersList.Max(t => t.TeacherID) + 1 : 1;
                teachersList.Add(newTeacher);
            }

            string filePath = Path.Combine(teacherInfoFolderPath, $"{newTeacher.TeacherID}.txt");
            string teacherData = $"{newTeacher.TeacherID},{newTeacher.FirstName},{newTeacher.MiddleName},{newTeacher.LastName},{newTeacher.Department},{newTeacher.EmailAddress},{newTeacher.ContactNumber}";
            System.IO.File.WriteAllText(filePath, teacherData);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteTeacherInfo(int teacherID)
        {
            var teacher = teachersList.FirstOrDefault(teach => teach.TeacherID == teacherID);
            if (teacher != null)
            {
                teachersList.Remove(teacher);
                string filePath = Path.Combine(teacherInfoFolderPath, $"{teacherID}.txt");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditTeacherInfo(int teacherID)
        {
            LoadTeacherInfo();

            string filePath = Path.Combine(teacherInfoFolderPath, $"{teacherID}.txt");

            if (System.IO.File.Exists(filePath))
            {
                string[] teacherData = System.IO.File.ReadAllText(filePath).Split(',');

                if (teacherData.Length >= 7)
                {
                    var teacher = new TeacherViewModel
                    {
                        TeacherID = int.Parse(teacherData[0]),
                        FirstName = teacherData[1],
                        MiddleName = teacherData[2],
                        LastName = teacherData[3],
                        Department = teacherData[4],
                        EmailAddress = teacherData[5],
                        ContactNumber = teacherData[6],
                    };

                    return View("EditTeacher", teacher);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateTeacherInfo(TeacherViewModel updatedTeacher)
        {
            string filePath = Path.Combine(teacherInfoFolderPath, $"{updatedTeacher.TeacherID}.txt");

            if (System.IO.File.Exists(filePath))
            {
                string teacherData = $"{updatedTeacher.TeacherID},{updatedTeacher.FirstName},{updatedTeacher.MiddleName},{updatedTeacher.LastName},{updatedTeacher.Department},{updatedTeacher.EmailAddress},{updatedTeacher.ContactNumber}";
                System.IO.File.WriteAllText(filePath, teacherData);
            }

            return RedirectToAction("Index");
        }

        private void LoadTeacherInfo()
        {
            teachersList.Clear();
            foreach (var file in Directory.GetFiles(teacherInfoFolderPath, "*.txt"))
            {
                string[] teacherData = System.IO.File.ReadAllText(file).Split(',');
                if (teacherData.Length >= 7)
                {
                    teachersList.Add(new TeacherViewModel
                    {
                       TeacherID = int.Parse(teacherData[0]),
                        FirstName = teacherData[1],
                        MiddleName = teacherData[2],
                        LastName = teacherData[3],
                        Department = teacherData[4],
                        EmailAddress = teacherData[5],
                        ContactNumber = teacherData[6],
                    });
                }
            }
            teachersList = teachersList.OrderBy(teach => teach.TeacherID).ToList();
        }
    }
}
