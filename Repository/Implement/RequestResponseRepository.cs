using Repository.Interface;

namespace Repository.Implement
{
    public class RequestResponseRepository : IRequestResponseRepository
    {
        public async Task CreateResponeWhenCheckMachineTaskSuccess(int requestResponseId)
        {
            //var requestResponse = await RequestResponseDao.Instance.GetRequestResponse(requestResponseId);

            //if (requestResponse == null)
            //{
            //    return;
            //}

            //var newResponse = new RequestResponse
            //{
            //    MachineCheckRequestId = requestResponse.MachineCheckRequestId,

            //}
        }
    }
}
