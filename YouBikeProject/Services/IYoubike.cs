using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouBikeProject.Models;
using YouBikeProject.Models.ViewModel;

namespace YouBikeProject.Services
{
    public interface IYoubike
    {
        public Task GetYoubikeAPI();
    }
}
