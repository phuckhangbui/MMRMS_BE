using BusinessObject;

namespace DAO
{
    public class AccountBusinessDao : BaseDao<AccountBusiness>
    {
        private static AccountBusinessDao instance = null;
        private static readonly object instacelock = new object();

        private AccountBusinessDao()
        {

        }

        public static AccountBusinessDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountBusinessDao();
                }
                return instance;
            }
        }
    }
}
