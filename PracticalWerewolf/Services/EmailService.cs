using log4net;
using Microsoft.AspNet.Identity;
using PracticalWerewolf.EmailTemplates;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using RazorEngine.Templating;


namespace PracticalWerewolf.Services
{
    public class EmailService : IIdentityMessageService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(EmailService));
        private static bool templateKeysInitialized = false;
        private static string faviconFilePath;

        public EmailService()
        {
            initTemplateKeys();
        }

        private void initTemplateKeys()
        {
            if (!templateKeysInitialized)
            {
                templateKeysInitialized = true;

                List<Tuple<string, string>> templateKeys = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("OrderUpdate", @"~\EmailTemplates\OrderUpdate.cshtml"),
                    new Tuple<string, string>("WorkOrder", @"~\EmailTemplates\WorkOrder.cshtml"),
                };
                try
                {
                    faviconFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/favicon.ico");
                    foreach (var keys in templateKeys)
                    {
                        var filePath = keys.Item2;
                        var fullPath = System.Web.HttpContext.Current.Server.MapPath(filePath);
                        var template = System.IO.File.ReadAllText(fullPath);
                        Engine.Razor.AddTemplate(keys.Item1, template);
                    }
                }
                catch
                {
                    templateKeysInitialized = false;
                    logger.Error("Error intializing email templates");
                }
            }
        }

        public async Task SendAsync(IdentityMessage message)
        {
            var email = new MailMessage();
            email.To.Add(new MailAddress(message.Destination));
            email.Subject = message.Subject;
            email.Body = message.Body;
            email.IsBodyHtml = true;

            try
            {
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(email);
                }

            }
            catch (Exception e)
            {
                var ex = e.InnerException;
                var errorMessage = "SendAsync() - Inner Exception: " + ex.Message + "\n" + "Stack Track: " + ex.StackTrace;
                logger.Error(errorMessage);
            }
        }

        public void Send(string destination, string subject, string body, List<Tuple<string, string>> imageIds = null)
        {
            var email = new MailMessage();
            email.To.Add(new MailAddress(destination));
            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;

            AlternateView view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

            if (imageIds != null)
            {
                foreach (var imageResource in imageIds)
                {
                    var image = new LinkedResource(imageResource.Item1);
                    image.ContentId = imageResource.Item2;
                    view.LinkedResources.Add(image);
                }

                email.AlternateViews.Add(view);
            }
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(email);
                }

            }
            catch (Exception e)
            {
                var ex = e.InnerException;
                var errorMessage = "Send() - Inner Exception: " + ex.Message + "\n" + "Stack Track: " + ex.StackTrace;
                logger.Error(errorMessage);
            }
        }

        public async Task SendAsync(string destination, string subject, string body, List<Tuple<string, string>> imageIds = null)
        {
            var email = new MailMessage();
            email.To.Add(new MailAddress(destination));
            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;

            AlternateView view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

            if (imageIds != null)
            {
                foreach (var imageResource in imageIds)
                {
                    var image = new LinkedResource(imageResource.Item1);
                    image.ContentId = imageResource.Item2;
                    view.LinkedResources.Add(image);
                }

                email.AlternateViews.Add(view);
            }
            try
            {
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(email);
                }

            }
            catch (Exception e)
            {
                var ex = e.InnerException;
                var errorMessage = "SendAsync() - Inner Exception: " + ex.Message + "\n" + "Stack Track: " + ex.StackTrace;
                logger.Error(errorMessage);
            }
        }

        public async Task SendOrderConfirmEmail(OrderRequestInfo order, ApplicationUser user)
        {
            initTemplateKeys();
            if (templateKeysInitialized)
            {
                OrderUpdateModel model = new OrderUpdateModel
                {
                    UserName = user.UserName,
                    OrderId = order.OrderRequestInfoGuid,
                    ArrivalDate = "In the near future",         //todo calculate this
                    Cost = "Lotsa Moneeyyy",                    //todo calculate this
                    Destination = order.DropOffAddress.RawInputAddress,
                    UpdateType = OrderUpdateType.Order,
                    UpdateDescription = "has been placed!",
                    LogoId = Guid.Empty.ToString()
                };

                string subject = "Order Confirmation";

                await SendOrderUpdateEmail(model, user.Email, subject);
            }
        }

        public async Task SendOrderShippedEmail(OrderRequestInfo order, ApplicationUser user)
        {
            initTemplateKeys();
            if (templateKeysInitialized)
            {
                OrderUpdateModel model = new OrderUpdateModel
                {
                    UserName = user.UserName,
                    OrderId = order.OrderRequestInfoGuid,
                    ArrivalDate = "In the near future",         //todo calculate this
                    Cost = "Lotsa Moneeyyy",                    //todo calculate this
                    Destination = order.DropOffAddress.RawInputAddress,
                    UpdateType = OrderUpdateType.Shipping,
                    UpdateDescription = "has shipped!",
                    LogoId = Guid.Empty.ToString()
                };

                string subject = "Your Order Has Shipped!";

                await SendOrderUpdateEmail(model, user.Email, subject);
            }
        }

        public async Task SendOrderDeliveredEmail(OrderRequestInfo order, ApplicationUser user)
        {
            initTemplateKeys();
            if (templateKeysInitialized)
            {
                OrderUpdateModel model = new OrderUpdateModel
                {
                    UserName = user.UserName,
                    OrderId = order.OrderRequestInfoGuid,
                    ArrivalDate = "In the near future",         //todo calculate this
                    Cost = "Lotsa Moneeyyy",                    //todo calculate this
                    Destination = order.DropOffAddress.RawInputAddress,
                    UpdateType = OrderUpdateType.Delivery,
                    UpdateDescription = "has been delivered!",
                    LogoId = Guid.Empty.ToString()
                };

                string subject = "Your Order Has Been Delivered";

                await SendOrderUpdateEmail(model, user.Email, subject);
            }
        }

        private async Task SendOrderUpdateEmail(OrderUpdateModel model, string email, string subject, List<Tuple<string, string>> imageIds = null)
        {
            if (imageIds == null)
            {
                imageIds = new List<Tuple<string, string>>();
            }

            imageIds.Add(new Tuple<string, string>(faviconFilePath, Guid.Empty.ToString()));
            var result = Engine.Razor.RunCompile("OrderUpdate", typeof(OrderUpdateModel), model);

            int index = result.IndexOf("<!DOCTYPE html>");
            result = result.Substring(index);

            await SendAsync(email, subject, result, imageIds);
        }

        public void SendWorkOrderEmail(ApplicationUser contractor, OrderRequestInfo order)
        {
            if (contractor == null)
            {
                throw new Exception("Null contractor");
            }

            initTemplateKeys();
            if (templateKeysInitialized)
            {
                WorkOrderModel model = new WorkOrderModel
                {
                    ContractorUserName = contractor.UserName,
                    DropOffAddress = order.DropOffAddress.RawInputAddress,
                    PickUpAddress = order.PickUpAddress.RawInputAddress,
                    LogoId = Guid.Empty.ToString()
                };
                string subject = "Work Order Request";

                SendWorkOrderEmail(model, contractor.Email, subject);
            }
        }

        private void SendWorkOrderEmail(WorkOrderModel model, string email, string subject, List<Tuple<string, string>> imageIds = null)
        {
            if (imageIds == null)
            {
                imageIds = new List<Tuple<string, string>>();
            }

            imageIds.Add(new Tuple<string, string>(faviconFilePath, Guid.Empty.ToString()));
            var result = Engine.Razor.RunCompile("WorkOrder", typeof(WorkOrderModel), model);

            int index = result.IndexOf("<!DOCTYPE html>");
            result = result.Substring(index);

            Send(email, subject, result, imageIds);
        }

    }
}