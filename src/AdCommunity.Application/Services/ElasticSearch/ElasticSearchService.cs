using Microsoft.Extensions.Configuration;
using Nest;

namespace AdCommunity.Application.Services.ElasticSearch;

public class ElasticSearchService<T> : IElasticSearchService<T>
       where T : class
{
    private readonly IElasticClient _client;

    public ElasticSearchService(IConfiguration configuration)
    {
        _client = CreateInstance(configuration);
    }

    private ElasticClient CreateInstance(IConfiguration configuration)
    {
        string host = configuration.GetSection("ElasticsearchServer:Host").Value;
        string port = configuration.GetSection("ElasticsearchServer:Port").Value;
        string username = configuration.GetSection("ElasticsearchServer:Username").Value;
        string password = configuration.GetSection("ElasticsearchServer:Password").Value;
        var settings = new ConnectionSettings(new Uri(host + ":" + port));

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            settings.BasicAuthentication(username, password);

        return new ElasticClient(settings);
    }

    public async Task CheckIndex(string indexName, Action<CreateIndexDescriptor> customMappingFunc = null)
    {
        var indexExistsResponse = await _client.Indices.ExistsAsync(indexName);

        if (indexExistsResponse.Exists)
            return;

        var createIndexDescriptor = new CreateIndexDescriptor(indexName)
            .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1));

        customMappingFunc?.Invoke(createIndexDescriptor);

        var response = await _client.Indices.CreateAsync(createIndexDescriptor);

        return;
    }



    public async Task InsertDocument(string indexName, T document)
    {
        var indexResponse = await _client.IndexAsync(document, idx => idx.Index(indexName));

        if (!indexResponse.IsValid)
        {
            throw new Exception(indexResponse.OriginalException.Message);
        }
    }

    public async Task DeleteIndex(string indexName)
    {
        var deleteIndexResponse = await _client.Indices.DeleteAsync(indexName);

        if (!deleteIndexResponse.IsValid)
        {
            throw new Exception(deleteIndexResponse.OriginalException.Message);
        }
    }

    public async Task<T> GetDocument(string indexName, string id)
    {
        var getResponse = await _client.GetAsync<T>(new DocumentPath<T>(id).Index(indexName));

        if (!getResponse.IsValid)
        {
            throw new Exception(getResponse.OriginalException.Message);
        }

        return getResponse.Source;
    }

    public async Task<List<T>> GetDocuments(string indexName)
    {
        var searchResponse = await _client.SearchAsync<T>(s => s.Index(indexName).Size(1000));

        if (!searchResponse.IsValid)
        {
            throw new Exception(searchResponse.OriginalException.Message);
        }

        return searchResponse.Documents.ToList();
    }

    public async Task InsertBulkDocuments(string indexName, List<T> documents)
    {
        await _client.IndexManyAsync(documents, indexName);
    }

    public async Task DeleteByIdDocument(string indexName, T document)
    {
        var deleteResponse = await _client.DeleteAsync<T>(document, idx => idx.Index(indexName));

        if (!deleteResponse.IsValid)
        {
            throw new Exception(deleteResponse.OriginalException.Message);
        }
    }
}

