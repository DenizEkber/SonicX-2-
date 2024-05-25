using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SonicXBackendC.MusicManagment;
using SonicXBackendC.PlaylistManagment;
using SonicXBackendC.UserManagement;
using System.Collections.Generic;
using System.IO;
using System.Net;


namespace SonicXBackendC.Server.PlayList
{
    public class PlaylistShowContent
    {
        public void Handle(HttpListenerContext context)
        {
            if (context.Request.HttpMethod.Equals("POST"))
            {
                var objectMapper = new JsonSerializer();
                var userService = UserService.GetInstance();

                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    var requestBody = reader.ReadToEnd();
                    var jsonRequest = JObject.Parse(requestBody);
                    var playlistName = jsonRequest["playlistName"].ToString();

                    Playlist playlist = new Playlist(playlistName);
                    List<MusicFile> musicFiles = playlist.GetAllMusicFiles(userService);

                    var responseJson = new JObject();
                    var dataArray = new JArray();

                    foreach (MusicFile musicFile in musicFiles)
                    {
                        var musicNode = new JObject();
                        musicNode["title"] = musicFile.Title;
                        musicNode["playlistName"] = playlistName;
                        musicNode["artist"] = musicFile.Artist;
                        musicNode["album"] = musicFile.Album;
                        musicNode["path"] = musicFile.Path;

                        dataArray.Add(musicNode);
                    }

                    responseJson["musicFiles"] = dataArray;

                    var response = System.Text.Encoding.UTF8.GetBytes(responseJson.ToString());
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Write(response, 0, response.Length);
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
        }

    }
}
