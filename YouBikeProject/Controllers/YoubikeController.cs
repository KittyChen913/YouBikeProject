using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using YouBikeProject.Models;
using YouBikeProject.Models.ViewModel;
using YouBikeProject.Services;

namespace YouBikeProject.Controllers
{
    public class YoubikeController : Controller
    {
        private readonly ILogger<YoubikeController> _logger;
        private IYoubike _youbike;
        private readonly IDBHelpers _dBHelpers;

        public YoubikeController(ILogger<YoubikeController> logger, IYoubike youbike, IDBHelpers dBHelpers)
        {
            this._dBHelpers = dBHelpers;
            _logger = logger;
            _youbike = youbike;
        }

        public IActionResult SearchYoubikeLog()
        {
            YoubikeLogFinderModel model = new YoubikeLogFinderModel();

            model.stationList = _dBHelpers.GetYouBikeStationList()
                .Select(n => new SelectListItem() { Text = n.SNA, Value = n.SNO }).ToList();

            model.regionList = _dBHelpers.GetRegionList().Select(n => new SelectListItem() { Text = n.AREANAME, Value = n.AREANAME }).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult YoubikeLogList(YoubikeLogFinderModel model)
        {
            YoubikeLogListViewModel YouBikeLogListModel = _dBHelpers.GetYouBikeLogList(model);
            return PartialView("_YoubikeLogList", YouBikeLogListModel);
        }

        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json")]
        public List<RegionModel> GetRegionList()
        {
            List<RegionModel> regionList = _dBHelpers.GetRegionList();
            return regionList;
        }

        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json")]
        public List<YouBikeStationModel> GetYouBikeStationList()
        {
            List<YouBikeStationModel> regionList = _dBHelpers.GetYouBikeStationList();
            return regionList;
        }

        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json")]
        public List<YouBikeLogViewModel> GetYoubikeLogList([FromBody]YoubikeLogFinderModel model)
        {
            YoubikeLogListViewModel YouBikeLogListModel = _dBHelpers.GetYouBikeLogList(model);
            return YouBikeLogListModel.YoubikeLogList;
        }
    }
}
