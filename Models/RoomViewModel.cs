namespace PrefinalWebApplication1.Models
{
    public class RoomViewModel
    {
        public int RoomID { get; set; }
        public string? RoomName { get; set; }
        public int Floor { get; set; }
        public string? Building { get; set; }

        public List<RoomViewModel> RoomsList { get; set; } = new List<RoomViewModel>();
    }
}
