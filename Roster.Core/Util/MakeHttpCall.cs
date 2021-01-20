﻿//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Roster.Core.ApiModels;
using Roster.Core.Models;

namespace Roster.Core.Util
{
    public static class MakeHttpCall
    {
        /// <summary>
        /// This is an helper method to help deserialize response from color api call.
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async Task<object> GetJsonResult(HttpClient httpClient, string uri)
        {
            var httpResponse = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);

            httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299

            if (httpResponse.Content is object && httpResponse.Content.Headers.ContentType.MediaType == "text/html")
            {
                var contentStream = await httpResponse.Content.ReadAsStreamAsync();

                using var streamReader = new StreamReader(contentStream);
                using var jsonReader = new JsonTextReader(streamReader);

                JsonSerializer serializer = new JsonSerializer();

                try
                {
                    var res = serializer.Deserialize<ColorModel>(jsonReader);
                    return res;
                }
                catch (JsonReaderException)
                {
                    Console.WriteLine("Invalid JSON.");
                }
            }
            else
            {
                Console.WriteLine("HTTP Response was invalid and cannot be deserialised.");
            }

            return null;
        }
    }
}
