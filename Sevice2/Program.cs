using Grpc.Core;
using Microsoft.AspNetCore.Hosting.Server;
using System.Threading.Tasks;

namespace Service2
{
    public class Service2Impl : Service2.Service2Base
    {
        public override Task<Response2> Method2(Request2 request, ServerCallContext context)
        {
            string message = request.Message;
            string reply = "Hello from Service 2: " + message;
            return Task.FromResult(new Response2 { Reply = reply });
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const int Port = 50052;

            Server server = new Server
            {
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Services.Add(Service2.BindService(new Service2Impl()));

            server.Start();

            System.Console.WriteLine("Service 2 is running...");
            System.Console.WriteLine("Press any key to stop the server...");
            System.Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
