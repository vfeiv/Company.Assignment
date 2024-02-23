namespace Company.Assignment.Common.Dtos;

public readonly record struct ArticleDto
{
    public Source Source { get; init; }

    /// <summary>
    /// The author of the article
    /// </summary>
    public string Author { get; init; }

    /// <summary>
    /// The headline or title of the article.
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// A description or snippet from the article.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// The direct URL to the article.
    /// </summary>
    public string Url { get; init; }

    /// <summary>
    /// The URL to a relevant image for the article.
    /// </summary>
    public string UrlToImage { get; init; }

    /// <summary>
    /// The date and time that the article was published, in UTC (+000)
    /// </summary>
    public string PublishedAt { get; init; }

    /// <summary>
    /// The unformatted content of the article, where available. This is truncated to 200 chars.
    /// </summary>
    public string Content { get; init; }
}

public readonly record struct Source
{
    /// <summary>
    /// The identifier id or the source this article came from.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// The display name for the source this article came from.
    /// </summary>
    public string Name { get; init; }
}
