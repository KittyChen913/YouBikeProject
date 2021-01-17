using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Npgsql;
using Dapper;
using YouBikeProject.Models.ViewModel;
using YouBikeProject.Models;

namespace YouBikeProject.Services
{
    public class Youbike : IYoubike
    {
        private readonly ILogger<Youbike> _logger;
        public IConfiguration _configuration;
        private readonly string YoubikeRequestUrl;
        private readonly IHttpClientHelpers _httpClientHelpers;
        private readonly IDBHelpers _dBHelpers;

        public Youbike(ILogger<Youbike> logger, IConfiguration configuration, IHttpClientHelpers httpClientHelpers, IDBHelpers dBHelpers)
        {
            this._dBHelpers = dBHelpers;
            this._httpClientHelpers = httpClientHelpers;
            _logger = logger;
            _configuration = configuration;
            YoubikeRequestUrl = _configuration.GetValue<string>("RequestAPIUrl:YoubikeAPI");
        }

        public async Task GetYoubikeAPI()
        {
            try
            {
                string responseContent = await _httpClientHelpers.GetAPI(YoubikeRequestUrl);

                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    var jObject = JObject.Parse(responseContent);
                    var contentValue = jObject["retVal"].Children().Values();

                    var stationList = _dBHelpers.GetYouBikeStationList().ToDictionary(n => n.SNO, n => n.SNA);

                    foreach (var item in contentValue)
                    {
                        //已停用的站點不再更新紀錄
                        if (item.Value<int>("act") == 1)
                        {
                            if (!stationList.ContainsKey(item.Value<string>("sno")))
                            {
                                _dBHelpers.AddYouBikeStation(new YouBikeStationModel()
                                {
                                    SNO = item.Value<string>("sno"),
                                    SNA = item.Value<string>("sna"),
                                    TOT = item.Value<int>("tot"),
                                    SAREA = item.Value<string>("sarea"),
                                    LAT = item.Value<string>("lat"),
                                    LNG = item.Value<string>("lng"),
                                    AR = item.Value<string>("ar"),
                                    SAREAEN = item.Value<string>("sareaen"),
                                    SNAEN = item.Value<string>("snaen"),
                                    AREN = item.Value<string>("aren"),
                                    ACT = item.Value<int>("act")
                                });
                            }

                            DateTime _mday;
                            if (DateTime.TryParseExact(item.Value<string>("mday"), "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out _mday)
                                && _mday.CompareTo(DateTime.Now.AddHours(-1)) >= 0)
                            {
                                _dBHelpers.AddYouBikeLog(new YouBikeLogModel()
                                {
                                    LOGID = string.Concat(item.Value<string>("sno"), item.Value<string>("mday")),
                                    SNO = item.Value<string>("sno"),
                                    SBI = item.Value<int>("sbi"),
                                    MDAY = item.Value<string>("mday"),
                                    BEMP = item.Value<int>("bemp"),
                                    UPDATEDATETIME = DateTime.Now
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
