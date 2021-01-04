using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace YouBikeProject.Models.ViewModel
{
    public class YoubikeLogListViewModel
    {
        public List<YouBikeLogViewModel> YoubikeLogList { get; set; }
    }

    public class YouBikeLogViewModel
    {
        /// <summary>
        /// Log ID
        /// </summary>
        public string LOGID { get; set; }

        /// <summary>
        /// 站點代號
        /// </summary>
        public string SNO { get; set; }

        /// <summary>
        /// 場站中文名稱
        /// </summary>
        public string SNA { get; set; }

        /// <summary>
        /// 場站區域
        /// </summary>
        public string SAREA { get; set; }

        /// <summary>
        /// 場站目前車輛數量
        /// </summary>
        public int SBI { get; set; }

        /// <summary>
        /// 空位數量
        /// </summary>
        public int BEMP { get; set; }

        /// <summary>
        /// 官方Youbike更新時間
        /// </summary>
        public string MDAY { get; set; }

        /// <summary>
        /// Log 紀錄時間
        /// </summary>
        public DateTime UPDATEDATETIME { get; set; }
    }
}
