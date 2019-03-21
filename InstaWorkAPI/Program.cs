using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;

namespace InstaWorkAPI
{
    internal class Program
    {
        private static string SessionLog { get; set; }
        private static string SessionPwd { get; set; }

        public static void Main(string[] args)
        {
            SessionLog = "LOG";
            SessionPwd = "PWD";

            string login = "User1";
            string pwd = "qwerty123";
            string mail = "User@gmail.com";
            string proxy = "127.0.0.1:8080";
            string name = "Yankee";

            Console.WriteLine("Start Work..");
            Worker(login, pwd, mail, proxy, name).Wait();
        }

        public static async Task Worker(string login, string password, string mail, string proxy, string name)
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                Proxy = new WebProxy(proxy),
                UseProxy = true
            };

            var userSession = new UserSessionData
            {
                UserName = SessionLog,
                Password = SessionPwd
            };

            HttpClient cl = new HttpClient(handler);


            cl.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Linux; U; Android 2.2) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1");

            IInstaApi instaApi = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .UseHttpClientHandler(handler)
                .UseHttpClient(cl)
                .Build();


            try
            {
                var nd = await instaApi.CreateNewAccount(login, password, mail,
                    name);


                switch (nd.Info.Message)
                {
                    case "feedback_required":
                        Console.WriteLine("Ban account!");
                        break;
                    case "No errors detected":
                        CreationResponse ni1 = nd.Value;
                        Console.WriteLine("Created: " + ni1.AccountCreated);
                        break;
                    default:
                        Console.WriteLine(nd.Info.Message);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}