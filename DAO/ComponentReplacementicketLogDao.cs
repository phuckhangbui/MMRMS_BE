using BusinessObject;

namespace DAO
{
    public class ComponentReplacementicketLogDao : BaseDao<ComponentReplacementTicketLog>
    {
        private static ComponentReplacementicketLogDao instance = null;
        private static readonly object instacelock = new object();

        private ComponentReplacementicketLogDao()
        {

        }

        public static ComponentReplacementicketLogDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ComponentReplacementicketLogDao();
                }
                return instance;
            }
        }
    }
}
