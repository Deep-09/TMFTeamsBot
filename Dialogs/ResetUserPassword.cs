using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS.ActionBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LuisBot.Dialogs
{
   

    [Serializable]
    [LuisActionBinding("Reset User Password", FriendlyName = "Reset User Password Service Request")]
    public class ResetUserPassword : BaseLuisAction
    {
        static string aeRequestIdpwd;

        public static string aeRequestId1; //variable creation for ae call
        [Required(ErrorMessage = "Please give me your user name")]
        [LuisActionBindingParam(CustomType = "adusername", Order = 1)]
        //CustomType/BuiltinType = (Entity name in LUIS)
        public string adusername { get; set; }

        [Required(ErrorMessage = "Please enter your phone number below")]
        [LuisActionBindingParam(BuiltinType = BuiltInTypes.OTP, Order = 2)]
        public string phonenumber { get; set; }

      

        [NonSerialized]
        Timer t;

        public override Task<object>FulfillAsync()
        {
            Dictionary<string, string> MyEntities = new Dictionary<string, string>();

            MyEntities.Add("username", this.adusername);   //Workflow parameter
            MyEntities.Add("mobile", this.phonenumber);    //Workflow parameter

            CreateJSON createJSON = new CreateJSON(); //instance created for createjson



            aeRequestIdpwd = createJSON.AECall(MyEntities, "Generate OTP");  // ae call


           
            t = new Timer(new TimerCallback(timerEvent));
            t.Change(40000, Timeout.Infinite);

            return Task.FromResult((object)$" OTP is sent on your registered mobile number,I will prompt you to enter OTP");

            //return Task.FromResult((object)$"I will reset password for  {this.adusername} as soon as possible... Visit me again whenever you need my help. Have a great day :)");

            //return Task.FromResult((object)$" OTP is sent on your registered mobile number, please enter otp. Your reference key is - {aeRequestId1} (RK). You need to enter reference key<space>OTP eg. (RK) AZ6754");
            //Get Status Response from ae

        }
        public void timerEvent(object target)
        {

            t.Dispose();
            StartConversation.Resume(StartConversation.conversationId, StartConversation.channelId, aeRequestIdpwd); //We don't need to wait for this, just want to start the interruption here

        }

    }
}