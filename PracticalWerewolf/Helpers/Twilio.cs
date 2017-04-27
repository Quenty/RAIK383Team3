using System.Threading.Tasks;
using PracticalWerewolf;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Web.Configuration;

namespace PracticalWerewolf.Helpers
{
    public interface ITwilioMessageSender
    {
        Task SendMessageAsync(string to, string from, string body);
    }
    public class TwilioMessageSender : ITwilioMessageSender
    {
        public TwilioMessageSender()
        {
            TwilioClient.Init(Config.AccountSid, Config.AuthToken);
        }

        public async Task SendMessageAsync(string to, string from, string body)
        {
            await MessageResource.CreateAsync(new PhoneNumber(to),
                                              from: new PhoneNumber(from),
                                              body: body);
        }
    }

    public class Config
    {
        public static string AccountSid => WebConfigurationManager.AppSettings["AccountSid"] ??
                                           "ACe3a619c07f08c0675bedeabebeeb7f41";

        public static string AuthToken => WebConfigurationManager.AppSettings["AuthToken"] ??
                                          "1dfc104619430da7781c33e4738acc61";

        public static string TwilioNumber => WebConfigurationManager.AppSettings["TwilioNumber"] ??
                                             "+16057895908";
    }

}