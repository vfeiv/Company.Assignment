using Company.Assignment.Common.Dtos;

namespace Company.Assignment.Common.Abstractions.Services;

public interface IAggregateService
{
    Task<AggregatedData> GetAggregateData(CancellationToken cancellationToken = default);
}
