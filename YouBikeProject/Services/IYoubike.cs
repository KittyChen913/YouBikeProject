using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouBikeProject.Models
{
    public interface IYoubike
    {
        public Task GetYoubikeAPI();
        public List<YouBikeStationModel> GetYouBikeStationList();
        public void AddYouBikeStation(YouBikeStationModel data);
        public void AddYouBikeLog(YouBikeLogModel data);

    }
}
