using Microsoft.EntityFrameworkCore;
using TaskManagmentAPI.Model;

namespace TaskManagmentAPI.Database
{
    public class SqlConnectionDB: DbContext
    {
        public SqlConnectionDB(DbContextOptions<SqlConnectionDB> options) : base(options)
        {

        }

        public DbSet<TaskModel> Tasks { get; set; }
    }
}
