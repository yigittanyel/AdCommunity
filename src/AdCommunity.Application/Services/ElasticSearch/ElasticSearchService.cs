using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace AdCommunity.Application.Services.ElasticSearch
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly ElasticLowLevelClient _client;
        private readonly string _elasticSearchUrl;

        public ElasticSearchService(IConfiguration configuration)
        {
            _elasticSearchUrl = configuration.GetSection("ElasticSearch:Url").Value;
            ValidateConfiguration();

            var settings = new ConnectionConfiguration(new Uri(_elasticSearchUrl));
            _client = new ElasticLowLevelClient(settings);
        }

        public async Task<bool> IndexExistsAsync(string indexName)
        {
            var response = await _client.Indices.ExistsAsync<StringResponse>(indexName);
            return response.Success;
        }

        public async Task CreateIndexAsync(string indexName, string mapping)
        {
            await _client.Indices.CreateAsync<StringResponse>(indexName, PostData.Serializable(mapping));
        }

        public async Task<List<T>> SearchAsync<T>(string indexName, string fieldName, string value)
        {
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

            var response = await _client.SearchAsync<StringResponse>(indexName, PostData.Serializable(searchQuery));

            var results = JObject.Parse(response.Body);

            var hits = results["hits"]["hits"].ToObject<List<JObject>>();

            List<T> items = new();

            foreach (var hit in hits)
            {
                items.Add(hit["_source"].ToObject<T>());
            }

            return items;
        }

        public async Task SyncToElastic<T>(string indexName, Func<Task<List<T>>> getDataFunc)
        {
            List<T> items = await getDataFunc();

            if (items == null)
            {
                return;
            }

            var tasks = new List<Task>();

            foreach (var item in items)
            {
                var itemId = item?.GetType().GetProperty("Id")?.GetValue(item)?.ToString();

                if (string.IsNullOrEmpty(itemId))
                {
                    continue;
                }

                var response = await _client.GetAsync<StringResponse>(indexName, itemId);

                if (response.HttpStatusCode != 200)
                {
                    tasks.Add(_client.IndexAsync<StringResponse>(indexName, itemId, PostData.Serializable(item)));
                }
            }

            await Task.WhenAll(tasks);
        }

        public async Task SyncSingleToElastic<T>(string indexName, T data)
        {

            var dataId = data?.GetType().GetProperty("Id")?.GetValue(data)?.ToString();

            if (string.IsNullOrEmpty(dataId))
            {
                return;
            }

            var response = await _client.CreateAsync<StringResponse>(indexName, dataId, PostData.Serializable(data));

            if (response.HttpStatusCode == 201)
            {
                response = await _client.IndexAsync<StringResponse>(indexName, dataId, PostData.Serializable(data));

                if (response.HttpStatusCode != 200)
                {
                    throw new Exception($"Failed to index document. HttpStatusCode: {response.HttpStatusCode}");
                }
            }
            else
            {
                throw new Exception($"Failed to create document. HttpStatusCode: {response.HttpStatusCode}");
            }
        }

        private void ValidateConfiguration()
        {
            if (string.IsNullOrEmpty(_elasticSearchUrl))
            {
                throw new InvalidOperationException("ElasticSearchUrl is not configured.");
            }
        }
    }
}