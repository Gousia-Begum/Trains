using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainsBot.Models
{
    public class TrainList
    {
        public static List<Train> TrainsList { get; set; } = new List<Train>()
        {
          new Train() {id="Train ID: ICE 611",Model="ICE 4",Capacity="second class 627 seats, first class 205 seats"},
          new Train() {id="Train ID: ICE 598",Model="ICE 2",Capacity="second class 514 seats, first class 198 seats"},

        };
    }

    public class Train
    {
        public string id { get; set; }
        public string Model { get; set; }
       
        public string Capacity { get; set; }
    }
}