using Microsoft.AspNetCore.Mvc;
using PrefinalWebApplication1.Models;

namespace PrefinalWebApplication1.Controllers
{
    public class SectionController : Controller
    {
        private static List<SectionViewModel> sectionsList = new List<SectionViewModel>();
        private const string sectionInfoFolderPath = "PrototypeDB/SectionInfoFolder";

        public SectionController()
        {
            if (!Directory.Exists(sectionInfoFolderPath))
            {
                Directory.CreateDirectory(sectionInfoFolderPath);
            }

            LoadSectionInfo();
        }

        public IActionResult Index()
        {
            var model = new SectionViewModel
            {
                SectionsList = sectionsList.OrderBy(sec => sec.SectionID).ToList()
            };

            return View(model);
        }

        //save user
        [HttpPost]
        public IActionResult SaveSectionInfo(SectionViewModel newSection)
        {
            if (newSection == null)
            {
                return RedirectToAction("Index");
            }

            var existingSection = sectionsList.FirstOrDefault(sec => sec.SectionID == newSection.SectionID);

            if (existingSection != null)
            {
                existingSection.SectionName = newSection.SectionName;
                existingSection.Course = newSection.Course;
                existingSection.YearLevel = newSection.YearLevel;
            }
            else
            {
                newSection.SectionID = sectionsList.Count + 1;
                sectionsList.Add(newSection);
            }

            string filePath = Path.Combine(sectionInfoFolderPath, $"{newSection.SectionID}.txt");
            string sectionData = $"{newSection.SectionID},{newSection.SectionName},{newSection.Course},{newSection.YearLevel}";
            System.IO.File.WriteAllText(filePath, sectionData);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteSectionInfo(int sectionID)
        {
            var user = sectionsList.FirstOrDefault(usr => usr.SectionID == sectionID);
            if (user != null)
            {
                sectionsList.Remove(user);
                string filePath = Path.Combine(sectionInfoFolderPath, $"{sectionID}.txt");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditSectionInfo(int sectionID)
        {
            LoadSectionInfo();

            string filePath = Path.Combine(sectionInfoFolderPath, $"{sectionID}.txt");

            if (System.IO.File.Exists(filePath))
            {
                string[] sectionData = System.IO.File.ReadAllText(filePath).Split(',');

                if (sectionData.Length >= 4)
                {
                    var section = new SectionViewModel
                    {
                        SectionID = int.Parse(sectionData[0]),
                        SectionName = sectionData[1],
                        Course = sectionData[2],
                        YearLevel = sectionData[3],
                    };

                    return View("EditSection", section);
                }
            }

            return RedirectToAction("Index");
        }

        // Update user details from editing
        [HttpPost]
        public IActionResult UpdateSectionInfo(SectionViewModel updatedSection)
        {
            string filePath = Path.Combine(sectionInfoFolderPath, $"{updatedSection.SectionID}.txt");

            if (System.IO.File.Exists(filePath))
            {
                string sectionData = $"{updatedSection.SectionID},{updatedSection.SectionName},{updatedSection.Course},{updatedSection.YearLevel}";
                System.IO.File.WriteAllText(filePath, sectionData);
            }

            return RedirectToAction("Index");
        }

        // Load users for viewing
        private void LoadSectionInfo()
        {
            sectionsList.Clear();
            foreach (var file in Directory.GetFiles(sectionInfoFolderPath, "*.txt"))
            {
                string[] sectionData = System.IO.File.ReadAllText(file).Split(',');
                if (sectionData.Length >= 4)
                {
                    sectionsList.Add(new SectionViewModel
                    {
                       SectionID = int.Parse(sectionData[0]),
                        SectionName = sectionData[1],
                        Course = sectionData[2],
                        YearLevel = sectionData[3],
                    });
                }
            }
            sectionsList = sectionsList.OrderBy(sec => sec.SectionID).ToList();
        }
    }
}
