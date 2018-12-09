using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CameraGearViewer.Classes
{
    public class GearComponent
    {
        public string Name { get; set; }
        public double Price { get; set; }

        [Key]
        public string ForumLink { get; set; }
        public DateTime Date { get; set; }

        [JsonConstructor]
        public GearComponent(string name, double price, string forumLink, DateTime date)
        {
            Name = name;
            Price = price;
            ForumLink = forumLink;
            Date = date;
        }
    }
}
