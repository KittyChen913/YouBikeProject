using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouBikeProject.Models
{
    public class YouBikeLogModel
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
        /// 場站目前車輛數量
        /// </summary>
        public int SBI { get; set; }

        /// <summary>
        /// 官方Youbike更新時間
        /// </summary>
        public string MDAY { get; set; }

        /// <summary>
        /// 空位數量
        /// </summary>
        public int BEMP { get; set; }

        /// <summary>
        /// Log 紀錄時間
        /// </summary>
        public DateTime UPDATEDATETIME { get; set; }
    }
}
