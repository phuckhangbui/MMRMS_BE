//using BusinessObject;
//using Microsoft.EntityFrameworkCore;

//namespace DAO
//{
//    public class ContractSerialNumberProductDao : BaseDao<ContractSerialNumberProduct>
//    {
//        private static ContractSerialNumberProductDao instance = null;
//        private static readonly object instacelock = new object();

//        private ContractSerialNumberProductDao()
//        {

//        }

//        public static ContractSerialNumberProductDao Instance
//        {
//            get
//            {
//                if (instance == null)
//                {
//                    instance = new ContractSerialNumberProductDao();
//                }
//                return instance;
//            }
//        }

//        public async Task<IEnumerable<ContractSerialNumberProduct>> GetContractSerialNumberProductsByContractId(string contractId)
//        {
//            using (var context = new MmrmsContext())
//            {
//                return await context.ContractSerialNumberProducts
//                    .Where(c => c.ContractId == contractId)
//                    .Include(c => c.SerialNumberProduct)
//                    .ToListAsync();
//            }
//        }
//    }
//}
