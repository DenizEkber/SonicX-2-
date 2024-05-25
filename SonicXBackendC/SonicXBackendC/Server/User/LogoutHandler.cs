using Newtonsoft.Json;
using SonicXBackendC.UserManagement;
using System;
using System.IO;
using System.Net;

namespace SonicXBackendC.Server.User
{
    public class LogoutHandler
    {
        public  void Handle(HttpListenerContext context)
        {
            if (context.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                var objectMapper = new JsonSerializer();

                var userService = UserService.GetInstance();
                userService.Logout();

                var responseJson = new { success = true };
                var response = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseJson));
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
