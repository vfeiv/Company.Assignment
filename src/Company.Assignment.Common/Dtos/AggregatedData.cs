namespace Company.Assignment.Common.Dtos;

public readonly record struct AggregatedData
{
    public AggregatedData() {}

    public string Test { get; init; } = "This is a test";
}
