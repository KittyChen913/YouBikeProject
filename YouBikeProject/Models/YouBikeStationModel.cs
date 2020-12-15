using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouBikeProject.Models
{
    public class YouBikeStationModel
    {
        /// <summary>
        /// 站點代號
        /// </summary>
        public string SNO { get; set; }

        /// <summary>
        /// 場站中文名稱
        /// </summary>
        public string SNA { get; set; }

        /// <summary>
        /// 場站總停車格
        /// </summary>
        public int TOT { get; set; }

        /// <summary>
        /// 場站區域
        /// </summary>
        public string SAREA { get; set; }

        /// <summary>
        /// 緯度
        /// </summary>
        public string LAT { get; set; }

        /// <summary>
        /// 經度
        /// </summary>
        public string LNG { get; set; }

        /// <summary>
        /// 地點
        /// </summary>
        public string AR { get; set; }

        /// <summary>
        /// 場站區域英文
        /// </summary>
        public string SAREAEN { get; set; }

        /// <summary>
        /// 場站名稱英文
        /// </summary>
        public string SNAEN { get; set; }

        /// <summary>
        /// 地址英文
        /// </summary>
        public string AREN { get; set; }

        /// <summary>
        /// 全站禁用狀態
        /// </summary>
        public int ACT { get; set; }
    }
}
