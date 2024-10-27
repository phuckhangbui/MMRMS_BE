using BusinessObject;

namespace DAO
{
    public class MachineCheckCriteriaDao : BaseDao<MachineCheckCriteria>
    {
        private static MachineCheckCriteriaDao instance = null;
        private static readonly object instacelock = new object();

        private MachineCheckCriteriaDao()
        {

        }

        public static MachineCheckCriteriaDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MachineCheckCriteriaDao();
                }
                return instance;
            }
        }
    }
}
