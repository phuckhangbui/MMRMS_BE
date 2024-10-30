//using BusinessObject;
//using Microsoft.EntityFrameworkCore;

//namespace DAO
//{
//    public class RequestResponseDao : BaseDao<RequestResponse>
//    {
//        private static RequestResponseDao instance = null;
//        private static readonly object instacelock = new object();

//        private RequestResponseDao()
//        {

//        }

//        public static RequestResponseDao Instance
//        {
//            get
//            {
//                if (instance == null)
//                {
//                    instance = new RequestResponseDao();
//                }
//                return instance;
//            }
//        }

//        public async Task<RequestResponse> GetRequestResponse(int requestResponseId)
//        {
//            using (var context = new MmrmsContext())
//            {
//                return await context.RequestResponses
//                    .FirstOrDefaultAsync(s => s.RequestResponseId == requestResponseId);
//            }
//        }
//    }
//}
