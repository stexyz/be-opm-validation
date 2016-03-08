using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using opm_validation_service.Models;

namespace EndToEndTests.HttpClient {
    public class OpmVerificationHttpClient : IOpmVerificationHttpClient
    {
        private readonly Uri baseUri;

        public OpmVerificationHttpClient(string endpoint)
        {
            baseUri = new Uri(endpoint);
        }

        public OpmVerificationResult GetWithCookie(string id, String token)
        {
            string resultString = HttpGet("api/OpmDuplicity/", token);
            return JsonConvert.DeserializeObject<OpmVerificationResult>(resultString);
        }

        public OpmVerificationResult GetWithInlineToken(string id, string token)
        {
            //TODO SP: add the credentials here...
            string resultString = HttpGet("api/OpmDuplicity/" + id + "?token=" + token);
            return JsonConvert.DeserializeObject<OpmVerificationResult>(resultString);
        }

        public OpmVerificationResult GetAd(string id)
        {
            string resultString = HttpGet("api/OpmDuplicityCredentials/" + id);
            return JsonConvert.DeserializeObject<OpmVerificationResult>(resultString);
        }

        private string HttpGet(string restOfUri, string token = "") {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUri + restOfUri);
            request.Method = "GET";
            request.Accept = "application/json;charset=utf-8";

            request.CookieContainer = new CookieContainer();
            if (token != "") {
                Cookie c = new Cookie("iPlanetDirectoryPro", token) { Domain = baseUri.Host };
                request.CookieContainer.Add(c);
            }

            Stream responseStream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            return reader.ReadToEnd();
        }
    }
}
