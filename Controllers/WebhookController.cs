using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet_CLI_Tools;
using WebhookDemo.Business;

namespace WebhookDemo.Controllers
{
    public class WebhookController : HookController
    {
        public override IActionResult Receive()
        {
            JObject jsonObject;

            using (var reader = new StreamReader(Request.Body))
            {
                jsonObject = JObject.Parse(reader.ReadToEnd());
            }

            return HookHandler.Handle(jsonObject) ? new OkResult() : new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}