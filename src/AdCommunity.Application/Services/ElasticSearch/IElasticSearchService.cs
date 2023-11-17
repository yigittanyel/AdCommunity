namespace AdCommunity.Application.Services.ElasticSearch;

public interface IElasticSearchService
{
    Task<bool> IndexExistsAsync(string indexName);
    Task CreateIndexAsync(string indexName, string mapping);
    Task<List<T>> SearchAsync<T>(string indexName, string fieldName, string value);
    Task SyncToElastic<T>(string indexName, Func<Task<List<T>>> getDataFunc);
    Task SyncSingleToElastic<T>(string indexName, T data);
}