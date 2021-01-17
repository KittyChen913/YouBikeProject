using System.Collections.Generic;
using YouBikeProject.Models;
using YouBikeProject.Models.ViewModel;

namespace YouBikeProject.Services
{
    public interface IDBHelpers
    {
        public List<YouBikeStationModel> GetYouBikeStationList();
        public void AddYouBikeStation(YouBikeStationModel data);
        public YoubikeLogListViewModel GetYouBikeLogList(YoubikeLogFinderModel data);
        public void AddYouBikeLog(YouBikeLogModel data);
        public List<RegionModel> GetRegionList();
    }
}