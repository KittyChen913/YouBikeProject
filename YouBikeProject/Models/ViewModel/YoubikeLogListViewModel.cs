using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YouBikeProject.Models.ViewModel
{
    public class YoubikeLogListViewModel
    {
        public List<SelectListItem> stationList { get; set; }
        public List<SelectListItem> regionList { get; set; }
        public string Sno { get; set; }
        public string Sarea { get; set; }
    }
}
