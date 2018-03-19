using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SignalR.Chat.EndPoints;
using SignalR.Chat.Services;

namespace SignalR.Chat
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<Hubs.Chat>("/chat");
            });

            app.UseSockets(o =>
            {
                o.MapEndPoint<StockEndPoint>("/stock");
            });

            app.UseFileServer();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<PersistentHubConnectionManager, PersistentHubConnectionManager>();

            services.AddSingleton<PersistentConnectionManager, PersistentConnectionManager>();

            services.AddMvc();

            services.AddSockets();
            services.AddEndPoint<StockEndPoint>();

            services.AddSignalR();
        }
    }
}