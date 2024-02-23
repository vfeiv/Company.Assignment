using Company.Assignment.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Company.Assignment.Endpoints
{
    public static class AggregationApi
    {
        public static RouteHandlerBuilder MapAggregatedDataEndpoints(this IEndpointRouteBuilder routes, WebApplication app)
        {
            var route = routes.MapGet("/aggregated-data", AggregatedDataHandler.Get)
                .WithName(nameof(AggregationApi))
                .Produces<AggregatedData>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
                .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
                .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized, MediaTypeNames.Application.Json)
                .Produces<ProblemDetails>(StatusCodes.Status403Forbidden, MediaTypeNames.Application.Json)
                .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json)
                .WithOpenApi(operation => new(operation) 
                {
                    Description = "Fetches data from external APIs and returns the aggregated data.",
                    Summary = "Gets aggregated data",
                });

            if(!app.Environment.IsDevelopment())
            {
                route.RequireAuthorization();
            }

            return route;
        }
    }
}
