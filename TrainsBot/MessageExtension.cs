using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;

using System.Linq;

namespace TrainsBot
{
    public class MessageExtension
    {
        public static ComposeExtensionResponse HandleMessageExtensionQuery(ConnectorClient connector, Activity activity)
        {
            var query = activity.GetComposeExtensionQueryData();
            if (query == null || (query.CommandId != "getStudents" && query.CommandId != "getTeachers"))
            {
                // We only process the 'getRandomText' queries with this message extension
                return null;
            }

            var title = "";
            var titleParam = query.Parameters?.FirstOrDefault(p => p.Name == "cardTitle");
            if (titleParam != null)
            {
                title = titleParam.Value.ToString();
            }

            var attachments = new ComposeExtensionAttachment[5];
            var role = query.CommandId == "getStudents" ? "Student" : "Teacher";
            for (int i = 0; i < 5; i++)
            {
                attachments[i] = GetAttachment(title, role);
            }

            var response = new ComposeExtensionResponse(new ComposeExtensionResult("list", "result"));
            response.ComposeExtension.Attachments = attachments.ToList();

            return response;
        }

        private static ComposeExtensionAttachment GetAttachment(string title = null, string type = null)
        {
            var person1 = new Bogus.Person();
            var card = new ThumbnailCard
            {
                Title = !string.IsNullOrWhiteSpace(title) ? title : person1.FullName,
                Text = "Role: " + type,
                Subtitle = person1.Address.City,
                Images = { new CardImage(person1.Avatar) }
            };

            return card
                .ToAttachment()
                .ToComposeExtensionAttachment();
        }
    }
}