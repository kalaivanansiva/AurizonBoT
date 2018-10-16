using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Web.Http.Description;
using System.Net.Http;
using System.Diagnostics;
using SimpleEchoBot;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.DataContracts;

namespace Microsoft.Bot.Sample.LuisBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {            
            Debug.Write("App Start");
            var att=activity.Attachments;
            Activity act=activity;
            // check if activity is of type message
            if (activity.GetActivityType() == ActivityTypes.Message && att.Count==0)
            {
                await Conversation.SendAsync(activity, () => new BasicLuisDialog());
            }            
            else if(att.Count>0)
            {
                 var reply = activity.CreateReply();
               var client = new ConnectorClient(new Uri(act.ServiceUrl), new MicrosoftAppCredentials());
               reply.Text = $"Attachment received";
              await client.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                await HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private async Task<Activity> HandleSystemMessage(Activity message)
        {
            WebApiApplication.Telemetry.TrackEvent(@"SystemMessage", new Dictionary<string, string> { { @"Type", message.Type } });
            
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                IConversationUpdateActivity update = message;
                var client = new ConnectorClient(new Uri(message.ServiceUrl), new MicrosoftAppCredentials());
                if (update.MembersAdded != null && update.MembersAdded.Count>0)
                {
                    foreach (var newMember in update.MembersAdded)
                    {
                        if (newMember.Id != message.Recipient.Id)
                        {
                            var reply = message.CreateReply();
                            reply.Text = $"Welcome {newMember.Name}!";
                            await client.Conversations.ReplyToActivityAsync(reply);
                        }
                    }
                }         
               var telemetry = new EventTelemetry("Welcome event");
               
               telemetry.Properties.Add("Message", "Test events");               
               
               WebApiApplication.Telemetry.TrackEvent(telemetry);
               
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
                                
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;        
        }
    }
}
