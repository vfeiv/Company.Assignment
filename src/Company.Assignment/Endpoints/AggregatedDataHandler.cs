using Company.Assignment.Common.Abstractions.Services;
using Company.Assignment.Common.Dtos;
using Company.Assignment.Common.Filters;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Company.Assignment.Endpoints
{
    internal static class AggregatedDataHandler
    {
        public static async Task<Results<Ok<AggregatedData>, NotFound>> Get(IAggregateService service, [AsParameters] AggregateFilter aggregateFilter)
        {
            var result = await service.GetAggregateData(aggregateFilter);
            if (result == default)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(result);
        }
    }
}
