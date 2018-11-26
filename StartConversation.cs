using System;
using Microsoft.Bot.Connector;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using LuisBot.Dialogs;

namespace LuisBot
{
    public class StartConversation
    { 

        //Note: Of course you don't want these here. Eventually you will need to save these in some table
        //Having them here as static variables means we can only remember one user :)
        public static string fromId;
        public static string fromName;
        public static string toId;
        public static string toName;
        public static string serviceUrl;
        public static string channelId;
        public static string conversationId;

        //This will send an adhoc message to the user
        public static async Task Resume(string conversationId, string channelId, string aeRequestId1)
        {
            GetStatus getStatus = new GetStatus();

            string response = getStatus.GetStatusAECall(aeRequestId1); //Get Status Response(response from ae) // Error or message get from ae aeRequestId

            var rss = JObject.Parse(response);                        //Get Status Response(response from ae)
            string AeRequestStatus = (string)rss["workflowResponse"];  //Get Status Response(response from ae)
            rss = JObject.Parse(AeRequestStatus); //Get Status Response(response from ae)
            var op = rss["outputParameters"];
            EnterOTP.username = (string)rss["username"];   // username is stored in attribute1 field of response.
            EnterOTP.phoneno = (string)rss["mobile"];    // phoneno is stored in attribute2 field of response.
            EnterOTP.otp = (string)rss["custNxt"];

            //string aemessage = (string)rss["result"]; //Get Status Response(response from ae)


            var userAccount = new ChannelAccount(toId, toName);
            var botAccount = new ChannelAccount(fromId, fromName);
            var connector = new ConnectorClient(new Uri(serviceUrl));

            IMessageActivity message = Activity.CreateMessageActivity();
            if (!string.IsNullOrEmpty(conversationId) && !string.IsNullOrEmpty(channelId))
            {
                message.ChannelId = channelId;
            }
            else
            {
                conversationId = (await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount)).Id;
            }
            message.From = botAccount;
            message.Recipient = userAccount;
            message.Conversation = new ConversationAccount(id: conversationId);
            message.Text = $"{rss}";

            message.Locale = "en-Us";
            await connector.Conversations.SendToConversationAsync((Activity)message);
        }
    }
}