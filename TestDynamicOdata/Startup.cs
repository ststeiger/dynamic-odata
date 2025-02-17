﻿
namespace TestDynamicOdata
{

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;


    public class Startup
    {

        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }


        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Configuration = configuration;
        } // End Constructor 


        public void ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            services.AddSingleton<System.TimeProvider>(System.TimeProvider.System);
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            services.AddSingleton<DynamicRouter>();
        }


        public void Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder app)
        {
            Microsoft.AspNetCore.Hosting.IWebHostEnvironment env = app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
            Configure(app, env);
        }


        public void Configure(
            Microsoft.AspNetCore.Builder.IApplicationBuilder app, 
            Microsoft.AspNetCore.Hosting.IWebHostEnvironment env
        )
        {
            string virtualDirectory = "/Virt_DIR";
            virtualDirectory = "/";

            if (virtualDirectory.EndsWith("/"))
                virtualDirectory = virtualDirectory.Substring(0, virtualDirectory.Length - 1);

            if (string.IsNullOrWhiteSpace(virtualDirectory))
                ConfigureMapped(app, env); // Don't map if you don't have to 
            // (wonder what the framework does or does not  do for that case)
            else
                app.Map(virtualDirectory,
                    delegate (Microsoft.AspNetCore.Builder.IApplicationBuilder mappedApp) { ConfigureMapped(mappedApp, env); }
                );
        } // End Sub Configure 


        public void ConfigureMapped(
            Microsoft.AspNetCore.Builder.IApplicationBuilder app,
            Microsoft.AspNetCore.Hosting.IWebHostEnvironment env
        )
        {
            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            app.UseHttpsRedirection();


            app.UseDefaultFiles(new Microsoft.AspNetCore.Builder.DefaultFilesOptions()
            {
                DefaultFileNames = new System.Collections.Generic.List<string>()
                {
                    "index.htm", "index.html", "slick.htm"
                }
            });



            app.UseStaticFiles(
                new Microsoft.AspNetCore.Builder.StaticFileOptions()
                {
                    ServeUnknownFileTypes = true,
                    DefaultContentType = "application/octet-stream",
                    ContentTypeProvider = new ExtensionContentTypeProvider(),

                    OnPrepareResponse = delegate (Microsoft.AspNetCore.StaticFiles.StaticFileResponseContext context)
                    {
                        // https://stackoverflow.com/questions/49547/how-do-we-control-web-page-caching-across-all-browsers

                        // The Cache-Control is per the HTTP 1.1 spec for clients and proxies
                        // If you don't care about IE6, then you could omit Cache-Control: no-cache.
                        // (some browsers observe no-store and some observe must-revalidate)
                        context.Context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate, max-age=0";
                        // Other Cache-Control parameters such as max-age are irrelevant 
                        // if the abovementioned Cache-Control parameters (no-cache,no-store,must-revalidate) are specified.


                        // Expires is per the HTTP 1.0 and 1.1 specs for clients and proxies. 
                        // In HTTP 1.1, the Cache-Control takes precedence over Expires, so it's after all for HTTP 1.0 proxies only.
                        // If you don't care about HTTP 1.0 proxies, then you could omit Expires.
                        context.Context.Response.Headers["Expires"] = "-1, 0, Tue, 01 Jan 1980 1:00:00 GMT";

                        // The Pragma is per the HTTP 1.0 spec for prehistoric clients, such as Java WebClient
                        // If you don't care about IE6 nor HTTP 1.0 clients 
                        // (HTTP 1.1 was introduced 1997), then you could omit Pragma.
                        context.Context.Response.Headers["pragma"] = "no-cache";


                        // On the other hand, if the server auto-includes a valid Date header, 
                        // then you could theoretically omit Cache-Control too and rely on Expires only.

                        // Date: Wed, 24 Aug 2016 18:32:02 GMT
                        // Expires: 0

                        // But that may fail if e.g. the end-user manipulates the operating system date 
                        // and the client software is relying on it.
                        // https://stackoverflow.com/questions/21120882/the-date-time-format-used-in-http-headers
                    } // End Sub OnPrepareResponse 

                }
            );


            // app.UseAuthentication();
            app.UseRouting();
            app.UseMiddleware<DynamicRouteMiddleware>();


            // Use the older style to define routes and endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/hello", () => "Hellou Wörld!");

                endpoints.MapGet("/cia", async context =>
                {
                    await context.Response.WriteAsync("Ciao!");
                });

                endpoints.MapGet("/pax", async delegate (Microsoft.AspNetCore.Http.HttpContext context)
                {
                    // return "Pax americana";
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync("Peace !");
                });

                endpoints.MapControllers();
            });

        } // End Sub ConfigureMapped 


    } // End Class Startup 


} // End Namespace 
