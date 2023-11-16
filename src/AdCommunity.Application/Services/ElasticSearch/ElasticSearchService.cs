using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace AdCommunity.Application.Services.ElasticSearch;

public class ElasticSearchService : IElasticSearchService
{
    private readonly string _elasticSearchUrl;

    public ElasticSearchService(IConfiguration configuration)
    {
        _elasticSearchUrl = configuration["ElasticSearch:Url"];
    }

    private ElasticLowLevelClient GetClient()
    {
        var settings = new ConnectionConfiguration(new Uri(_elasticSearchUrl));
        var client = new ElasticLowLevelClient(settings);
        return client;
    }

    public async Task SyncToElastic<T>(string indexName, Func<Task<List<T>>> getDataFunc) where T : class
    {
        var client = GetClient();

        List<T> items = await getDataFunc();

        var tasks = new List<Task>();

        foreach (var item in items)
        {
            var itemId = item.GetType().GetProperty("Id")?.GetValue(item).ToString();

            if (string.IsNullOrEmpty(itemId))
            {
                // Hata işlemesi eklenebilir
                continue;
            }

            var response = await client.GetAsync<StringResponse>(indexName, itemId);

            if (response.HttpStatusCode != 200)
            {
                tasks.Add(client.IndexAsync<StringResponse>(indexName, itemId, PostData.Serializable(item)));
            }
        }

        await Task.WhenAll(tasks);
    }

    public async Task SyncSingleToElastic<T>(string indexName, T data) where T : class
    {
        var client = GetClient();

        var dataId = data.GetType().GetProperty("Id")?.GetValue(data).ToString();

        if (string.IsNullOrEmpty(dataId))
        {
            throw new Exception("Id bulunamadı!");
        }

        var response = await client.GetAsync<StringResponse>(indexName, dataId);

        if (response.HttpStatusCode != 200)
        {
            await client.IndexAsync<StringResponse>(indexName, dataId, PostData.Serializable(data));
        }
    }

    public async Task<bool> IndexExistsAsync(string indexName)
    {
        var client = GetClient();
        var response = await client.Indices.ExistsAsync<StringResponse>(indexName);
        return response.Success;
    }

    public async Task CreateIndexAsync(string indexName, string mapping)
    {
        var client = GetClient();
        var createIndexResponse = await client.Indices.CreateAsync<StringResponse>(indexName, PostData.String(mapping));
        if (!createIndexResponse.Success)
        {
            throw new Exception($"Failed to create index {indexName}. Error: {createIndexResponse.OriginalException.Message}");
        }
    }

    public async Task<bool> AddDocumentAsync<T>(string indexName, string documentId, T document) where T : class
    {
        var client = GetClient();
        var indexResponse = await client.IndexAsync<StringResponse>(indexName, documentId, PostData.Serializable(document));
        return indexResponse.Success;
    }

    public async Task<List<T>> SearchAsync<T>(string indexName, string fieldName, string value) where T : class
    {
        var client = GetClient();

        var searchQuery = new
        {
            query = new
            {
                wildcard = new Dictionary<string, object>
                {
                    { fieldName, new { value = $"*{value}*" } }
                }
            }
        };

        var response = await client.SearchAsync<StringResponse>(indexName, PostData.Serializable(searchQuery));

        if (response.Success)
        {
            var results = JObject.Parse(response.Body);
            var hits = results["hits"]["hits"].ToObject<List<JObject>>();
            List<T> items = new();

            foreach (var hit in hits)
            {
                items.Add(hit["_source"].ToObject<T>());
            }

            return items;
        }
        else
        {
            throw new Exception($"Failed to execute search on index {indexName}. Error: {response.OriginalException.Message}");
        }
    }
}