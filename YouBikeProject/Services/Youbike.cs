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

namespace YouBikeProject.Models
{
    public class Youbike : IYoubike
    {
        private readonly ILogger<Youbike> _logger;
        private IHttpClientFactory _httpClientFactory;
        public IConfiguration _configuration;
        private readonly string YoubikeRequestUrl;
        private readonly string YouBikeDBConnectionString;

        public Youbike(ILogger<Youbike> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            YoubikeRequestUrl = _configuration.GetValue<string>("RequestAPIUrl:YoubikeAPI");
            YouBikeDBConnectionString = _configuration.GetConnectionString("YouBikePostgreDBString");
        }

        public async Task GetYoubikeAPI()
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(YoubikeRequestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var jObject = JObject.Parse(responseContent);
                    var contentValue = jObject["retVal"].Children().Values();

                    var stationList = GetYouBikeStationList().ToDictionary(n => n.SNO, n => n.SNA);

                    foreach (var item in contentValue)
                    {
                        //已停用的站點不再更新紀錄
                        if (item.Value<int>("act") == 1)
                        {
                            if (!stationList.ContainsKey(item.Value<string>("sno")))
                            {
                                AddYouBikeStation(new YouBikeStationModel()
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
                                AddYouBikeLog(new YouBikeLogModel()
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

        public List<YouBikeStationModel> GetYouBikeStationList()
        {
            List<YouBikeStationModel> youBikeStationsList = new List<YouBikeStationModel>();

            try
            {
                using (var conn = new NpgsqlConnection(YouBikeDBConnectionString))
                {
                    string sql = @" SELECT sno, sna, tot, sarea, lat, lng, ar, sareaen, snaen, aren, act
                                    FROM youbikestation";

                    youBikeStationsList = conn.Query<YouBikeStationModel>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return youBikeStationsList;
        }

        public void AddYouBikeStation(YouBikeStationModel data)
        {
            try
            {
                using (var conn = new NpgsqlConnection(YouBikeDBConnectionString))
                {
                    string sql = @" INSERT INTO youbikestation(
                                        sno, sna, tot, sarea, lat, lng, ar, sareaen, snaen, aren, act)
	                                VALUES(@sno, @sna, @tot, @sarea, @lat, @lng, @ar, @sareaen, @snaen, @aren, @act);";

                    conn.Execute(sql, data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public void AddYouBikeLog(YouBikeLogModel data)
        {
            try
            {
                using (var conn = new NpgsqlConnection(YouBikeDBConnectionString))
                {
                    string sql = @" INSERT INTO youbikelog(
	                                    logid, sno, sbi, mday, bemp, updatedatetime)
	                                VALUES (@logid, @sno, @sbi, @mday, @bemp, @updatedatetime);";

                    conn.Execute(sql, data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
