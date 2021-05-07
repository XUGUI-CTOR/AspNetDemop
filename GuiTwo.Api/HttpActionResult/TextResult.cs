using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GuiTwo.Api.HttpActionResult
{
    public class TextResult : IHttpActionResult
    {
        string _value;
        HttpRequestMessage _request;
        public TextResult(string value, HttpRequestMessage request)
        {
            _value = value;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(_value, encoding: Encoding.Unicode),
                RequestMessage = _request
            };
            return Task.FromResult(response);
        }
    }
}