using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SonicXBackendC.PlaylistManagment;
using SonicXBackendC.UserManagement;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;


namespace SonicXBackendC.Server.PlayList
{
    public class PlaylistShowHandler
    {
        public void Handle(HttpListenerContext context)
        {
            if (context.Request.HttpMethod.Equals("GET"))
            {
                var objectMapper = new JsonSerializer();
                var userService = UserService.GetInstance();
                PlaylistDAO playlistDAO = null;
                try
                {
                    playlistDAO = new PlaylistDAO();
                }
                catch (SqlException e)
                {
                    throw new Exception("Error creating PlaylistDAO", e);
                }

                List<string> playlists = null;
                try
                {
                    playlists = playlistDAO.GetAllPlaylistNames(userService);
                }
                catch (SqlException e)
                {
                    throw new Exception("Error getting all playlist names", e);
                }

                var arrayNode = new JArray();
                foreach (var playlist in playlists)
                {
                    arrayNode.Add(playlist);
                }

                var responseJson = new JObject();
                responseJson["playlists"] = arrayNode;

                var response = System.Text.Encoding.UTF8.GetBytes(responseJson.ToString());
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Write(response, 0, response.Length);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
        }

    }
}
