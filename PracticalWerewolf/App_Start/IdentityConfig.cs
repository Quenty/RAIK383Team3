﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PracticalWerewolf.Models;
using System.Net.Mail;
using log4net;
using System.Collections.Generic;
using PracticalWerewolf.Helpers;
using PracticalWerewolf.Application;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.EmailTemplates;
using RazorEngine;
using RazorEngine.Templating;
using System.Web;

namespace PracticalWerewolf
{
    public class EmailService : IIdentityMessageService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(EmailService));
        private static bool templateKeysInitialized = false;
        private static string faviconFilePath;
        private static string url;
        private static string detailsPageUrl = "/Order/Order/";

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
                    url = HttpContext.Current.Request.Url.Authority;
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

        public async Task SendOrderConfirmEmail(Order order, ApplicationUser user, decimal cost)
        {
            initTemplateKeys();
            if (templateKeysInitialized)
            {
                OrderUpdateModel model = new OrderUpdateModel
                {
                    UserName = user.UserName,
                    OrderId = order.RequestInfo.OrderRequestInfoGuid,
                    Cost = string.Format("${0:0.00}", cost),
                    PickUpAddress = order.RequestInfo.PickUpAddress.RawInputAddress,
                    Destination = order.RequestInfo.DropOffAddress.RawInputAddress,
                    UpdateType = OrderUpdateType.Order,
                    UpdateDescription = "has been placed!",
                    LogoId = Guid.Empty.ToString(),
                    DetailsUrl = GetOrderDetailsUrl(order)
                };

                string subject = "Order Confirmation";

                await SendOrderUpdateEmail(model, user.Email, subject);
            }
        }

        public async Task SendOrderShippedEmail(Order order, ApplicationUser user, decimal cost)
        {
            initTemplateKeys();
            if (templateKeysInitialized)
            {
                OrderUpdateModel model = new OrderUpdateModel
                {
                    UserName = user.UserName,
                    OrderId = order.RequestInfo.OrderRequestInfoGuid,
                    Cost = string.Format("${0:0.00}", cost),
                    PickUpAddress = order.RequestInfo.PickUpAddress.RawInputAddress,
                    Destination = order.RequestInfo.DropOffAddress.RawInputAddress,
                    UpdateType = OrderUpdateType.Shipping,
                    UpdateDescription = "has shipped!",
                    LogoId = Guid.Empty.ToString(),
                    DetailsUrl = GetOrderDetailsUrl(order)
                };

                string subject = "Your Order Has Shipped!";

                await SendOrderUpdateEmail(model, user.Email, subject);
            }
        }

        public async Task SendOrderDeliveredEmail(Order order, ApplicationUser user, decimal cost)
        {
            initTemplateKeys();
            if (templateKeysInitialized)
            {
                OrderUpdateModel model = new OrderUpdateModel
                {
                    UserName = user.UserName,
                    OrderId = order.RequestInfo.OrderRequestInfoGuid,
                    Cost = string.Format("${0:0.00}", cost),
                    PickUpAddress = order.RequestInfo.PickUpAddress.RawInputAddress,
                    Destination = order.RequestInfo.DropOffAddress.RawInputAddress,
                    UpdateType = OrderUpdateType.Delivery,
                    UpdateDescription = "has been delivered!",
                    LogoId = Guid.Empty.ToString(),
                    DetailsUrl = GetOrderDetailsUrl(order)
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

        public void SendWorkOrderEmail(ApplicationUser contractor, Order order)
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
                    DropOffAddress = order.RequestInfo.DropOffAddress.RawInputAddress,
                    PickUpAddress = order.RequestInfo.PickUpAddress.RawInputAddress,
                    LogoId = Guid.Empty.ToString(),
                    DetailsUrl = GetOrderDetailsUrl(order)
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

        private string GetOrderDetailsUrl(Order order)
        {
            return $"https://{url}{detailsPageUrl}{order.OrderGuid.ToString()}";
        }
    }

    public class SmsService : IIdentityMessageService
    {
        private readonly ITwilioMessageSender _messageSender;

        public SmsService() : this(new TwilioMessageSender()) { }

        public SmsService(ITwilioMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        public async Task SendAsync(IdentityMessage message)
        {
            await _messageSender.SendMessageAsync(message.Destination,
                                                  Config.TwilioNumber,
                                                  message.Body);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }


        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(
                new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses
            // Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(
            IdentityFactoryOptions<ApplicationSignInManager> options,
            IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(),
                                                context.Authentication);
        }
    }
}


