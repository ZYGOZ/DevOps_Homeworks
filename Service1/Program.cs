using Microsoft.AspNetCore.Hosting.Server;
using Grpc.Core;
using System.Threading.Tasks;

namespace Service1
{
    public class Service1Impl : Service1.Service1Base
    {
        public override Task<Response1> Method1(Request1 request, ServerCallContext context)
        {
            string message = request.Message;
            string reply = "Hello from Service 1: " + message;
            return Task.FromResult(new Response1 { Reply = reply });
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            const int Port = 50051;

            Server server = new Server
            {
                Services = { Service1.BindService(new Service1Impl()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };

            server.Start();

            System.Console.WriteLine("Service 1 is running...");
            System.Console.WriteLine("Press any key to stop the server...");
            System.Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}