using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Microsoft.Bot.Sample.LuisBot;
using Newtonsoft.Json;
using RestSharp;

namespace LuisBot.Dialogs
{
    public class CreateJSON
    {
        public string AECall(Dictionary<string, string> MyEntities,string intentsr)
        {
            var client = new RestClient("https://43d5c2e2.ngrok.io/aeengine/rest/authenticate"); //ae authentication
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "ea502694-bf8a-9c2e-e27b-8082381ce137");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"username\"\r\n\r\nVyomlabs\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"password\"\r\n\r\nAdmin@123\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request); // post call execute
            string jsonresult;
            jsonresult = response.Content;
            var myDetails = JsonConvert.DeserializeObject<MyDetail>(jsonresult);
            string token = myDetails.sessionToken; //authenticate result (session token)
            var request1 = new RestRequest("https://43d5c2e2.ngrok.io/aeengine/rest/execute", Method.POST); //workflow execution
            request1.AddHeader("X-session-token", token);

            JavaScriptSerializer serialiser = new JavaScriptSerializer();
            List<AutomationParameter> ListAutomationField = new List<AutomationParameter>();

            //List<JsonParam> InnerJsonParam = new List<JsonParam>();

            //JsonParam[] jparameter = new JsonParam[MyEntities.Count];
            //for (int i = 0; i < MyEntities.Count; i++)
            //{
            //    jparameter[i] = new JsonParam();
            //    jparameter[i].question = MyEntities.ElementAt(i).Key;
            //    jparameter[i].answer = MyEntities.ElementAt(i).Value;
            //    InnerJsonParam.Add(jparameter[i]);
            //}


            AutomationParameter[] jparameter = new AutomationParameter[MyEntities.Count];
            for (int i = 0; i < MyEntities.Count; i++)
            {
                jparameter[i] = new AutomationParameter();
                jparameter[i].name = MyEntities.ElementAt(i).Key;
                jparameter[i].value = MyEntities.ElementAt(i).Value;
                ListAutomationField.Add(jparameter[i]);
            }

            //InnerJson innerjsonobject = new InnerJson();
            //innerjsonobject.ServiceRequest = intentsr;
            //innerjsonobject.@params = InnerJsonParam;

            //string json1 = serialiser.Serialize(innerjsonobject);

            //AutomationParameter parameter1 = new AutomationParameter();
            //parameter1.name = "jsonInput";
            //parameter1.value = json1;
            //parameter1.type = "String";
            //parameter1.order = 1;
            //parameter1.secret = false;
            //parameter1.optional = false;
            //parameter1.displayName = "jsonInput";
            //parameter1.extension = null;
            //parameter1.poolCredential = false;

            //ListAutomationField.Add(parameter1);

            //AutomationParameter parameter2 = new AutomationParameter();
            //parameter2.name = "clientEmail";
            //parameter2.value = "satyendar.daragani@3i-infotech.com";
            //parameter2.type = "String";
            //parameter2.order = 2;
            //parameter2.secret = false;
            //parameter2.optional = false;
            //parameter2.displayName = "snapshotname";
            //parameter2.extension = null;
            //parameter2.poolCredential = false;

            //ListAutomationField.Add(parameter2);

            Guid temp = Guid.NewGuid();

            RootAutomation AutoRoot = new RootAutomation();    //json
            AutoRoot.orgCode = "TENANT1";
            AutoRoot.workflowName = intentsr;
            AutoRoot.userId = "Geetanjali Khyale";
            AutoRoot.@params = ListAutomationField;
            AutoRoot.sourceId = temp.ToString();
            AutoRoot.source = "AutomationEdge HelpDesk";
            AutoRoot.responseMailSubject = null;
            string json = serialiser.Serialize(AutoRoot);
            //await context.PostAsync($"{json}");

            request1.AddHeader("content-type", "application/json");
            request1.AddParameter("application/json", json, ParameterType.RequestBody);
            request1.RequestFormat = DataFormat.Json;
            IRestResponse response1 = client.Execute(request1);
            string jsonresult1;
            jsonresult1 = response1.Content;
            var getAEId = JsonConvert.DeserializeObject<GetAEId>(jsonresult1);
            string aeRequestId = getAEId.automationRequestId;
            return aeRequestId;
        }
    }
}


