using BusinessObject;

namespace DAO
{
    public class ProductImageDao : BaseDao<ProductImage>
    {
        private static ProductImageDao instance = null;
        private static readonly object instacelock = new object();

        private ProductImageDao()
        {

        }

        public static ProductImageDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductImageDao();
                }
                return instance;
            }
        }
    }
}
