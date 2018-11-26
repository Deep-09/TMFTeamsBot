using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS.ActionBinding;

namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("Create AD User", FriendlyName = "Create AD User Service Request")]
    public class CreateADuser : BaseLuisAction
    {
        [Required(ErrorMessage = "Give me your First name please ")]
        [LuisActionBindingParam(CustomType = "firstname", Order = 1)]
        public string firstname { get; set; }

        [Required(ErrorMessage = "Give me your Last name please")]
        [LuisActionBindingParam(CustomType = "lastname", Order = 2)]
        public string lastname { get; set; }

        [Required(ErrorMessage = "Please enter your phone number below")]
        [LuisActionBindingParam(BuiltinType = BuiltInTypes.OTP, Order = 3)]
        public string phonenumber { get; set; }

        [Required(ErrorMessage = "Please enter your email address below")]
        [LuisActionBindingParam(CustomType = "emailaddress", Order = 4)]
        public string emailaddress { get; set; }

        [Required(ErrorMessage = "Please enter your ad username below")]
        [LuisActionBindingParam(CustomType = "adusername", Order = 5)]
        public string adusername { get; set; }

        [Required(ErrorMessage = "Please enter your ad password below")]
        [LuisActionBindingParam(CustomType = "adpassword", Order = 6)]
        public string adpassword { get; set; }




        //[Required(ErrorMessage = "Give template a name of your choice")]
        //[LuisActionBindingParam(CustomType = "Template_Name", Order = 7)]
        //public string Template_Name { get; set; }

        //[Required(ErrorMessage = "And give this VM a name of your choice")]
        //[LuisActionBindingParam(CustomType = "VM_Name", Order = 8)]
        //public string VM_Name { get; set; }
        string aeRequestid=""; //


        [NonSerialized]
        Timer t;

        public override Task<object> FulfillAsync()
        {

            Dictionary<string, string> MyEntities = new Dictionary<string, string>();

            MyEntities.Add("fname", this.firstname);
            MyEntities.Add("lname", this.lastname);
            MyEntities.Add("phno", this.phonenumber);
            MyEntities.Add("email", this.emailaddress);
            MyEntities.Add("uname", this.adusername);
            MyEntities.Add("pwd", this.adpassword);
            MyEntities.Add("q", "test");
            MyEntities.Add("category", "/api/ticket_category/68");
           

            CreateJSON createJSON = new CreateJSON();



            //createJSON.AECall(MyEntities, "Create AD User");

            t = new Timer(new TimerCallback(timerEvent));
            t.Change(5000, Timeout.Infinite);

            return Task.FromResult((object)$"I will add AD user named {this.adusername} soon... Visit me again whenever you need my help... Have a great day :)");
        }

        public void timerEvent(object target)
        {

            t.Dispose();
            ConversationStarter.Resume(ConversationStarter.conversationId, ConversationStarter.channelId, aeRequestid); //We don't need to wait for this, just want to start the interruption here
        }

    }
}