using AdaptiveCards;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using TrainsBot.Models;

namespace TrainsBot.Helpers
{
    public class CardHelper
    {
        public static Attachment GetAdaptiveCard(string filenumber)
        {
            // Parse the JSON 
            AdaptiveCardParseResult result = AdaptiveCard.FromJson(GetAdaptiveCardJson(filenumber));

            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = result.Card

            };
        }
        public static Attachment GetWelcomeScreen()
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion("1.0"))
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveContainer()
                    {
                        Items=new List<AdaptiveElement>()
                        {
                            new AdaptiveTextBlock()
                            {
                                Text="Welcome to Railways Bot",
                                Spacing=AdaptiveSpacing.None,
                                Size=AdaptiveTextSize.Large,
                                Weight=AdaptiveTextWeight.Bolder
                            },
                            new AdaptiveTextBlock()
                            {
                                Text="Please select the type of notification you want to send",
                                Id="txt1",
                                Separator=true
                            },
                            new AdaptiveChoiceSetInput()
                            {
                                Id="choices",
                                Choices=new List<AdaptiveChoice>()
                                {
                                    
                                    new AdaptiveChoice()
                                    {
                                        Title="get special assistance card",
                                        
                                        Value="get special assistance card"
                                    },
                                    new AdaptiveChoice()
                                    {
                                        Title="get normal passenger card",
                                        Value="get normal passenger card"
                                    },
                                    new AdaptiveChoice()
                                    {
                                        Title="get available trains card",
                                        Value="get available trains card"
                                    },
                                    new AdaptiveChoice()
                                    {
                                        Title="getrefreshmentscard",
                                        Value="getrefreshmentscard"
                                    }

                                },
                                Style=AdaptiveChoiceInputStyle.Compact,
                                Value=""
                            },
                            new AdaptiveTextInput()
                            {
                                Placeholder="Enter channel id here",
                                Id="channelId",
                                Value=""
                            }
                        }
                    }
                },
                Actions=new List<AdaptiveAction>()
                {
                    new AdaptiveSubmitAction()
                    {
                        Title="Send Notification",
                        Data=new ActionDetails(){ActionType=Constants.SendNotification}
                    }
                }
            };
            return new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
        }
        public static String GetAdaptiveCardJson(string filenumber)
        {
            var path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Cards/" + filenumber + ".json");
            return File.ReadAllText(path);
        }
        public static Attachment GetAvailableTrainsCard()
        {
            var listCard = new ListCard();
            listCard.content = new Content();
            listCard.content.title = "The following trains are available at Berlin HBF";
            var list = new List<Item>();
            foreach (var i in TrainList.TrainsList)
            {
                var item = new Item();
                item.id = i.ToString();
                item.type = "resultItem";
                item.icon = ApplicationSettings.BaseUrl + "/Resources/train.jpg";
                item.title = i.id;
                item.subtitle = "Model: "+i.Model + " "+"Capacity: "+i.Capacity;
                item.tap = new Tap()
                {
                    type = "messageBack",
                    text=Constants.TrainDetails,
                    value =  Constants.TrainDetails 
                };
                list.Add(item);
            }

            listCard.content.items = list.ToArray();

            Attachment attachment = new Attachment();
            attachment.ContentType = listCard.contentType;
            attachment.Content = listCard.content;
            return attachment;
        }
        public static Attachment GetAvailableRefreshments()
        {
            var listCard = new ListCard();
            listCard.content = new Content();
            listCard.content.title = "The following refreshments are available on train ";
            var list = new List<Item>();
            foreach (var i in RefreshmentsList.Refrehsmentslist)
            {
                var item = new Item();
                item.id = i.ToString();
                item.type = "resultItem";
                item.icon = ApplicationSettings.BaseUrl + "/Resources/tea.jpg";
                item.title = "Item code:" + i.ItemCode + "| Item Name:" + i.ItemName;
                item.subtitle = "Quantity:174";
                item.tap = new Tap()
                {
                    type = "messageBack",
                    text = Constants.GetRefreshmentItem,
                    value = Constants.GetRefreshmentItem
                };
                list.Add(item);
            }

            listCard.content.items = list.ToArray();

            Attachment attachment = new Attachment();
            attachment.ContentType = listCard.contentType;
            attachment.Content = listCard.content;
            return attachment;
        }
    }
        public class ListCard
        {
            public Content content { get; set; }

            public string contentType { get; set; } = "application/vnd.microsoft.teams.card.list";
        }

        public class Content
        {
            public string title { get; set; }
            public Item[] items { get; set; }
            public Button[] buttons { get; set; }
        }

        public class Item
        {
            public string type { get; set; }
            public string title { get; set; }
            public string id { get; set; }
            public string subtitle { get; set; }
            public Tap tap { get; set; }
            public string icon { get; set; }
        }

        public class Tap
        {
            public string type { get; set; }
            public string title { get; set; }
            public string value { get; set; }
            public string text { get; set; }
        
        }

        public class Button
        {
            public string type { get; set; }
            public string title { get; set; }
            public string value { get; set; }
        }

    
}