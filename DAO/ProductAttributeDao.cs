using BusinessObject;

namespace DAO
{
    public class ProductAttributeDao : BaseDao<ProductAttribute>
    {
        private static ProductAttributeDao instance = null;
        private static readonly object instacelock = new object();

        private ProductAttributeDao()
        {

        }

        public static ProductAttributeDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductAttributeDao();
                }
                return instance;
            }
        }
    }
}
