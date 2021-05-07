using GuiTwo.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace GuiTwo.Api.Controllers
{
    public class ValuesController : ApiController
    {
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };
        public void Post()
        {

        }

        public HttpResponseMessage Get()
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK,products);
            //response.Content = new StringContent("Hello World!", Encoding.Unicode);
            //response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
            //{
            //    MaxAge = TimeSpan.FromSeconds(12)
            //};
            return response;
        }
    }
}
