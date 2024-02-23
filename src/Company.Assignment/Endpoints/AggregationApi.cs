namespace Company.Assignment.Endpoints
{
    public static class AggregationApi
    {
        public static RouteHandlerBuilder MapAggregatedDataEndpoints(this IEndpointRouteBuilder routes, WebApplication app)
        {
            var route = routes.MapGet("/aggregated-data", AggregatedDataHandler.Get)
                .WithName("GetAggregatedData")
                .WithOpenApi();

            if(!app.Environment.IsDevelopment())
            {
                route.RequireAuthorization();
            }

            return route;
        }
    }
}
