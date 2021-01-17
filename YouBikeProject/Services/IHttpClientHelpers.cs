using System.Threading.Tasks;

namespace YouBikeProject.Services
{
    public interface IHttpClientHelpers
    {
        public Task<string> GetAPI(string requestUrl);
    }
}