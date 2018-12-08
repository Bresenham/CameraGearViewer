using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CameraGearViewer.Classes
{
    public class GearComponent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }

        public GearComponent(string name, string description, string date)
        {
            Name = name;
            Description = description;
            Date = date;
        }
    }
}
