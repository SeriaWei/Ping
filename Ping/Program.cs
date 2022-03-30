using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ping
{
    //dotnet pack -o ./
    [Command(Description = "Determine whether a remote web server is accessible over the network.")]
    class Program
    {
        const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3438.3 Safari/537.36";
        static Task<int> Main(string[] args)
        {
            return CommandLineApplication.ExecuteAsync<Program>(args);
        }

        [Option(Description = "Host name with schema, http://www.zkea.net")]
        public string Host { get; set; }
        [Option(Description = "Timeout seconds.")]
        public int Timeout { get; set; }
        private async Task<int> OnExecuteAsync()
        {
            bool success = await PingAsync();
            return success ? 0 : 1;
        }
        private async Task<bool> PingAsync()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                if (string.IsNullOrEmpty(Host)) return false;

                Console.WriteLine("Ping:{0}", Host);
                using HttpClient httpClient = new HttpClient();
                var requestMessage = new HttpRequestMessage(HttpMethod.Head, Host);
                httpClient.Timeout = new TimeSpan(0, 0, Timeout == 0 ? 10 : Timeout);
                var responseMessage = await httpClient.SendAsync(requestMessage);
                Console.WriteLine(responseMessage.StatusCode);
                return responseMessage.IsSuccessStatusCode;
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
