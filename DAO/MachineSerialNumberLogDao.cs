using BusinessObject;

namespace DAO
{
    public class MachineSerialNumberLogDao : BaseDao<MachineSerialNumberLog>
    {
        private static MachineSerialNumberLogDao instance = null;
        private static readonly object instacelock = new object();

        private MachineSerialNumberLogDao()
        {

        }

        public static MachineSerialNumberLogDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MachineSerialNumberLogDao();
                }
                return instance;
            }
        }



    }
}
