using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpinShareLib;
using SpinShareLib.Types;

namespace SpinShareLibTests
{
    [TestClass]
    public class SSAPITests
    {
        private readonly SSAPI _inst;
        public SSAPITests()
        {
            _inst = new SSAPI();
        }

        [TestMethod]
        public void getPing()
        {
            Task.Run(async () => {
               Content thing = await _inst.ping();
               Console.WriteLine(thing.status);
            }).GetAwaiter().GetResult();
            
        }
        [TestMethod]
        public void getPromos()
        {
            Task.Run(async () => {
                Content<Promo[]> thing = await _inst.getPromos();
                Console.WriteLine(thing.data[0].button.data);
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void getNewSongs()
        {
            Task.Run(async () => {
                Content<Song[]> thing = await _inst.getNewSongs(0);
                Console.WriteLine(thing.data[0].id);
            }).GetAwaiter().GetResult();

        }
        [TestMethod]
        public void getHotThisWeekSongs()
        {
            Task.Run(async () => {
                Content<Song[]> thing = await _inst.getHotThisWeekSongs(0);
                Console.WriteLine(thing.data[0].id);
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void getSongDetail()
        {
            Task.Run(async () => {
                Content<SongDetail> thing = await _inst.getSongDetail("1234");
                thing.data.tags.ToList().ForEach(i => Console.WriteLine(i.ToString()));
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void downloadSongZip()
        {
            Task.Run(async () => {
                await _inst.downloadSongZip("1234", Path.Combine(Path.GetTempPath()));
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void downloadSongAndUnzip()
        {
            Task.Run(async () => {
                Console.WriteLine(Path.GetTempPath());
                await _inst.downloadSongAndUnzip("1234", Path.Combine(Path.GetTempPath()));
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void downloadSongAndUnzipAddToQueue()
        {
            Stopwatch watch = new Stopwatch();
            Task.Run(async () => {
                watch.Start();
                await _inst.downloadSongAndUnzipAddToQueue("1234", Path.Combine(Path.GetTempPath()));
                watch.Stop();
                Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
                await _inst.downloadSongAndUnzipAddToQueue("10", Path.Combine(Path.GetTempPath()));
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void getSongDetailReviews()
        {
            Task.Run(async () => {
                Content<Reviews> thing = await _inst.getSongDetailReviews("1234");
                Console.WriteLine(thing.data.reviews[0]);
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void getSongDetailSpinPlays()
        {
            Task.Run(async () => {
                Content<SpinPlays> thing = await _inst.getSongDetailSpinPlays("1234");
                Console.WriteLine(thing.data.spinPlays[0].videoUrl);
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void getPlaylist()
        {
            Task.Run(async () => {
                Content<Playlist> thing = await _inst.getPlaylist("10");
                Console.WriteLine(thing.data.user.username);
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void getUserDetail()
        {
            Task.Run(async () => {
                Content<UserDetail> thing = await _inst.getUserDetail("72");
                Console.WriteLine(thing.data.username);
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void getUserCharts()
        {
            Task.Run(async () => {
                Content<Song[]> thing = await _inst.getUserCharts("72");
                Console.WriteLine(thing.data[0].title);
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void getUserReviews()
        {
            Task.Run(async () => {
                Content<Reviews.Review[]> thing = await _inst.getUserReviews("72");
                Console.WriteLine(thing.data[0].id);
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void getUserSpinPlays()
        {
            Task.Run(async () => {
                Content<SpinPlays.Spinplay[]> thing = await _inst.getUserSpinPlays("20");
                Console.WriteLine(thing.data[0].id);
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void getUserPlaylists()
        {
            Task.Run(async () => {
                Content<Playlist[]> thing = await _inst.getUserPlaylists("278");
                Console.WriteLine(thing.data[0].id);
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void getSearch()
        {
            Task.Run(async () => {
                Content<Search> thing = await _inst.search("ayanamy");
                Console.WriteLine(thing.data.users[0].id);
            }).GetAwaiter().GetResult();
        }
    }
    
    [TestClass]
    public class SSAPILargeTests {
        private readonly SSAPI _inst;
        public SSAPILargeTests()
        {
            HttpClient client = new HttpClient();
            _inst = new SSAPI(client);
        }
        
        [TestMethod]
        public void getTournamentMapPool()
        {
            Task.Run(async () => {
                Content<SongDetailTournament[]> thing = await _inst.getTournamentMapPool();
                thing.data[32].tags.ToList().ForEach(i => Console.WriteLine(i.ToString()));
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void searchAll()
        {
            Task.Run(async () => {
                Content<Search> thing = await _inst.searchAll();
                Console.WriteLine(thing.data.songs.Length);
            }).GetAwaiter().GetResult();
        }
    }
}