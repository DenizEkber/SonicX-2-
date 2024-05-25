/*using SonicXBackendC.Server.Email;
using SonicXBackendC.Server.PlayList;
using SonicXBackendC.Server.User;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SonicXBackendC.Server
{
    public class CSharpHttpServer
    {
        public static void Main(string[] args)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8002/");
            listener.Start();

            Console.WriteLine("C# HTTP server is running: http://localhost:8002");

            while (true)
            {
                var context = listener.GetContext();
                Task.Run(() => HandleRequestAsync(context));
            }
        }

        private static async Task HandleRequestAsync(HttpListenerContext context)
        {
            var path = context.Request.Url.AbsolutePath;

            switch (path)
            {
                case "/login":
                    new LoginHandler().Handle(context);
                    break;
                case "/register":
                    new RegisterHandler().Handle(context);
                    break;
                case "/verify-email":
                    new VerifyEmailHandler().Handle(context);
                    break;
                case "/logout":
                    new LogoutHandler().Handle(context);
                    break;
                case "/profile":
                    new ProfileHandler().Handle(context);
                    break;
                case "/playlist-create":
                    new PlaylistCreateHandler().Handle(context);
                    break;
                case "/playlist-show":
                    new PlaylistShowHandler().Handle(context);
                    break;
                case "/playlist-showContent":
                    new PlaylistShowContent().Handle(context);
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
            }

            context.Response.Close();
        }
    }
}*/
