using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Cognitive.LUIS.ActionBinding;
using Newtonsoft.Json.Linq;

namespace LuisBot.Dialogs
{
    [Serializable]
    [LuisActionBinding("Enter OTP", FriendlyName = "Enter OTP")]
    public class EnterOTP : BaseLuisAction
    {

        public static string username;
        public static string phoneno;
        public static string otp;

        [Required(ErrorMessage = "Please Enter your OTP")]
        [LuisActionBindingParam(CustomType = "OTP", Order = 1)]
        //CustomType/BuiltinType = (Entity name in LUIS)
        public string OTP { get; set; }


        public override Task<object> FulfillAsync()
        {
            string result;
            
            //GetStatus getStatus = new GetStatus();   //instance created of GetStatus
            //Thread.Sleep(15000); // Wait for workflow execution
            //string response = getStatus.GetStatusAECall(ResetUserPassword.aeRequestId1); //Get Status Response - response from ae.
            //var rss = JObject.Parse(response);      // JObject.Parse string is converting in json format.
            //username = (string)rss["attribute1"];   // username is stored in attribute1 field of response.
            //phoneno = (string)rss["attribute2"];    // phoneno is stored in attribute2 field of response.
            //otp = (string)rss["attribute3"];        // otp is stored in attribute3 field of response.
            

            if (OTP.ToUpper().Equals(otp))
            {
                Dictionary<string, string> MyEntities = new Dictionary<string, string>();
                MyEntities.Add("User_Logon_Name", username);
                //MyEntities.Add("User_Logon_Name", "Geetanjali Khyale");
                MyEntities.Add("aewfid", "198");
                MyEntities.Add("MobileNumber", phoneno);
                //MyEntities.Add("MobileNumber", "919730975520");

                CreateJSON createJSON = new CreateJSON();

                string aeRequestId;
                aeRequestId = createJSON.AECall(MyEntities, "Reset Password");//Call Automation Edge.

                //return Task.FromResult((object)$"{ResetUserPassword.username} \n {ResetUserPassword.phoneno} \n {ResetUserPassword.otp}Your request for reset password has been initiated and password is sent on your registered mobile number.\n Was I helpful?");
                result = $"Username: {username} \n Phoneno: {phoneno} \n OTP: {otp} \n Your request for reset password has been initiated and password is sent on your registered mobile number.\n Was I helpful?";
            }
            else
            {
                result = $"OTP didn't match";

            }




            return Task.FromResult((object)result);

        }
        
    }


}
 