using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.ExternalApiClients.Models.News;

namespace Company.Assignment.Core.Mappers;

public class ArticleResponseToArticleDto : BaseMapper<ArticleResponse, ArticleDto>
{
    public override ArticleDto Map(ArticleResponse from) =>
        new()
        {
            Source = new Common.Dtos.Source
            {
                Id = from.Source.Id,
                Name = from.Source.Name,
            },
            Author = from.Author,
            Title = from.Title,
            Description = from.Description,
            Url = from.Url,
            UrlToImage = from.UrlToImage,
            PublishedAt = from.PublishedAt,
            Content = from.Content
        };
}
