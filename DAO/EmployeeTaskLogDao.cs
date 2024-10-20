using BusinessObject;

namespace DAO
{
    public class MachineTaskLogDao : BaseDao<MachineTaskLog>
    {
        private static MachineTaskLogDao instance = null;
        private static readonly object instacelock = new object();

        private MachineTaskLogDao()
        {

        }

        public static MachineTaskLogDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MachineTaskLogDao();
                }
                return instance;
            }
        }
    }
}
