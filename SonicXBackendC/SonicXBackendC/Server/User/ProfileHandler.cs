using Newtonsoft.Json;
using SonicXBackendC.UserManagement;
using System;
using System.IO;
using System.Net;

namespace SonicXBackendC.Server.User
{
    public class ProfileHandler
    {
        public void Handle(HttpListenerContext context)
        {
            if (context.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                var userService = UserService.GetInstance();
                var currentUser = userService.GetUser();
                if (currentUser != null)
                {
                    var objectMapper = new JsonSerializer();
                    var userData = new
                    {
                        firstName = currentUser.FirstName,
                        lastName = currentUser.LastName,
                        email = currentUser.Email
                    };

                    var responseJson = new { success = true, data = userData };
                    var response = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseJson));
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Write(response, 0, response.Length);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
        }
    }
}
