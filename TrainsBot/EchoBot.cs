using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TrainsBot.Helpers;

namespace TrainsBot
{
    [Serializable]
    public class EchoBot : IDialog<object>
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
                        card = CardHelper.GetAdaptiveCard("specialassistance");
                        break;
                    case Constants.NormalPassenger:
                        card = CardHelper.GetAdaptiveCard("normalpassenger");
                        break;
                    case Constants.AvailableTrains:
                        card = CardHelper.GetAvailableTrainsCard();
                        break;
                    case Constants.Refreshment:
                        card = CardHelper.GetAvailableRefreshments();
                        break;

                    case Constants.TrainDetails:

                        card = CardHelper.GetAdaptiveCard("traindetails");
                        break;
                    case Constants.GetRefreshmentItem:

                        card = CardHelper.GetAdaptiveCard("refreshments");
                        break;
                    case Constants.GetTrainStatusCard:

                        card = CardHelper.GetAdaptiveCard("trainstatus");
                        break;
                    case Constants.GetCustomerfeedbackcard:

                        card = CardHelper.GetAdaptiveCard("customerfeedback");
                        break;
                    case Constants.GetMenuCard:

                        card = CardHelper.GetAdaptiveCard("MenuCard");
                        break;
                    case Constants.GetOrderDetails:

                        card = CardHelper.GetAdaptiveCard("Order");
                        break;
                    default:
                        card = CardHelper.GetWelcomeScreen();
                        break;
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
            if (actionDetails.channelId != null)
            {

                var type = actionDetails.ActionType;
                var data = actionDetails.choices;

                Attachment card = null;

                switch (data)
                {
                    case Constants.SpecialAssistance:
                        card = CardHelper.GetAdaptiveCard("specialassistance");
                        break;
                    case Constants.NormalPassenger:
                        card = CardHelper.GetAdaptiveCard("normalpassenger");
                        break;
                    case Constants.AvailableTrains:
                        card = CardHelper.GetAvailableTrainsCard();
                        break;
                    case Constants.Refreshment:
                        card = CardHelper.GetAvailableRefreshments();
                        break;

                    case Constants.TrainDetails:
                        card = CardHelper.GetAdaptiveCard("traindetails");
                        break;
                    case Constants.GetRefreshmentItem:

                        card = CardHelper.GetAdaptiveCard("refreshments");
                        break;
                    case Constants.GetTrainStatusCard:

                        card = CardHelper.GetAdaptiveCard("trainstatus");
                        break;
                    case Constants.GetCustomerfeedbackcard:

                        card = CardHelper.GetAdaptiveCard("customerfeedback");
                        break;
                    case Constants.GetMenuCard:

                        card = CardHelper.GetAdaptiveCard("MenuCard");
                        break;
                    case Constants.GetOrderDetails:

                        card = CardHelper.GetAdaptiveCard("Order");
                        break;

                }
                //call send notification and pass param
                await SendNotification(activity, actionDetails.channelId, card);


            }
            return;
        }

        public static async Task<bool> SendNotification(Activity activity, string channelId, Attachment attachment)
        {
            // MicrosoftAppCredentials.TrustServiceUrl(activity.ServiceUrl);
            using (var connectorClient = new ConnectorClient(new Uri("https://smba.trafficmanager.net/amer/")))
            {
                var message = Activity.CreateMessageActivity();
                message.Attachments.Add(attachment);
                var parameters = new ConversationParameters
                {
                    Bot = activity.Recipient,
                    IsGroup = true,
                    ChannelData = new TeamsChannelData
                    {
                        Channel = new ChannelInfo(channelId),
                        Notification = new NotificationInfo() { Alert = true }
                    },
                    Activity = (Activity)message
                };
                // MicrosoftAppCredentials.TrustServiceUrl("https://canary.botapi.skype.com/amer/", DateTime.MaxValue);

                try
                {
                    var response = await connectorClient.Conversations.CreateConversationAsync(parameters);
                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                }
                // await connectorClient.Conversations.SendToConversationAsync((Activity)message);
                return true;
            }
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
        public string channelId { get; set; }
        public string choices { get; set; }

    }

}