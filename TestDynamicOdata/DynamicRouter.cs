
namespace TestDynamicOdata
{


    public static class AddRouteExtensions
    {


        public static void AddRoute(this Microsoft.AspNetCore.Builder.IApplicationBuilder app, string pattern, Microsoft.AspNetCore.Http.RequestDelegate handler)
        {
            // DynamicRouter dynamicRouter = app.Services.GetRequiredService<DynamicRouter>();

            DynamicRouter dynamicRouter = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions
                .GetRequiredService<DynamicRouter>(app.ApplicationServices);

            dynamicRouter.AddRoute(pattern, handler);
        } // End Sub AddRoute 

    }


    public class DynamicRouter
    {

        private readonly System.Collections.Generic.Dictionary<string, Microsoft.AspNetCore.Http.RequestDelegate> m_routes;
            


        public DynamicRouter()
        {
            this.m_routes = new System.Collections.Generic.Dictionary<string, Microsoft.AspNetCore.Http.RequestDelegate>(System.StringComparer.InvariantCultureIgnoreCase);
        }


        public void AddRoute(string pattern, Microsoft.AspNetCore.Http.RequestDelegate handler)
        {
            this.m_routes[pattern] = handler;
        } // End Sub AddRoute 


        public bool TryGetHandler(Microsoft.AspNetCore.Http.HttpContext context, out Microsoft.AspNetCore.Http.RequestDelegate? handler)
        {
            if (this.m_routes.ContainsKey(context.Request.Path))
            {
                handler = this.m_routes[context.Request.Path];
                return true;
            }

            handler = null;
            return false;
        } // End Function TryGetHandler 


    } // End Class DynamicRouter 


    public class DynamicRouteMiddleware
    {
        private readonly Microsoft.AspNetCore.Http.RequestDelegate m_next;
        private readonly DynamicRouter m_dynamicRouter;


        public DynamicRouteMiddleware(
            Microsoft.AspNetCore.Http.RequestDelegate next, 
            DynamicRouter dynamicRouter
        )
        {
            this.m_next = next;
            this.m_dynamicRouter = dynamicRouter;
        } // End Constructor 


        public async System.Threading.Tasks.Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext context)
        {
            Microsoft.AspNetCore.Http.RequestDelegate? handler;

            if (this.m_dynamicRouter.TryGetHandler(context,  out handler))
            {
                await handler!(context);
            }
            else
            {
                await this.m_next(context);
            }

        } // End Task InvokeAsync 


    } // End Class DynamicRouter 


} // End Namespace 
