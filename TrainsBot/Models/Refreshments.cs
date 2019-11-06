using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainsBot.Models
{
    public class RefreshmentsList
    {
        public static List<Refreshments> Refrehsmentslist { get; set; } = new List<Refreshments>()
        {
          new Refreshments() {ItemCode="Ref001",ItemName="Coffee"},
          new Refreshments() {ItemCode="Ref002",ItemName="Croissant"},
          new Refreshments() {ItemCode="Ref003",ItemName="M&Ms"},
          new Refreshments() {ItemCode="Ref004",ItemName="Earl grey tee"},

        };
    }
    public class Refreshments
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
    }
}