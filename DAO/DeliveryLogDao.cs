using BusinessObject;

namespace DAO
{
    public class DeliveryTaskLogDao : BaseDao<DeliveryTaskLog>
    {
        private static DeliveryTaskLogDao instance = null;
        private static readonly object instacelock = new object();

        private DeliveryTaskLogDao()
        {

        }

        public static DeliveryTaskLogDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DeliveryTaskLogDao();
                }
                return instance;
            }
        }
    }
}
