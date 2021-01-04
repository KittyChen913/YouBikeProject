using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using YouBikeProject.Models;
using YouBikeProject.Models.ViewModel;

namespace YouBikeProject.Controllers
{
    public class YoubikeController : Controller
    {
        private readonly ILogger<YoubikeController> _logger;
        private IYoubike _youbike;

        public YoubikeController(ILogger<YoubikeController> logger, IYoubike youbike)
        {
            _logger = logger;
            _youbike = youbike;
        }

        public IActionResult SearchYoubikeLog()
        {
            YoubikeLogFinderModel model = new YoubikeLogFinderModel();

            model.stationList = _youbike.GetYouBikeStationList()
                .Select(n => new SelectListItem() { Text = n.SNA, Value = n.SNO }).ToList();

            model.regionList = _youbike.GetRegionList().Select(n => new SelectListItem() { Text = n.AREANAME, Value = n.AREANAME }).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult YoubikeLogList(YoubikeLogFinderModel model)
        {
            YoubikeLogListViewModel YouBikeLogListModel = _youbike.GetYouBikeLogList(model);
            return PartialView("_YoubikeLogList", YouBikeLogListModel);
        }
    }
}
