using BusinessObject;

namespace DAO
{
    public class ProductNumberDao : BaseDao<SerialNumberProduct>
    {
        private static ProductNumberDao instance = null;
        private static readonly object instacelock = new object();

        private ProductNumberDao()
        {

        }

        public static ProductNumberDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductNumberDao();
                }
                return instance;
            }
        }
    }
}
