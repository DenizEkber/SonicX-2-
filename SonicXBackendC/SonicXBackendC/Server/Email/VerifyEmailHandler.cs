using System.IO;
using Newtonsoft.Json;
using System.Net;
using SonicXBackendC.UserManagement;
using System.Text;
using System;

namespace SonicXBackendC.Server.Email
{
    public class VerifyEmailHandler
    {
        public void Handle(HttpListenerContext context)
        {
            if (context.Request.HttpMethod == "POST")
            {
                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    var requestBody = reader.ReadToEnd();
                    var requestData = JsonConvert.DeserializeObject<RequestData>(requestBody);

                    Console.WriteLine("Gelen veri: " + requestData);

                    var userService = UserService.GetInstance();
                    bool success = userService.VerifyEmail(requestData.Email, requestData.VerificationCode);

                    var responseJson = JsonConvert.SerializeObject(new { success });

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    var responseBytes = Encoding.UTF8.GetBytes(responseJson);
                    context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
        }
    }
}
