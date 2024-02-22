namespace Company.Assignment.Core.ExternalApiClients.Models.News;

internal readonly record struct TopHeadLinesRespone
{
    public List<ArticleResponse> Articles { get; init; }
}

public readonly record struct ArticleResponse
{
    public Source Source { get; init; }
    public string Author { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string Url { get; init; }
    public string UrlToImage { get; init; }
    public string PublishedAt { get; init; }
    public string Content { get; init; }
}

public readonly record struct Source
{
    public string Id { get; init; }
    public string Name { get; init; }
}
