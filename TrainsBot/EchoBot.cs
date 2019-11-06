using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TrainsBot.Helpers;

namespace TrainsBot
{
    [Serializable]
    public class EchoBot:IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            string message = string.Empty;
            if (!string.IsNullOrEmpty(activity.Text))
            {
                message = Microsoft.Bot.Connector.Teams.ActivityExtensions.GetTextWithoutMentions(activity).ToLowerInvariant();
                Attachment card = null;
                switch (message.Trim())
                {
                    case Constants.SpecialAssistance:
                        card = CardHelper.GetAdaptiveCard("1");
                        break;
                    case Constants.NormalPassenger:
                        card = CardHelper.GetAdaptiveCard("2");
                        break;
                    case Constants.AvailableTrains:
                        card = CardHelper.GetAvailableTrainsCard();
                        break;
                    case Constants.Refreshment:
                        card = CardHelper.GetAvailableRefreshments();
                        break;
                   
                    case Constants.TrainDetails:
                        
                        card = CardHelper.GetAdaptiveCard("3");
                        break;
                    case Constants.GetRefreshmentItem:
                        
                        card = CardHelper.GetAdaptiveCard("4");
                        break;

                        //default:
                        //    card = CardHelper.GetWelcomeScreen(userDetails.GivenName ?? userDetails.Name);
                        //    break;
                }

                var reply = context.MakeMessage();
                reply.Attachments.Add(card);
                await context.PostAsync(reply);

            }
            else if (activity.Value != null)
            {
                await HandleActions(context, activity);
                return;
            }
        }
        private async Task HandleActions(IDialogContext context, Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            var actionDetails = JsonConvert.DeserializeObject<ActionDetails>(activity.Value.ToString());
           
            var type = actionDetails.ActionType;

            Attachment card = null;

            switch (type)
            {
                case Constants.TrainDetails:
                    var detailsreply = Activity.CreateMessageActivity();
                    card = CardHelper.GetAdaptiveCard("2");
                    detailsreply.Attachments.Add(card);
                    await connector.Conversations.UpdateActivityAsync(activity.Conversation.Id, activity.ReplyToId, (Activity)detailsreply);
                    break;
                //case Constants.NextWeekRoster:
                //    card = await CardHelper.GetWeeklyRosterCard(userDetails.UserPrincipalName);
                //    break;
                
            }

            var reply = context.MakeMessage();
            reply.Attachments.Add(card);
            await context.PostAsync(reply);
            return;
        }
        public static async Task EchoMessage(ConnectorClient connector, Activity activity)
        {
            var reply = activity.CreateReply("You said: " + activity.GetTextWithoutMentions());
            await connector.Conversations.ReplyToActivityWithRetriesAsync(reply);
        }
    }
    public class ActionDetails
    {
        public string ActionType { get; set; }
    }
}