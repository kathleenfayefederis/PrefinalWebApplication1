using Microsoft.EntityFrameworkCore;

namespace PrefinalWebApplication1.Models
{
    public partial class StudInfoSysContext
    {
        public StudInfoSysContext()
        {

        }

		public object Teacher { get; internal set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

			//laptop: DESKTOP-D5DOJRU\SQLEXPRESS
			//lab computer: DESKTOP-792M68U\\SQLEXPRESS
			optionsBuilder.UseSqlServer("Data Source=DESKTOP-D5DOJRU\\SQLEXPRESS;Initial Catalog=StudInfoSys;Integrated Security=True;Trust Server Certificate=True");
        }
    }
}
