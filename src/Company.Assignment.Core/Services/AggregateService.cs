using Company.Assignment.Common.Abstractions.Services;
using Company.Assignment.Common.Dtos;

namespace Company.Assignment.Core.Services;

internal class AggregateService : IAggregateService
{
    public Task<AggregatedData> GetAggregateData(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new AggregatedData());
    }
}
