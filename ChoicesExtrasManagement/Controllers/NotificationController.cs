using Microsoft.AspNetCore.Mvc;
using System.Configuration;

using Twilio;
using Twilio.AspNet.Core;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.Types;

namespace ChoicesExtrasManagement.Controllers
{
    public class NotificationController : TwilioController
    {
        private readonly IConfiguration _configuration;

        public NotificationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string SendSMSToBuyer()
        {
            var accountSID = _configuration["Twilio-accountSID"];
            var authToken = _configuration["Twilio-authToken"];

            TwilioClient.Init(accountSID, authToken);

            var from = _configuration["Twilio-fromNumber"];
            var to = new PhoneNumber("+447436972016");

            var message = MessageResource.Create(to:to, from:from, body:"Notification from Modern Homes, Please check the updates for current choices and extras ");

            return message.Sid;
        }
    }
}
