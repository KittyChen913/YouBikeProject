using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using YouBikeProject.Models;
using YouBikeProject.Models.ViewModel;

namespace YouBikeProject.Services
{
    public class DBHelpers : IDBHelpers
    {
        private readonly string YouBikeDBConnectionString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DBHelpers> _logger;
        public DBHelpers(ILogger<DBHelpers> _logger, IConfiguration _configuration)
        {
            this._logger = _logger;
            this._configuration = _configuration;
            YouBikeDBConnectionString = _configuration.GetConnectionString("YouBikePostgreDBString");
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

        public YoubikeLogListViewModel GetYouBikeLogList(YoubikeLogFinderModel data)
        {
            YoubikeLogListViewModel youBikeLogList = new YoubikeLogListViewModel();

            try
            {
                using (var conn = new NpgsqlConnection(YouBikeDBConnectionString))
                {
                    string sql = @" SELECT logid, yl.sno, sna, sarea, sbi, mday, bemp, updatedatetime 
                                    FROM youbikelog yl
                                    LEFT JOIN youbikestation ys ON yl.sno = ys.sno ";

                    string wheresql = string.Empty;

                    if (!string.IsNullOrWhiteSpace(data.Sno))
                    {
                        wheresql = string.Concat(wheresql, "yl.sno = @sno");
                    }
                    if (!string.IsNullOrWhiteSpace(data.Sarea))
                    {
                        wheresql = string.Concat(wheresql,
                            (string.IsNullOrWhiteSpace(wheresql) ? string.Empty : " AND "), "ys.sarea = @Sarea");
                    }

                    if (wheresql.Length > 0)
                    {
                        sql = string.Concat(sql, " WHERE ", wheresql);
                        youBikeLogList.YoubikeLogList = conn.Query<YouBikeLogViewModel>(sql, data).ToList();
                    }
                    else
                    {
                        youBikeLogList.YoubikeLogList = conn.Query<YouBikeLogViewModel>(sql).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return youBikeLogList;
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

        public List<RegionModel> GetRegionList()
        {
            List<RegionModel> regionList = new List<RegionModel>();

            try
            {
                using (var conn = new NpgsqlConnection(YouBikeDBConnectionString))
                {
                    string sql = @" SELECT zipcode, cityname, cityengname, areaname, areaengname
	                                FROM region;";

                    regionList = conn.Query<RegionModel>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return regionList;
        }
    }
}