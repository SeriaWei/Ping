using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;
using System.Net;

namespace Ping
{
    //dotnet pack -o ./
    [Command(Description = "Determine whether a remote web server is accessible over the network.")]
    class Program
    {
        static int Main(string[] args)
        {
            return CommandLineApplication.Execute<Program>(args);
        }

        [Option(Description = "Host name with schema, http://www.zkea.net")]
        public string Host { get; set; }
        [Option(Description = "Timeout")]
        public int Timeout { get; set; }
        private int OnExecute()
        {
            return Ping() ? 0 : 1;
        }
        private bool Ping()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                if (string.IsNullOrEmpty(Host)) return false;

                Console.WriteLine("Ping:{0}", Host);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Host);
                request.Timeout = Timeout == 0 ? 10000 : Timeout;
                request.AllowAutoRedirect = false;
                request.Method = "HEAD";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    Console.WriteLine(response.StatusCode);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                Console.WriteLine("Elapsed: {0}", stopwatch.Elapsed);
            }
        }
    }
}
