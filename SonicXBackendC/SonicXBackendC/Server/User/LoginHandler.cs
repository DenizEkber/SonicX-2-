using Newtonsoft.Json;
using SonicXBackendC.UserManagement;
using System;
using System.IO;
using System.Net;

namespace SonicXBackendC.Server.User
{
    public class LoginHandler
    {
        public void Handle(HttpListenerContext context)
        {
            if (context.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                var objectMapper = new JsonSerializer();
                var requestData = default(RequestData);
                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    var requestBody = reader.ReadToEnd();
                    requestData = JsonConvert.DeserializeObject<RequestData>(requestBody);
                }

                Console.WriteLine("Gelen veri: " + requestData);

                var userService = UserService.GetInstance();
                bool success = userService.Login(requestData.Email, requestData.Password);

                var responseJson = new { success };
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
