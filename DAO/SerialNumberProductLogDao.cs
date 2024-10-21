using BusinessObject;

namespace DAO
{
    public class SerialNumberProductLogDao : BaseDao<SerialNumberProductLog>
    {
        private static SerialNumberProductLogDao instance = null;
        private static readonly object instacelock = new object();

        private SerialNumberProductLogDao()
        {

        }

        public static SerialNumberProductLogDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SerialNumberProductLogDao();
                }
                return instance;
            }
        }



    }
}
