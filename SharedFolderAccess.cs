using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS.ActionBinding;
using Newtonsoft.Json.Linq;

namespace LuisBot.Dialogs
{
    [Serializable]         //(Intent Name)
    [LuisActionBinding("Shared Folder Access", FriendlyName = "Shared Folder Access Service Request")]
    public class SharedFolderAccess:BaseLuisAction
    {
        [Required(ErrorMessage = "Give me your user name please ")]    // user ask
        [LuisActionBindingParam(CustomType = "SamAccountName", Order = 1)]
        public string SamAccountName { get; set; }

        [Required(ErrorMessage = "Give me your folder name please")]    // user ask
        [LuisActionBindingParam(CustomType = "foldername", Order = 2)]
        public string foldername { get; set; }

        [Required(ErrorMessage = "please enter permission")]           // user ask
        [LuisActionBindingParam(CustomType = "permission", Order = 3)]
        public string permission { get; set; }

        string GroupName;
        string aeRequestId;

        [NonSerialized]
        Timer t;

        public override Task<object> FulfillAsync()
        {
           

            if (foldername.Equals("hr") && permission.Equals("read"))  //validation of foldername & permission
            {
                GroupName = "HR_Read";

            }
            else if (foldername.Equals("hr") && permission.Equals("modified"))
            {
                GroupName = "HR_Modified";

            }
            else if (foldername.Equals("corporate") && permission.Equals("Read"))
            {
                GroupName = "Corporate_Read";

            }
            else if (foldername.Equals("corporate") && permission.Equals("modified"))
            {
                GroupName = "Corporate_Modified";

            }
            else if (foldername.Equals("policy") && permission.Equals("read"))
            {
                GroupName = "Policy_Read";

            }
            else if (foldername.Equals("policy") && permission.Equals("modified"))
            {
                GroupName = "Policy_Modified";

            }
            else
            {
                GroupName = "Default";
            }


            Dictionary<string, string> MyEntities = new Dictionary<string, string>();

            MyEntities.Add("SamAccountName", this.SamAccountName); //Workflow parameter

            MyEntities.Add("GroupName", this.GroupName);         //Workflow parameter
                                        



            CreateJSON createJSON = new CreateJSON(); //instance created for createjson

           

            aeRequestId = createJSON.AECall(MyEntities, "SharedFolder"); // ae call

            //GetStatus getStatus = new GetStatus();  //instance created of GetStatus
           // Thread.Sleep(15000); // Wait for workflow execution
            //string response = getStatus.GetStatusAECall(aeRequestId); //Get Status Response(response from ae)

            //var rss = JObject.Parse(response);                        //Get Status Response(response from ae)
            //string AeRequestStatus = (string)rss["workflowResponse"];  //Get Status Response(response from ae)
            //rss = JObject.Parse(AeRequestStatus);                      //Get Status Response(response from ae)
            //string message = (string)rss["message"];                   //Get Status Response(response from ae)

            t = new Timer(new TimerCallback(timerEvent));
            t.Change(5000, Timeout.Infinite);


            return Task.FromResult((object)"message"); //Get Status Response.(response from ae)
        }

        public void timerEvent(object target)
        {

            t.Dispose();
            ConversationStarter.Resume(ConversationStarter.conversationId, ConversationStarter.channelId, aeRequestId); //We don't need to wait for this, just want to start the interruption here
        }
    }
}