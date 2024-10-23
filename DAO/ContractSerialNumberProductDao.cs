//using BusinessObject;
//using Microsoft.EntityFrameworkCore;

//namespace DAO
//{
//    public class ContractMachineSerialNumberDao : BaseDao<ContractMachineSerialNumber>
//    {
//        private static ContractMachineSerialNumberDao instance = null;
//        private static readonly object instacelock = new object();

//        private ContractMachineSerialNumberDao()
//        {

//        }

//        public static ContractMachineSerialNumberDao Instance
//        {
//            get
//            {
//                if (instance == null)
//                {
//                    instance = new ContractMachineSerialNumberDao();
//                }
//                return instance;
//            }
//        }

//        public async Task<IEnumerable<ContractMachineSerialNumber>> GetContractMachineSerialNumbersByContractId(string contractId)
//        {
//            using (var context = new MmrmsContext())
//            {
//                return await context.ContractMachineSerialNumbers
//                    .Where(c => c.ContractId == contractId)
//                    .Include(c => c.MachineSerialNumber)
//                    .ToListAsync();
//            }
//        }
//    }
//}
