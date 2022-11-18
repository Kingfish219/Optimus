using CoCoNuT.Attributes;
using CoCoNuT.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;

namespace CoCoNuT.ApiControllers
{
    [RoutePrefix("api/public")]
    public class PublicApiController : ApiController
    {
        private readonly EventLog _logger;
        private readonly ValidationManager _validationManager;

        public PublicApiController()
        {
            _logger = new EventLog { Source = "KasraValidation" };
            _validationManager = new ValidationManager(_logger);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Test()
        {
            return Ok();
        }

        [Route("validate")]
        [HttpPost]
        [Symmetric(InputParameterName = "transferDataModel", InputType = typeof(TransferDataModel))]
        [CoconutAuthorize]
        public async Task<IHttpActionResult> Validate(TransferDataModel transferDataModel)
        {
            var validation = await _validationManager.Validate(transferDataModel);

            //var lockManager = new Manager();
            //var parameter = JsonConvert.DeserializeObject(lockManager.DecryptFromString(payload.Message), typeof(TransferDataModel));

            //var res = JsonConvert.SerializeObject(parameter);

            //var response = new Payload
            //{
            //    Message = lockManager.EncryptToString(res)
            //};

            //return Ok(response);


            return Ok(validation);
        }
    }
}
