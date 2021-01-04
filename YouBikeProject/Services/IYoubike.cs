using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouBikeProject.Models.ViewModel;

namespace YouBikeProject.Models
{
    public interface IYoubike
    {
        public Task GetYoubikeAPI();
        public List<YouBikeStationModel> GetYouBikeStationList();
        public void AddYouBikeStation(YouBikeStationModel data);
        public YoubikeLogListViewModel GetYouBikeLogList(YoubikeLogFinderModel data);
        public void AddYouBikeLog(YouBikeLogModel data);
        public List<RegionModel> GetRegionList();
    }
}
