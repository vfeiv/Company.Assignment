using Company.Assignment.Common.Dtos;
using Company.Assignment.Common.Filters;

namespace Company.Assignment.Common.Abstractions.Services;

public interface IAggregateService
{
    Task<AggregatedData> GetAggregateData(AggregateFilter aggregateFilter, CancellationToken cancellationToken = default);
}
