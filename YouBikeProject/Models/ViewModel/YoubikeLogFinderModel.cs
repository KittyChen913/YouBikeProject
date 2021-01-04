using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YouBikeProject.Models.ViewModel
{
    public class YoubikeLogFinderModel
    {
        /// <summary>
        /// 站點代號
        /// </summary>
        public string Sno { get; set; }

        /// <summary>
        /// 場站區域
        /// </summary>
        public string Sarea { get; set; }

        /// <summary>
        /// 站點 List
        /// </summary>
        /// <value></value>
        public List<SelectListItem> stationList { get; set; }

        /// <summary>
        /// 區域 List
        /// </summary>
        /// <value></value>
        public List<SelectListItem> regionList { get; set; }
    }
}