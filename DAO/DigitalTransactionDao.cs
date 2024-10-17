using BusinessObject;

namespace DAO
{
    public class DigitalTransactionDao : BaseDao<DigitalTransaction>
    {
        private static DigitalTransactionDao instance = null;
        private static readonly object instacelock = new object();

        private DigitalTransactionDao()
        {

        }

        public static DigitalTransactionDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DigitalTransactionDao();
                }
                return instance;
            }
        }
    }
}
