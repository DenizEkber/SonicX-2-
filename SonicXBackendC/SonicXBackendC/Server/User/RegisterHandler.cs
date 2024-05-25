using Newtonsoft.Json;
using SonicXBackendC.UserManagement;
using System;
using System.IO;
using System.Net;

namespace SonicXBackendC.Server.User
{
    public class RegisterHandler
    {
        public void Handle(HttpListenerContext context)
        {
            if (context.Request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    var requestBody = reader.ReadToEnd();
                    var requestData = JsonConvert.DeserializeObject<RequestData>(requestBody);

                    Console.WriteLine("Gelen veri: " + requestData);

                    var userService = UserService.GetInstance();
                    var isSuccess = userService.Register(requestData.FirstName, requestData.LastName, requestData.Email, requestData.Password);
                    Console.WriteLine(isSuccess);
                    var responseJson = new { success = isSuccess };
                    var response = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseJson));
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Write(response, 0, response.Length);
                }

                context.Response.Close();
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                context.Response.Close();
            }
        }
    }
}
