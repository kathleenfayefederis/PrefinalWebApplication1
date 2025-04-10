using Microsoft.AspNetCore.Mvc;
using PrefinalWebApplication1.Models;

namespace PrefinalWebApplication1.Controllers
{
    public class CourseController : Controller
    {
        private static List<CourseViewModel> coursesList = new List<CourseViewModel>();
        private const string courseInfoFolderPath = "PrototypeDB/CourseInfoFolder";

        public CourseController()
        {
            if (!Directory.Exists(courseInfoFolderPath))
            {
                Directory.CreateDirectory(courseInfoFolderPath);
            }

            LoadCourseInfo();
        }

        public IActionResult Index()
        {
            var model = new CourseViewModel
            {
                CoursesList = coursesList.OrderBy(crs => crs.CourseID).ToList()
            };

            return View(model);
        }

        //save user
        [HttpPost]
        public IActionResult SaveCourseInfo(CourseViewModel updatedCourse)
        {
            if (updatedCourse == null)
            {
                return RedirectToAction("Index");
            }

            var existingCourse = coursesList.FirstOrDefault(rm => rm.CourseID == updatedCourse.CourseID);

            if (existingCourse != null)
            {
                existingCourse.CourseID = updatedCourse.CourseID;
                existingCourse.CourseName = updatedCourse.CourseName;
                existingCourse.CourseDuration = updatedCourse.CourseDuration;
                existingCourse.CourseDepartment = updatedCourse.CourseDepartment;
            }
            else
            {
                updatedCourse.CourseID = coursesList.Count + 1;
                coursesList.Add(updatedCourse);
            }

            string filePath = Path.Combine(courseInfoFolderPath, $"{updatedCourse.CourseID}.txt");
            string courseData = $"{updatedCourse.CourseID},{updatedCourse.CourseName},{updatedCourse.CourseDuration},{updatedCourse.CourseDepartment}";
            System.IO.File.WriteAllText(filePath, courseData);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteCourseInfo(int courseID)
        {
            var user = coursesList.FirstOrDefault(usr => usr.CourseID == courseID);
            if (user != null)
            {
                coursesList.Remove(user);
                string filePath = Path.Combine(courseInfoFolderPath, $"{courseID}.txt");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditCourseInfo(int courseID)
        {
            LoadCourseInfo();

            string filePath = Path.Combine(courseInfoFolderPath, $"{courseID}.txt");

            if (System.IO.File.Exists(filePath))
            {
                string[] courseData = System.IO.File.ReadAllText(filePath).Split(',');

                if (courseData.Length >= 4)
                {
                    var course = new CourseViewModel
                    {
                        CourseID = int.Parse(courseData[0]),
                        CourseName = courseData[1],
                        CourseDuration = int.Parse(courseData[2]),
                        CourseDepartment = courseData[3],
                    };

                    return View("EditCourse", course);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCourseInfo(CourseViewModel updatedCourse)
        {
            string filePath = Path.Combine(courseInfoFolderPath, $"{updatedCourse.CourseID}.txt");

            if (System.IO.File.Exists(filePath))
            {
                string courseData = $"{updatedCourse.CourseID},{updatedCourse.CourseName},{updatedCourse.CourseDuration},{updatedCourse.CourseDepartment}";
                System.IO.File.WriteAllText(filePath, courseData);
            }

            return RedirectToAction("Index");
        }

        private void LoadCourseInfo()
        {
            coursesList.Clear();
            foreach (var file in Directory.GetFiles(courseInfoFolderPath, "*.txt"))
            {
                string[] courseData = System.IO.File.ReadAllText(file).Split(',');
                if (courseData.Length >= 4)
                {
                    coursesList.Add(new CourseViewModel
                    {
                        CourseID = int.Parse(courseData[0]),
                        CourseName = courseData[1],
                        CourseDuration = int.Parse(courseData[2]),
                        CourseDepartment = courseData[3],
                    });
                }
            }
            coursesList = coursesList.OrderBy(sec => sec.CourseID).ToList();
        }
    }
}
