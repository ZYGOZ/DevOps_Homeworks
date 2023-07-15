using Grpc.Core;

namespace mvc_app
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<Service1.Service1Impl>();
                endpoints.MapGrpcService<Service2.Service2Impl>();

                endpoints.MapGet("/{message}", async context =>
                {
                    var message = context.Request.RouteValues["message"] as string;
                    var response = await CallGRPCServices(message);
                    await context.Response.WriteAsync(response);
                });
            });
        }

        private async Task<string> CallGRPCServices(string message)
        {
            string response = "";

            var service1Channel = new Channel("grpc-service-1:50051", ChannelCredentials.Insecure);
            var service1Client = new Service1.Service1Client(service1Channel);
            var service1Reply = await service1Client.Method1Async(new Service1.Request1 { Message = message });
            response += service1Reply.Reply + "\n";

            var service2Channel = new Channel("grpc-service-2:50052", ChannelCredentials.Insecure);
            var service2Client = new Service2.Service2Client(service2Channel);
            var service2Reply = await service2Client.Method2Async(new Service2.Request2 { Message = message });
            response += service2Reply.Reply + "\n";

            if (string.IsNullOrEmpty(response))
            {
                response = "No response received from gRPC services.";
            }

            return response;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.ConfigureServices((_, services) => services.AddGrpc());
            builder.Configure(app =>
            {
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGrpcService<Service1.Service1Impl>();
                    endpoints.MapGrpcService<Service2.Service2Impl>();

                    endpoints.MapGet("/{message}", async context =>
                    {
                        var message = context.Request.RouteValues["message"] as string;
                        var response = await CallGRPCServices(message);
                        await context.Response.WriteAsync(response);
                    });
                });
            });

            var app = builder.Build();
            app.Run();
        }

        private static async Task<string> CallGRPCServices(string message)
        {
            string response = "";

            var service1Channel = new Channel("grpc-service-1:50051", ChannelCredentials.Insecure);
            var service1Client = new Service1.Service1Client(service1Channel);
            var service1Reply = await service1Client.Method1Async(new Service1.Request1 { Message = message });
            response += service1Reply.Reply + "\n";

            var service2Channel = new Channel("grpc-service-2:50052", ChannelCredentials.Insecure);
            var service2Client = new Service2.Service2Client(service2Channel);
            var service2Reply = await service2Client.Method2Async(new Service2.Request2 { Message = message });
            response += service2Reply.Reply + "\n";

            if (string.IsNullOrEmpty(response))
            {
                response = "No response received from gRPC services.";
            }

            return response;
        }
    }
}