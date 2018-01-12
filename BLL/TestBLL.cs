using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL
{
    public class TestBLL
    {
        public void Test()
        {
            EFDAL.UserDAL userDAL = new EFDAL.UserDAL();
            userDAL.Add(new T_SYS_User());
        }
    }
}
