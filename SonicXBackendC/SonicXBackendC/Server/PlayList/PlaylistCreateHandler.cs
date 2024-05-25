using Newtonsoft.Json.Linq;
using SonicXBackendC.MusicManagment;
using SonicXBackendC.PlaylistManagement;
using SonicXBackendC.PlaylistManagment;
using SonicXBackendC.UserManagement;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;

namespace SonicXBackendC.Server.PlayList
{
    public class PlaylistCreateHandler
    {
        public void Handle(HttpListenerContext context)
        {


            if (context.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                var userService = UserService.GetInstance();
                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    var requestBody = reader.ReadToEnd();
                    var jsonRequest = JObject.Parse(requestBody);
                    var playlistName = jsonRequest["playlistName"].ToString();
                    var songNode = (JObject)jsonRequest["song"];

                    var title = songNode["title"].ToString();
                    var artist = songNode["artist"].ToString();
                    var album = songNode["album"].ToString();
                    var duration = songNode["duration"].Value<int>();
                    var path = songNode["path"].ToString();

                    PlaylistManager playlistManager = null;
                    try
                    {
                        playlistManager = new PlaylistManager();
                    }
                    catch (SqlException e)
                    {
                        throw new Exception("Error creating PlaylistManager", e);
                    }

                    try
                    {
                        playlistManager.CreatePlaylistTableForUser(userService);
                    }
                    catch (SqlException e)
                    {
                        throw new Exception("Error creating playlist table for user", e);
                    }

                    var musicFile = new MusicFile(title, artist, album, duration, path);

                    var playlist = new Playlist(playlistName);
                    playlist.AddMusicFileToPlaylist(musicFile, userService);

                    var responseJson = new JObject();
                    responseJson["success"] = true;

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
