using System.Data.Entity;
using System.Runtime.Remoting.Messaging;
using Model;

namespace EFDAL
{
    public class DbContextFactory
    {
        public static DbContext GetCurrentDbContext()
        {
            // 一次请求共用一个实例
            DbContext db = CallContext.GetData("DbContext") as DbContext;
            if (db == null)
            {
                db = new XXXEntities(true);
                CallContext.SetData("DbContext", db);
            }

            return db;
        }
    }
}
