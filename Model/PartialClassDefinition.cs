using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.IO;

namespace Model
{
    public partial class Operate_Result
    {
        public int IsSucceed { get; set; }

        private string _msgText;
        public string MsgText
        {
            get
            {
                if (IsSucceed == 0)
                {
                    _msgText = "操作失败";
                }
                else if (IsSucceed == 1 && string.IsNullOrEmpty(_msgText))
                {
                    _msgText = "操作成功";
                }

                return _msgText;
            }
            set { _msgText = value; }
        }

        public string RetInfo { get; set; }

        public Operate_Result()
        {
            IsSucceed = 0;
            RetInfo = string.Empty;
        }
    }

    public partial class XXXEntities : DbContext
    {
        public XXXEntities(bool isLog) : base("name=XXXEntities")
        {
            DbInterception.Add(new EFIntercepterLogging());
        }

        public XXXEntities(string efConnectionString) : base(efConnectionString)
        {

        }
    }
}
