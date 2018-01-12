using System.Data.Entity;

namespace Model
{
    public class MyDbConfiguration : DbConfiguration
    {
        public MyDbConfiguration()
        {
            // 中文
            //SetDatabaseLogFormatter((context, writeAction) => new ChineseDatabaseLogFormatter(context, writeAction));
            // 英文
            SetDatabaseLogFormatter((context, writeAction) => new EnglishDatabaseLogFormatter(context, writeAction));
        }
    }
}
