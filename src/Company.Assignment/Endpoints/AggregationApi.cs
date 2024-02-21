namespace Company.Assignment.Endpoints
{
    public static class AggregationApi
    {
        public static RouteHandlerBuilder MapAggregatedDataEndpoints(this IEndpointRouteBuilder routes)
        {
            return routes.MapGet("/aggregated-data", AggregatedDataHandler.Get)
            .WithName("GetAggregatedData")
            .WithOpenApi();
        }
    }
}
