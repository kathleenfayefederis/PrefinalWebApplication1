using Microsoft.AspNetCore.Mvc;
using PrefinalWebApplication1.Models;

namespace PrefinalWebApplication1.Controllers
{
    public class RoomController : Controller
    {
        private static List<RoomViewModel> roomsList = new List<RoomViewModel>();
        private const string roomInfoFolderPath = "PrototypeDB/RoomInfoFolder";

        public RoomController()
        {
            if (!Directory.Exists(roomInfoFolderPath))
            {
                Directory.CreateDirectory(roomInfoFolderPath);
            }

            LoadRoomInfo();
        }

        public IActionResult Index()
        {
            var model = new RoomViewModel
            {
                RoomsList = roomsList.OrderBy(sec => sec.RoomID).ToList()
            };

            return View(model);
        }

        //save user
        [HttpPost]
        public IActionResult SaveRoomInfo(RoomViewModel updatedRoom)
        {
            if (updatedRoom == null)
            {
                return RedirectToAction("Index");
            }

            var existingSection = roomsList.FirstOrDefault(rm => rm.RoomID == updatedRoom.RoomID);

            if (existingSection != null)
            {
                existingSection.RoomName = updatedRoom.RoomName;
                existingSection.Floor = updatedRoom.Floor;
                existingSection.Building = updatedRoom.Building;
            }
            else
            {
                updatedRoom.RoomID = roomsList.Count + 1;
                roomsList.Add(updatedRoom);
            }

            string filePath = Path.Combine(roomInfoFolderPath, $"{updatedRoom.RoomID}.txt");
            string roomData = $"{updatedRoom.RoomID},{updatedRoom.RoomName},{updatedRoom.Floor},{updatedRoom.Building}";
            System.IO.File.WriteAllText(filePath, roomData);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteRoomInfo(int roomID)
        {
            var user = roomsList.FirstOrDefault(usr => usr.RoomID == roomID);
            if (user != null)
            {
                roomsList.Remove(user);
                string filePath = Path.Combine(roomInfoFolderPath, $"{roomID}.txt");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditRoomInfo(int roomID)
        {
            LoadRoomInfo();

            string filePath = Path.Combine(roomInfoFolderPath, $"{roomID}.txt");

            if (System.IO.File.Exists(filePath))
            {
                string[] roomData = System.IO.File.ReadAllText(filePath).Split(',');

                if (roomData.Length >= 4)
                {
                    var room = new RoomViewModel
                    {
                        RoomID = int.Parse(roomData[0]),
                        RoomName = roomData[1],
                        Floor = int.Parse(roomData[2]),
                        Building = roomData[3],
                    };

                    return View("EditRoom", room);
                }
            }

            return RedirectToAction("Index");
        }

        // Update user details from editing
        [HttpPost]
        public IActionResult UpdateRoomInfo(RoomViewModel updatedRoom)
        {
            string filePath = Path.Combine(roomInfoFolderPath, $"{updatedRoom.RoomID}.txt");

            if (System.IO.File.Exists(filePath))
            {
                string roomData = $"{updatedRoom.RoomID},{updatedRoom.RoomName},{updatedRoom.Floor},{updatedRoom.Building}";
                System.IO.File.WriteAllText(filePath, roomData);
            }

            return RedirectToAction("Index");
        }

        // Load users for viewing
        private void LoadRoomInfo()
        {
            roomsList.Clear();
            foreach (var file in Directory.GetFiles(roomInfoFolderPath, "*.txt"))
            {
                string[] roomData = System.IO.File.ReadAllText(file).Split(',');
                if (roomData.Length >= 4)
                {
                    roomsList.Add(new RoomViewModel
                    {
                        RoomID = int.Parse(roomData[0]),
                        RoomName = roomData[1],
                        Floor = int.Parse(roomData[2]),
                        Building = roomData[3],
                    });
                }
            }
            roomsList = roomsList.OrderBy(sec => sec.RoomID).ToList();
        }
    }
}
