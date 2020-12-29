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

        public IActionResult SearchYoubikeLog(YoubikeLogListViewModel model)
        {
            model.stationList = _youbike.GetYouBikeStationList()
                .Select(n => new SelectListItem() { Text = n.SNA, Value = n.SNO }).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult YoubikeLogList(YoubikeLogListViewModel model)
        {
            List<YouBikeLogModel> YouBikeLogListModel = _youbike.GetYouBikeLogList(model);
            return PartialView("_YoubikeLogList", YouBikeLogListModel);
        }
    }
}
