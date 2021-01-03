using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouBikeProject.Models
{
    public class RegionModel
    {
        /// <summary>
        /// 地區ID
        /// </summary>
        public string ZIPCODE { get; set; }

        /// <summary>
        /// 城市名稱-中文
        /// </summary>
        public string CITYNAME { get; set; }

        /// <summary>
        /// 城市名稱-英文
        /// </summary>
        public string CITYENGNAME { get; set; }

        /// <summary>
        /// 區名稱-中文
        /// </summary>
        public string AREANAME { get; set; }

        /// <summary>
        /// 區名稱-英文
        /// </summary>
        public string AREAENGNAME { get; set; }
    }
}
