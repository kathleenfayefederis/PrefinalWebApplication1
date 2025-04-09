using Microsoft.AspNetCore.Mvc;
using PrefinalWebApplication1.Models;

namespace PrefinalWebApplication1.Controllers
{
    public class StudentController : Controller
    {
        private static List<StudentViewModel> studentsList = new List<StudentViewModel>();
        private const string studentInfoFolderPath = "PrototypeDB/StudentInfoFolder";

        public StudentController()
        {
            if (!Directory.Exists(studentInfoFolderPath))
            {
                Directory.CreateDirectory(studentInfoFolderPath);
            }

            LoadStudents();
        }

        public IActionResult Index()
        {
            var model = new StudentViewModel
            {
                StudentsList = studentsList.OrderBy(stud => stud.StudentID).ToList()
            };

            return View(model);
        }

        //save user
        [HttpPost]
        public IActionResult SaveStudentInfo(StudentViewModel newStudent)
        {
            if (newStudent == null)
            {
                return RedirectToAction("Index");
            }

            var existingStudent = studentsList.FirstOrDefault(stud => stud.StudentID == newStudent.StudentID);

            if (existingStudent != null)
            {
                // Update existing user
                existingStudent.FirstName = newStudent.FirstName;
                existingStudent.MiddleName = newStudent.MiddleName;
                existingStudent.LastName = newStudent.LastName;
                existingStudent.Course = newStudent.Course;
                existingStudent.Section = newStudent.Section;
                existingStudent.EmailAddress = newStudent.EmailAddress;
                existingStudent.ContactNumber = newStudent.ContactNumber;
            }
            else
            {
                // Create new user
                newStudent.StudentID = studentsList.Count + 1;
                studentsList.Add(newStudent);
            }

            string filePath = Path.Combine(studentInfoFolderPath, $"{newStudent.StudentID}.txt");
            string studentData = $"{newStudent.StudentID},{newStudent.FirstName},{newStudent.MiddleName},{newStudent.LastName},{newStudent.Course},{newStudent.Section},{newStudent.EmailAddress},{newStudent.ContactNumber}";
            System.IO.File.WriteAllText(filePath, studentData);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteStudentInfo(int studentID)
        {
            var user = studentsList.FirstOrDefault(usr => usr.StudentID == studentID);
            if (user != null)
            {
                studentsList.Remove(user);
                string filePath = Path.Combine(studentInfoFolderPath, $"{studentID}.txt");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditStudentInfo(int studentID)
        {
            LoadStudents();

            string filePath = Path.Combine(studentInfoFolderPath, $"{studentID}.txt");

            if (System.IO.File.Exists(filePath))
            {
                string[] studentData = System.IO.File.ReadAllText(filePath).Split(',');

                if (studentData.Length >= 8)
                {
                    var student = new StudentViewModel
                    {
                        StudentID = int.Parse(studentData[0]),
                        FirstName = studentData[1],
                        MiddleName = studentData[2],
                        LastName = studentData[3],
                        Course = studentData[4],
                        Section = studentData[5],
                        EmailAddress = studentData[6],
                        ContactNumber = studentData[7],
                    };

                    return View("EditStudent", student);
                }
            }

            return RedirectToAction("Index");
        }

        // Update user details from editing
        [HttpPost]
        public IActionResult UpdateStudent(StudentViewModel updatedStudent)
        {
            string filePath = Path.Combine(studentInfoFolderPath, $"{updatedStudent.StudentID}.txt");

            if (System.IO.File.Exists(filePath))
            {
                string studentData = $"{updatedStudent.StudentID},{updatedStudent.FirstName},{updatedStudent.MiddleName},{updatedStudent.LastName},{updatedStudent.Course},{updatedStudent.Section},{updatedStudent.EmailAddress},{updatedStudent.ContactNumber}";
                System.IO.File.WriteAllText(filePath, studentData);
            }

            return RedirectToAction("Index");
        }

        // Load users for viewing
        private void LoadStudents()
        {
            studentsList.Clear();
            foreach (var file in Directory.GetFiles(studentInfoFolderPath, "*.txt"))
            {
                string[] studentData = System.IO.File.ReadAllText(file).Split(',');
                if (studentData.Length >= 8)
                {
                    studentsList.Add(new StudentViewModel
                    {
                        StudentID = int.Parse(studentData[0]),
                        FirstName = studentData[1],
                        MiddleName = studentData[2],
                        LastName = studentData[3],
                        Course = studentData[4],
                        Section = studentData[5],
                        EmailAddress = studentData[6],
                        ContactNumber = studentData[7],
                    });
                }
            }
            studentsList = studentsList.OrderBy(stud => stud.StudentID).ToList();
        }

    }
}
