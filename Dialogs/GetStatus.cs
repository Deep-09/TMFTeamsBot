using Microsoft.Bot.Sample.LuisBot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Script.Serialization;

namespace LuisBot.Dialogs
{
    public class GetStatus
    {
        public string GetStatusAECall(string aeid)
        {
            var client = new RestClient("http://10.51.28.19:8080/aeengine/rest/authenticate");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "ea502694-bf8a-9c2e-e27b-8082381ce137");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"username\"\r\n\r\nVyomlabs\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"password\"\r\n\r\nAdmin@123\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string jsonresult;
            jsonresult = response.Content;
            var myDetails = JsonConvert.DeserializeObject<MyDetail>(jsonresult);
            string token = myDetails.sessionToken;
            
            
            
            //var request1 = new RestRequest("http://10.51.28.19:8080/aeengine/rest/workflowinstances?offset=0&size=1", Method.POST);
            //request1.AddHeader("X-session-token", token);
            //Console.WriteLine("token is"+token);
            //JavaScriptSerializer serialiser = new JavaScriptSerializer();
            //List<GetMessageParameters> ListAutomationField = new List<GetMessageParameters>();
            //FilterList FilterField = new FilterList();



            HttpClient Adclient = new HttpClient();
            Adclient.BaseAddress = new Uri("http://10.51.28.19:8080/");
            Adclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); ;
            Adclient.DefaultRequestHeaders.Add("X-session-token", token);

            var New_Response = Adclient.GetAsync("aeengine/rest/workflowinstances/" + aeid).Result;
            string resultContent = New_Response.Content.ReadAsStringAsync().Result;
            //Console.WriteLine(resultContent);
            //var rss = JObject.Parse(resultContent);
            //string AeRequestStatus = (string)rss["workflowResponse"];
            //rss = JObject.Parse(AeRequestStatus);
            //string message = (string)rss["message"];






            //GetMessageParameters parameter = new GetMessageParameters();
            //parameter.columnName = "id";
            //parameter.displayName = "ID";
            //parameter.columnType = "number";
            //parameter.visibility = true;
            //parameter.comparator = "eq";
            //parameter.values[0] = aeid; //response  aeid
            //parameter.values[1] = null; ;

            //ListAutomationField.Add(parameter);

            //FilterField.@filter = ListAutomationField;

            //string json = serialiser.Serialize(FilterField);
            ////await context.PostAsync($"{json}");

            //request1.AddHeader("content-type", "application/json");
            //request1.AddParameter("application/json", json, ParameterType.RequestBody);
            //request1.RequestFormat = DataFormat.Json;
            ////IRestResponse response1 = client.Execute(request1);
            //string jsonresult1;
            //jsonresult1 = response1.Content;
            //return jsonresult1;
            return resultContent;


        }
    }
}