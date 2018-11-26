using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Cognitive.LUIS.ActionBinding;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("Internet is not working", FriendlyName = "Internet is not working Request")]
    public class Internetnotworking : BaseLuisAction
    {
        [Required(ErrorMessage = "Can you please help me with Host Name ?")]
        [LuisActionBindingParam(CustomType = "hostname", Order = 1)]
        //CustomType/BuiltinType = (Entity name in LUIS)
        public string hostname { get; set; }


        string aeRequestid = "";

        [NonSerialized]
        Timer t;

        public override Task<object> FulfillAsync()
        {
            Dictionary<string, string> MyEntities = new Dictionary<string, string>();

            MyEntities.Add("hostname", this.hostname);

            CreateJSON createJSON = new CreateJSON();


            string aeRequestId;
            aeRequestId = createJSON.AECall(MyEntities, "ProxyUsecase");
            //GetStatus getStatus = new GetStatus();
            //Thread.Sleep(15000);
            //string response = getStatus.GetStatusAECall(aeRequestId);
            //var rss = JObject.Parse(response);
            //string AeRequestStatus = (string)rss["workflowResponse"];
            // rss = JObject.Parse(AeRequestStatus);
            //string message = (string)rss["message"];

            t = new Timer(new TimerCallback(timerEvent));
            t.Change(5000, Timeout.Infinite);


            //return Task.FromResult((object)$"Internet is not working for  {this.hostname} as soon as possible... Visit me again whenever you need my help. Have a great day :) {response}");
            return Task.FromResult((object)$"{"message"}Please wait while we work on your request. It typically takes 2 minutes to complete the operation, click after 2 minutes.< input type = 'button' class='button' id='check_status' value='Click here to check status' onclick=\"button_send('Check Status');\"/>");

        }
        public void timerEvent(object target)
        {

            t.Dispose();
            ConversationStarter.Resume(ConversationStarter.conversationId, ConversationStarter.channelId, aeRequestid); //We don't need to wait for this, just want to start the interruption here
        }

    }
}