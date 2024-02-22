using AutoFixture.Xunit2;
using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.ExternalApiClients.Models.News;
using Company.Assignment.Core.Mappers;
using FluentAssertions;
using Xunit;

namespace Company.Assignment.Core.Tests.Mappers;

public class ArticleResponseToArticleDtoTests
{
    private readonly ArticleResponseToArticleDto _mapper = new();

    [Theory, AutoData]
    public void ShouldMapArticleDto_WhenFromArticleResponseIsNotNull(
        string sourceId,
        string sourceName,
        string author,
        string title,
        string description,
        string url,
        string urlToImage,
        string publishedAt,
        string content)
    {
        var from = new ArticleResponse
        {
            Source = new Core.ExternalApiClients.Models.News.Source
            {
                Id = sourceId,
                Name = sourceName
            },
            Author = author,
            Title = title,
            Description = description,
            Url = url,
            UrlToImage = urlToImage,
            PublishedAt = publishedAt,
            Content = content
        };

        var to = _mapper.Map(from);

        to.Source.Id.Should().Be(from.Source.Id);
        to.Source.Name.Should().Be(from.Source.Name);
        to.Author.Should().Be(from.Author);
        to.Title.Should().Be(from.Title);
        to.Description.Should().Be(from.Description);
        to.Url.Should().Be(from.Url);
        to.UrlToImage.Should().Be(from.UrlToImage);
        to.PublishedAt.Should().Be(from.PublishedAt);
        to.Content.Should().Be(from.Content);
    }

    [Fact]
    public void ShouldReturnDefault_WhenFromIsDefault()
    {
        var to = _mapper.Map(default(ArticleResponse));

        to.Should().Be(default(ArticleDto));
    }
}
