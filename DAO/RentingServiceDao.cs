using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class RentingServiceDao : BaseDao<RentingService>
    {
        private static RentingServiceDao instance = null;
        private static readonly object instacelock = new object();

        private RentingServiceDao()
        {

        }

        public static RentingServiceDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RentingServiceDao();
                }
                return instance;
            }
        }
    }
}
