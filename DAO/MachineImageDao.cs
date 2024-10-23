using BusinessObject;

namespace DAO
{
    public class MachineImageDao : BaseDao<MachineImage>
    {
        private static MachineImageDao instance = null;
        private static readonly object instacelock = new object();

        private MachineImageDao()
        {

        }

        public static MachineImageDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MachineImageDao();
                }
                return instance;
            }
        }
    }
}
