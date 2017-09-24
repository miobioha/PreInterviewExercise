using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Vending.Core
{
    public class AccountServices : IAccountServices
    {
        private readonly string _endpointAddress;

        public AccountServices(string endpointAddress)
        {
            _endpointAddress = endpointAddress;
        }

        public bool Withdraw(Guid cardId, string pin, decimal amount)
        {
            var request = (HttpWebRequest)WebRequest.Create(_endpointAddress);
            request.Method = "PUT";

            var requestPayload = JsonConvert.SerializeObject(new
            {
                cardId = cardId,
                cardPin = pin,
                amount = amount
            });

            var encoding = new UTF8Encoding();
            request.ContentLength = encoding.GetByteCount(requestPayload);
            request.ContentType = "application/json";

            try
            {
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(encoding.GetBytes(requestPayload), 0, encoding.GetByteCount(requestPayload));
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }
    }
}
