﻿using ClosedXML.Excel;
using EShopAdminApplication.Models;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EShopAdminApplication.Controllers
{
    public class OrderController : Controller
    {
        public OrderController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44338/Api/Admin/GetAllActiveOrders";
            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Order>>().Result;
            return View(data);
        }
        public IActionResult Details(string orderId)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44338/Api/Admin/GetDetailsForOrders";
            var model = new
            {
                Id = orderId
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(URL, content).Result;
            var data = response.Content.ReadAsAsync<Order>().Result;
            return View(data);
        }
        [HttpGet]
        public IActionResult ExportAllOrders()
        {
            string fileName = ".Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            using(var workbook=new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");

                HttpClient client = new HttpClient();

                string URL = "https://localhost:44338/Api/Admin/GetAllActiveOrders";

                HttpResponseMessage response = client.GetAsync(URL).Result;

                var result = response.Content.ReadAsAsync<List<Order>>().Result;

                worksheet.Cell(1, 1).Value = "Order Id";
                worksheet.Cell(1, 2).Value = "Costumer Name";
                worksheet.Cell(1, 3).Value = "Costumer Email";
                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.User.UserName;
                    worksheet.Cell(i + 1, 3).Value = item.User.Email;

                    for (int p = 1; p <= item.TicketInOrders.Count(); p++)
                    {
                        worksheet.Cell(1, p + 3).Value = "Ticket-" + (p);
                        worksheet.Cell(i + 1, p + 3).Value = item.TicketInOrders.ElementAt(p - 1).OrderTicket.MovieName;
                    }

                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }
            }

            
        }
        public FileContentResult CreateInvoice(string orderId)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44338/Api/Admin/GetDetailsForOrders";
            var model = new
            {
                Id = orderId
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(URL, content).Result;
            var data = response.Content.ReadAsAsync<Order>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);
            document.Content.Replace("{{OrderNumber}}", data.Id.ToString());
            document.Content.Replace("{{UserName}}", data.User.UserName);
            StringBuilder sb = new StringBuilder();
            var totalPrice = 0.0;
            foreach(var item in data.TicketInOrders)
            {
                totalPrice += item.Quantity * item.OrderTicket.Price;
                sb.AppendLine(item.OrderTicket.MovieName + " with quantity of: " + item.Quantity + " and price of: $" + item.OrderTicket.Price);
            }
            document.Content.Replace("{{ProductList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString());

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(),new PdfSaveOptions().ContentType,"Export.pdf");
        }
    }
}
