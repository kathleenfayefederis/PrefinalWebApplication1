using Microsoft.EntityFrameworkCore;

namespace PrefinalWebApplication1.Models
{
    public partial class StudInfoSysContext
    {
        public StudInfoSysContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-792M68U\\SQLEXPRESS;Initial Catalog=StudInfoSys;Integrated Security=True;Trust Server Certificate=True");
        }
    }
}
