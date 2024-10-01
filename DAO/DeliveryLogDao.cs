using BusinessObject;

namespace DAO
{
    public class DeliveryLogDao : BaseDao<DeliveryLog>
    {
        private static DeliveryLogDao instance = null;
        private static readonly object instacelock = new object();

        private DeliveryLogDao()
        {

        }

        public static DeliveryLogDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DeliveryLogDao();
                }
                return instance;
            }
        }
    }
}
