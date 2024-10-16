using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected int GetLoginAccountId()
        {
            try
            {
                return int.Parse(this.User.Claims.First(i => i.Type == "AccountId").Value);
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        protected int GetLoginAccounRole()
        {
            try
            {
                return int.Parse(this.User.Claims.First(i => i.Type == "RoleId").Value);
            }
            catch (Exception e)
            {
                return -1;
            }
        }
    }
}
