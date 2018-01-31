using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace ConsumerTests
{
    public class ApiClient
    {
        public string BaseUri;

        private HttpClient _client;

        public ApiClient(string baseUri = null)
        {
            BaseUri = baseUri ?? "http://localhost:1234";
        }

        /// <summary>
        /// Makes a request to the specfied endpoint and returns a HttpResponseMessage
        /// </summary>
        /// <param name="method"></param>
        /// <param name="endpoint"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public HttpResponseMessage MakeRequest(HttpMethod method, string endpoint, IReadOnlyList<string> headers = null)
        {
            using (_client = new HttpClient { BaseAddress = new Uri(BaseUri) })
            {
                var request = new HttpRequestMessage(method, BaseUri + endpoint);

                if (headers == null)
                {
                    request.Headers.Add("Accept", "application/json");
                }
                else
                {
                    request.Headers.Add(headers[0], headers[1]);
                }

                var response = _client.SendAsync(request);

                return response.Result;
            }
        }

        /// <summary>
        /// Converts a http response message to a string
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string HttpResponseToString(HttpResponseMessage message)
        {
            var reader = new StreamReader(message.Content.ReadAsStreamAsync().Result);
            var text = reader.ReadToEnd();

            message.Dispose();

            return text;
        }
    }
}