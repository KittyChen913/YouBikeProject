using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YouBikeProject.Models;
using YouBikeProject.Services;

namespace YouBikeProject.Tests
{
    public class YouBikeTest
    {
        private Mock<ILogger<Youbike>> mockLogger = new Mock<ILogger<Youbike>>();
        private IConfigurationRoot config;

        [SetUp]
        public void Setup()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                 .AddInMemoryCollection(new Dictionary<string, string>
             {
                { "RequestAPIUrl:YoubikeAPI", "mockUrl" }
             });

            config = builder.Build();
        }

        [TestCase(0)]
        public async Task Not_AddYouBikeStation_Not_AddYouBikeLog(int act)
        {
            Mock<IDBHelpers> mockDBHelpers = new Mock<IDBHelpers>();
            mockDBHelpers.Setup(s => s.GetYouBikeStationList()).Returns(new List<YouBikeStationModel>());

            Mock<IHttpClientHelpers> mockHttpClientHelpers = new Mock<IHttpClientHelpers>();
            mockHttpClientHelpers.Setup(s => s.GetAPI(It.IsAny<string>()))
                .Returns(Task.FromResult(@"{'retCode':1,
                'retVal':{'0001':{'sno':'0001','sna':'','tot':'0','sbi':'0','sarea':'',
                'mday':'" + DateTime.Now.AddMinutes(-20).ToString("yyyyMMddHHmmss") + @"',
                'lat':'','lng':'','ar':'','sareaen':'','snaen':'','aren':'',
                'bemp':'0','act':'" + act + "'}}}"));

            Youbike _Youike = new Youbike(mockLogger.Object, config, mockHttpClientHelpers.Object, mockDBHelpers.Object);

            await _Youike.GetYoubikeAPI();
            mockDBHelpers.Verify(v => v.AddYouBikeStation(It.IsAny<YouBikeStationModel>()), Times.Never());
            mockDBHelpers.Verify(v => v.AddYouBikeLog(It.IsAny<YouBikeLogModel>()), Times.Never());
        }

        [TestCase(1)]
        public async Task Add_YouBikeStation_And_Add_YouBikeLog(int act)
        {
            Mock<IDBHelpers> mockDBHelpers = new Mock<IDBHelpers>();
            mockDBHelpers.Setup(s => s.GetYouBikeStationList()).Returns(new List<YouBikeStationModel>());

            Mock<IHttpClientHelpers> mockHttpClientHelpers = new Mock<IHttpClientHelpers>();
            mockHttpClientHelpers.Setup(s => s.GetAPI(It.IsAny<string>()))
                .Returns(Task.FromResult(@"{'retCode':1,
                'retVal':{'0001':{'sno':'0001','sna':'','tot':'0','sbi':'0','sarea':'',
                'mday':'" + DateTime.Now.AddMinutes(-20).ToString("yyyyMMddHHmmss") + @"',
                'lat':'','lng':'','ar':'','sareaen':'','snaen':'','aren':'',
                'bemp':'0','act':'" + act + "'}}}"));

            Youbike _Youike = new Youbike(mockLogger.Object, config, mockHttpClientHelpers.Object, mockDBHelpers.Object);

            await _Youike.GetYoubikeAPI();
            mockDBHelpers.Verify(v => v.AddYouBikeStation(It.IsAny<YouBikeStationModel>()), Times.Once());
            mockDBHelpers.Verify(v => v.AddYouBikeLog(It.IsAny<YouBikeLogModel>()), Times.Once());
        }

        [TestCase("0001")]
        public async Task Not_Add_YouBikeStation(string sno)
        {
            Mock<IDBHelpers> mockDBHelpers = new Mock<IDBHelpers>();
            mockDBHelpers.Setup(s => s.GetYouBikeStationList())
                .Returns(new List<YouBikeStationModel>() { new YouBikeStationModel() { SNO = sno } });

            Mock<IHttpClientHelpers> mockHttpClientHelpers = new Mock<IHttpClientHelpers>();
            mockHttpClientHelpers.Setup(s => s.GetAPI(It.IsAny<string>()))
                .Returns(Task.FromResult(@"{'retCode':1,
                'retVal':{'0001':{'sno':'0001','sna':'','tot':'0','sbi':'0','sarea':'',
                'mday':'" + DateTime.Now.AddMinutes(-20).ToString("yyyyMMddHHmmss") + @"',
                'lat':'','lng':'','ar':'','sareaen':'','snaen':'','aren':'','bemp':'0','act':'1'}}}"));

            Youbike _Youike = new Youbike(mockLogger.Object, config, mockHttpClientHelpers.Object, mockDBHelpers.Object);

            await _Youike.GetYoubikeAPI();
            mockDBHelpers.Verify(v => v.AddYouBikeStation(It.IsAny<YouBikeStationModel>()), Times.Never());
        }

        [TestCase("0002")]
        [TestCase("0003")]
        public async Task Add_YouBikeStation_Once(string sno)
        {
            Mock<IDBHelpers> mockDBHelpers = new Mock<IDBHelpers>();
            mockDBHelpers.Setup(s => s.GetYouBikeStationList())
                .Returns(new List<YouBikeStationModel>() { new YouBikeStationModel() { SNO = sno } });

            Mock<IHttpClientHelpers> mockHttpClientHelpers = new Mock<IHttpClientHelpers>();
            mockHttpClientHelpers.Setup(s => s.GetAPI(It.IsAny<string>()))
                .Returns(Task.FromResult(@"{'retCode':1,
                'retVal':{'0001':{'sno':'0001','sna':'','tot':'0','sbi':'0','sarea':'',
                'mday':'" + DateTime.Now.AddMinutes(-20).ToString("yyyyMMddHHmmss") + @"',
                'lat':'','lng':'','ar':'','sareaen':'','snaen':'','aren':'','bemp':'0','act':'1'}}}"));

            Youbike _Youike = new Youbike(mockLogger.Object, config, mockHttpClientHelpers.Object, mockDBHelpers.Object);

            await _Youike.GetYoubikeAPI();
            mockDBHelpers.Verify(v => v.AddYouBikeStation(It.IsAny<YouBikeStationModel>()), Times.Once());
        }

        [TestCase(70)]
        public async Task Not_Add_YouBikeLog(int addMinutes)
        {
            Mock<IDBHelpers> mockDBHelpers = new Mock<IDBHelpers>();
            mockDBHelpers.Setup(s => s.GetYouBikeStationList()).Returns(new List<YouBikeStationModel>());

            Mock<IHttpClientHelpers> mockHttpClientHelpers = new Mock<IHttpClientHelpers>();
            mockHttpClientHelpers.Setup(s => s.GetAPI(It.IsAny<string>()))
                .Returns(Task.FromResult(@"{'retCode':1,
                'retVal':{'0001':{'sno':'0001','sna':'','tot':'0','sbi':'0','sarea':'',
                'mday':'" + DateTime.Now.AddMinutes(-addMinutes).ToString("yyyyMMddHHmmss") + @"',
                'lat':'','lng':'','ar':'','sareaen':'','snaen':'','aren':'','bemp':'0','act':'1'}}}"));

            Youbike _Youike = new Youbike(mockLogger.Object, config, mockHttpClientHelpers.Object, mockDBHelpers.Object);

            await _Youike.GetYoubikeAPI();
            mockDBHelpers.Verify(v => v.AddYouBikeLog(It.IsAny<YouBikeLogModel>()), Times.Never());
        }

        [TestCase(20)]
        public async Task Add_YouBikeLog_Once(int addMinutes)
        {
            Mock<IDBHelpers> mockDBHelpers = new Mock<IDBHelpers>();
            mockDBHelpers.Setup(s => s.GetYouBikeStationList()).Returns(new List<YouBikeStationModel>());

            Mock<IHttpClientHelpers> mockHttpClientHelpers = new Mock<IHttpClientHelpers>();
            mockHttpClientHelpers.Setup(s => s.GetAPI(It.IsAny<string>()))
                .Returns(Task.FromResult(@"{'retCode':1,
                'retVal':{'0001':{'sno':'0001','sna':'','tot':'0','sbi':'0','sarea':'',
                'mday':'" + DateTime.Now.AddMinutes(-addMinutes).ToString("yyyyMMddHHmmss") + @"',
                'lat':'','lng':'','ar':'','sareaen':'','snaen':'','aren':'','bemp':'0','act':'1'}}}"));

            Youbike _Youike = new Youbike(mockLogger.Object, config, mockHttpClientHelpers.Object, mockDBHelpers.Object);

            await _Youike.GetYoubikeAPI();
            mockDBHelpers.Verify(v => v.AddYouBikeLog(It.IsAny<YouBikeLogModel>()), Times.Once());
        }
    }
}