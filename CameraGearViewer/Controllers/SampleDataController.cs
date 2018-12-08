using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CameraGearViewer.Classes;
using Microsoft.AspNetCore.Mvc;

namespace CameraGearViewer.Controllers
{
    [Route("api/controller")]
    public class SampleDataController : Controller
    {
        private static GearComponent[] gearComponents = new[]
        {
            new GearComponent("Camera", "Kamera", DateTime.Now.ToShortDateString())
        };

        [HttpGet("get")]
        public IEnumerable<GearComponent> GetGearComponents()
        {
            return gearComponents;
        }
    }
}
