using PracticalWerewolf.EmailTemplates;
using PracticalWerewolf.Models;
using PracticalWerewolf.Models.Orders;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PracticalWerewolf.Helpers
{
    public class EmailHelper
    {
        private static readonly EmailService _emailService = new EmailService();

        static EmailHelper()
        {
            var filePath = @"~\EmailTemplates\OrderUpdate.cshtml";
            var template = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(filePath));
            Engine.Razor.AddTemplate("OrderUpdate", template);
        }

        public static async Task SendOrderConfirmEmail(OrderRequestInfo order, ApplicationUser user)
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

        public static async Task SendOrderShippedEmail(OrderRequestInfo order, ApplicationUser user)
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

        public static async Task SendOrderDeliveryEmail(OrderRequestInfo order, ApplicationUser user)
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

        private static async Task SendOrderUpdateEmail(OrderUpdateModel model, string email, string subject, List<Tuple<string, string>> imageIds = null)
        {
            if(imageIds == null)
            {
                imageIds = new List<Tuple<string, string>>();
            }

            imageIds.Add(new Tuple<string, string>(System.Web.HttpContext.Current.Server.MapPath("~/favicon.ico"), Guid.Empty.ToString()));
            var result = Engine.Razor.RunCompile("OrderUpdate", typeof(OrderUpdateModel), model);

            int index = result.IndexOf("<!DOCTYPE html>");
            result = result.Substring(index);

            await _emailService.SendAsync(email, subject, result, imageIds);
        }
    }
}