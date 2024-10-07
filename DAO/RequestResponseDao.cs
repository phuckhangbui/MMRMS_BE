using BusinessObject;

namespace DAO
{
    public class RequestResponseDao : BaseDao<RequestResponse>
    {
        private static RequestResponseDao instance = null;
        private static readonly object instacelock = new object();

        private RequestResponseDao()
        {

        }

        public static RequestResponseDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RequestResponseDao();
                }
                return instance;
            }
        }
    }
}
