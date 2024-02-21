using Company.Assignment.Common.Abstractions.Services;
using Company.Assignment.Common.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Company.Assignment.Endpoints
{
    internal static class AggregatedDataHandler
    {
        public static async Task<Results<Ok<AggregatedData>, NotFound>> Get(IAggregateService service)
        {
            var result = await service.GetAggregateData();
            if (result == default)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(result);
        }
    }
}
