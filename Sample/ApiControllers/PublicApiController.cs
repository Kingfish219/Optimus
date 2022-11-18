using System.Web.Http;

namespace CoCoNuT.ApiControllers
{
    [RoutePrefix("api/public")]
    public class PublicApiController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Test()
        {
            return Ok();
        }
    }
}
