
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.ModelBuilder;

namespace TestDynamicOdata
{

    // using Microsoft.AspNetCore.Builder;
    // using Microsoft.AspNetCore.Http; // for WriteAsync 


    // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-8.0
    // https://christianmartinezfinancialfox.medium.com/how-to-create-gpts-in-chat-gpt-ai-agents-the-ultimate-guide-fb78a5b5dbd6
    // https://www.pluralsight.com/resources/blog/software-development/how-make-chatgpt-plugin
    // FICO, Fair Isaac Corporation, FICO® credit scoring models that many lenders use to help accurately predict a consumer's ability to repay a debt on time. 
    // sap pm, sap fi, sap fx (Financial Accounting (FI), Plant Maintenance (PM), Foreign Exchange (FX)  RE-FX (SAP Flexible Real Estate Management) ) 
    public class Program
    {


        public static async System.Threading.Tasks.Task<int> Main(string[] args)
        {
            //Microsoft.AspNetCore.Builder.WebApplicationBuilder builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(
            //    new Microsoft.AspNetCore.Builder.WebApplicationOptions
            //{
            //    Args = args, 
            //    ApplicationName = typeof(Program).Assembly.FullName, 
            // //    ApplicationName = typeof(Program).Assembly.GetName().Name, 
            //    ContentRootPath = System.IO.Directory.GetCurrentDirectory(), 
            //    EnvironmentName = Microsoft.Extensions.Hosting.Environments.Staging, 
            //    WebRootPath = "customwwwroot" 
            //});



            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntityType<Order>();
            modelBuilder.EntitySet<Customer>("Customers");





            Microsoft.AspNetCore.Builder.WebApplicationBuilder builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
        "odata",
        modelBuilder.GetEdmModel()));


            // System.Console.WriteLine(builder.Environment.ApplicationName);
            // string message = builder.Configuration["HelloKey"] ?? "Hello";
            // builder.Logging.AddJsonConsole();
            // builder.Services.AddMemoryCache();

            // builder.WebHost.UseHttpSys();
            // builder.WebHost.ConfigureKestrel(options => { });


            Startup startupInstance = new Startup(builder.Configuration);
            startupInstance.ConfigureServices(builder.Services);

            // Add services to the container.
            // builder.Services.AddRazorPages();
            // builder.Services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();


            Microsoft.AspNetCore.Builder.WebApplication app = builder.Build();
            startupInstance.Configure(app, app.Environment);

            // dotnet run --urls="https://localhost:7777"
            // app.Urls.Add("http://localhost:3000");
            // app.Urls.Add("http://localhost:4000");

            // app.UseRouting();

            // app.UseAuthorization();

            // app.MapRazorPages();

            // await app.RunAsync();

            System.Threading.Tasks.Task runTask = app.RunAsync();
            Microsoft.Extensions.Logging.LoggerExtensions.LogInformation(app.Logger, "The app started");


            await TestAddRoutesWithDelay(app);

            await runTask;

            return 0;
        } // End Task Main 


        // Simulate adding a route dynamically after the app has started
        public static async System.Threading.Tasks.Task TestAddRoutesWithDelay(Microsoft.AspNetCore.Builder.WebApplication app)
        {
            await System.Threading.Tasks.Task.Delay(5000); // Wait for 5 seconds


            app.AddRoute("/dr1", async context =>
            {
                // await context.Response.WriteAsync("This is a dynamically added route 1.");
                await Microsoft.AspNetCore.Http.HttpResponseWritingExtensions.WriteAsync(context.Response, "This is a dynamically added route 1.");
            });

            await System.Threading.Tasks.Task.Delay(5000); // Wait for 5 seconds

            app.AddRoute("/dr2", async delegate (Microsoft.AspNetCore.Http.HttpContext context)
            {
                // await context.Response.WriteAsync("This is a dynamically added route 2.");
                await Microsoft.AspNetCore.Http.HttpResponseWritingExtensions.WriteAsync(context.Response, "This is a dynamically added route 2.");
            });

            // await System.Threading.Tasks.Task.Delay(5000); // Wait for 5 seconds

            app.AddRoute("/dr3", DynamicRouteHandler3);


            // app.AddRoute("/", () => "Hello World!");
        } // End Task TestAddRoutesWithDelay 


        // public delegate System.Threading.Tasks.Task RequestDelegate(Microsoft.AspNetCore.Http.HttpContext context);
        public static async System.Threading.Tasks.Task DynamicRouteHandler3(Microsoft.AspNetCore.Http.HttpContext context)
        {
            // await context.Response.WriteAsync("This is a dynamically added route 3.");
            await Microsoft.AspNetCore.Http.HttpResponseWritingExtensions.WriteAsync(context.Response, "This is a dynamically added route 3.");
        } // End Task DynamicRouteHandler3 


    } // End Class Program 


} // End Namespace 
