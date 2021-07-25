using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BTTH.Models
{
    public class Model_Project_Context : DbContext
    {
        public Model_Project_Context () : base ("Model_Project_Context")  {
            
            }

        public System.Data.Entity.DbSet<BTTH.Models.Client> Clients { get; set; }

        public System.Data.Entity.DbSet<BTTH.Models.Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<BTTH.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<BTTH.Models.AssignTask> AssignTasks { get; set; }
    }
}