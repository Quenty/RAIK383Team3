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
            List<Tuple<string, string>> templateKeys = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("OrderUpdate", @"~\EmailTemplates\OrderUpdate.cshtml"),
                new Tuple<string, string>("WorkOrder", @"~\EmailTemplates\WorkOrder.cshtml"),
            };
            foreach(var keys in templateKeys)
            {
                var filePath = keys.Item2;
                var template = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(filePath));
                Engine.Razor.AddTemplate(keys.Item1, template);

            }
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

        public static async Task SendWorkOrderEmail(ApplicationUser contractor, OrderRequestInfo order)
        {
            WorkOrderModel model = new WorkOrderModel
            {
                ContractorUserName = contractor.UserName,
                DropOffAddress = order.DropOffAddress.RawInputAddress,
                PickUpAddress = order.PickUpAddress.RawInputAddress,
                LogoId = Guid.Empty.ToString()
            };
            string subject = "Work Order Request";

            await SendWorkOrderEmail(model, contractor.Email, subject);
        }

        private static async Task SendWorkOrderEmail(WorkOrderModel model, string email, string subject, List<Tuple<string, string>> imageIds = null)
        {
            if (imageIds == null)
            {
                imageIds = new List<Tuple<string, string>>();
            }

            imageIds.Add(new Tuple<string, string>(System.Web.HttpContext.Current.Server.MapPath("~/favicon.ico"), Guid.Empty.ToString()));
            var result = Engine.Razor.RunCompile("WorkOrder", typeof(WorkOrderModel), model);

            int index = result.IndexOf("<!DOCTYPE html>");
            result = result.Substring(index);

            await _emailService.SendAsync(email, subject, result, imageIds);
        }
    }
}