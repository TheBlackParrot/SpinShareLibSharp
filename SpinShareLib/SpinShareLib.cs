using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using SpinShareLib.Types;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SpinShareLib
{
    public class SSAPI
    {
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable MemberCanBePrivate.Global
        public string apiBase { get; private set; }
        public int supportedVersion { get; private set; }
        public HttpClient client { get; private set; }
        public SemaphoreSlim semaphore { get; private set; }
        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Local
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public SSAPI() : this(new HttpClient()) { }
        public SSAPI(HttpClient client)
        {
            apiBase = "https://spinsha.re/api/";
            supportedVersion = 1;
            client.DefaultRequestHeaders.UserAgent.ParseAdd($"Mozilla/5.0 (compatible; {nameof(SpinShareLib)}/{Assembly.GetExecutingAssembly().GetName().Version})");
            this.client = client;
            semaphore = new SemaphoreSlim(1);
        }
        
        public async Task<Content> ping()
        {
            return await getApiResultAsType<Content>($"{apiBase}ping");
        }
        public async Task<Content<Promo[]>> getPromos()
        {
            return await getApiResultAsType<Content<Promo[]>>($"{apiBase}promos");
        }
        public async Task<Content<Song[]>> getNewSongs(int offset)
        {
            return await getApiResultAsType<Content<Song[]>>($"{apiBase}songs/new/{offset}");
        }
        public async Task<Content<Song[]>> getHotThisWeekSongs(int offset)
        {
            return await getApiResultAsType<Content<Song[]>>($"{apiBase}songs/hotThisWeek/{offset}");
        }
        public async Task<Content<SongDetailTournament[]>> getTournamentMapPool()
        {
            return await getApiResultAsType<Content<SongDetailTournament[]>>($"{apiBase}tournament/mappool");
        }
        public async Task<Content<SongDetail>> getSongDetail(string songId)
        {
            return await getApiResultAsType<Content<SongDetail>>($"{apiBase}song/{songId}");
        }
        // ReSharper disable once MemberCanBePrivate.Global
        public async Task<(HttpResponseMessage response, Content<SongDetail> songdetail)> downloadSongZipStream(string songId)
        {
            Content<SongDetail> song = await getSongDetail(songId);
            return (await client.GetAsync(song.data.paths.zip), song);
        }
        public async Task<bool> downloadSongZip(string songId, string path)
        {
            try
            {
                (HttpResponseMessage response, Content<SongDetail> songdetail) tup = await downloadSongZipStream(songId);
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    await tup.response.Content.CopyToAsync(fs);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> downloadSongAndUnzip(string songId, string directoryPath)
        {
            string tempFileName = Path.GetTempFileName();
            await downloadSongZip(songId, tempFileName);
            System.IO.Compression.ZipFile.ExtractToDirectory(tempFileName, directoryPath);
            File.Delete(tempFileName);
            return true;
        }
        public async Task<bool> downloadSongAndUnzipAddToQueue(string songId, string directoryPath)
        {
            await semaphore.WaitAsync();
            try
            {
                return await downloadSongAndUnzip(songId, directoryPath);
            }
            finally
            {
                semaphore.Release();
            }
        }
        public async Task<Content<Reviews>> getSongDetailReviews(string songId)
        {
            return await getApiResultAsType<Content<Reviews>>($"{apiBase}song/{songId}/reviews");
        }
        public async Task<Content<SpinPlays>> getSongDetailSpinPlays(string songId)
        {
            return await getApiResultAsType<Content<SpinPlays>>($"{apiBase}song/{songId}/spinplays");
        }
        public async Task<Content<Playlist>> getPlaylist(string playlistId)
        {
            return await getApiResultAsType<Content<Playlist>>($"{apiBase}playlist/{playlistId}");
        }
        public async Task<Content<UserDetail>> getUserDetail(string userId)
        {
            return await getApiResultAsType<Content<UserDetail>>($"{apiBase}user/{userId}");
        }
        public async Task<Content<Song[]>> getUserCharts(string userId)
        {
            return await getApiResultAsType<Content<Song[]>>($"{apiBase}user/{userId}/charts");
        }
        public async Task<Content<Reviews.Review[]>> getUserReviews(string userId)
        {
            return await getApiResultAsType<Content<Reviews.Review[]>>($"{apiBase}user/{userId}/reviews");
        }
        public async Task<Content<SpinPlays.Spinplay[]>> getUserSpinPlays(string userId)
        {
            return await getApiResultAsType<Content<SpinPlays.Spinplay[]>>($"{apiBase}user/{userId}/spinplays");
        }
        public async Task<Content<Playlist[]>> getUserPlaylists(string userId)
        {
            return await getApiResultAsType<Content<Playlist[]>>($"{apiBase}user/{userId}/playlists");
        }
        public async Task<Content<Search>> search(string query)
        {
            return await getApiResultAsType<Content<Search>>($"{apiBase}search/{query}");
        }
        public async Task<Content<Search>> searchAll()
        {
            return await getApiResultAsType<Content<Search>>($"{apiBase}searchAll");
        }
        private async Task<T> getApiResultAsType<T>(string apiPath)
        {
            HttpResponseMessage resp = await client.GetAsync(apiPath);
            if (resp.IsSuccessStatusCode)
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true,
                    IncludeFields = true,
                    Converters = { new DateTimeParse() }
                };
                return JsonSerializer.Deserialize<T>(await resp.Content.ReadAsStringAsync(), options);
            }

            throw new HttpRequestException("Status code is not OK");
        }
    }
}
