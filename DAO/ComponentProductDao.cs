using BusinessObject;

namespace DAO
{
    public class ComponentProductDao : BaseDao<ComponentProduct>
    {
        private static ComponentProductDao instance = null;
        private static readonly object instacelock = new object();

        private ComponentProductDao()
        {

        }

        public static ComponentProductDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ComponentProductDao();
                }
                return instance;
            }
        }
    }
}
