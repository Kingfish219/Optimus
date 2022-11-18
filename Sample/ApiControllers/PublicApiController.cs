using System.Web.Http;

namespace Sample.ApiControllers
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
