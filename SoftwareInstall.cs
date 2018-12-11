using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Cognitive.LUIS.ActionBinding;
using RestSharp;

namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("Software Install", FriendlyName = "SoftwareInstall RFN-V2")]
    public class SoftwareInstall : BaseLuisAction
    {
        static string aeRequestIdSI;

        [Required(ErrorMessage = "Sure, I'll help you to install software. Please  help me with your system’s IP address.")]
        [LuisActionBindingParam(CustomType = "hostIP", Order = 1)]
        //CustomType/BuiltinType = (Entity name in LUIS)
        public string hostIP { get; set; }
        
        //[Required(ErrorMessage = "Thanks! I can help you with these software installation. Please select one of the software below.\"< br ><input type = 'button' class='button' id='notepad++' value='Notepad++' onclick=\"button_send('Notepad++');\"/><input type = 'button' class='button' id='adobe_reader' value='Adobe Reader' onclick=\"button_send('Adobe Reader');\"/><input type = 'button' class='button' id='nodejs' value='Node.js' onclick=\"button_send('Node.js');\"/>")]
        [Required(ErrorMessage = "Thanks! I can help you with these software installation. Please select one of the software below.\n 1. Notepad++ \n2. Adobe Reader \n3. nodejs")]
        [LuisActionBindingParam(CustomType = "software", Order = 1)]
        //CustomType/BuiltinType = (Entity name in LUIS)
        public string software { get; set; }

        

        [NonSerialized]
        Timer t;

        public override Task<object> FulfillAsync()
        {
         
            Dictionary<string, string> MyEntities = new Dictionary<string, string>();


            
            MyEntities.Add("socket_id", "/customer#VgP_SYSO6l2wE36uAAAC");
            MyEntities.Add("hostIP", this.hostIP);
            MyEntities.Add("SoftwareName", this.software);
            //MyEntities.Add("SoftwareName", "nppp.msi");

            CreateJSON createJSON = new CreateJSON();

            aeRequestIdSI = createJSON.AECall(MyEntities, "SoftwareInstall RFN-V2");
            //GetStatus getStatus = new GetStatus();
            //Thread.Sleep(15000);
            //string response = getStatus.GetStatusAECall(aeRequestId);

            t = new Timer(new TimerCallback(timerEvent));
            t.Change(50000, Timeout.Infinite);

            

            return Task.FromResult((object)$"Please wait while we work on your request.");
            //return Task.FromResult((object)$"Please wait while we work on your request. It typically takes 2 minutes to complete the operation, click after 2 minutes.< input type = 'button' class='button' id='check_status' value='Click here to check status' onclick=\"button_send('Check Status');\"/>");

        }

        public void timerEvent(object target)
        {

            t.Dispose();
            ConversationStarter.Resume(ConversationStarter.conversationId, ConversationStarter.channelId, aeRequestIdSI); //We don't need to wait for this, just want to start the interruption here
        }
    }

}

