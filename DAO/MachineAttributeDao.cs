using BusinessObject;

namespace DAO
{
    public class MachineAttributeDao : BaseDao<MachineAttribute>
    {
        private static MachineAttributeDao instance = null;
        private static readonly object instacelock = new object();

        private MachineAttributeDao()
        {

        }

        public static MachineAttributeDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MachineAttributeDao();
                }
                return instance;
            }
        }
    }
}
