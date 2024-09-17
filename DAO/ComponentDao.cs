using BusinessObject;

namespace DAO
{
    public class ComponentDao : BaseDao<Component>
    {
        private static ComponentDao instance = null;
        private static readonly object instacelock = new object();

        private ComponentDao()
        {

        }

        public static ComponentDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ComponentDao();
                }
                return instance;
            }
        }
    }
}
