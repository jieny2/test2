using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace EFDAL
{
    public class UserDAL
    {
        private XXXEntities db = (XXXEntities)DbContextFactory.GetCurrentDbContext();

        public void Add(T_SYS_User entity)
        {
            var x = from a in db.T_SYS_User
                    join b in db.T_EMP_Employee on a.ID equals b.ID
                    select new { a.Name, b.ID };
            x.ToList();
            db.T_SYS_User.Add(entity);
            db.SaveChanges();
        }

        public List<T_SYS_User> GetEntitysAll()
        {
            return db.T_SYS_User.Select(item => item).ToList();
        }
    }
}
