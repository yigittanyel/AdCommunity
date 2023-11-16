namespace AdCommunity.Application.Services.ElasticSearch;

public interface IElasticSearchService
{
    Task SyncToElastic<T>(string indexName, Func<Task<List<T>>> getDataFunc) where T : class;
    Task SyncSingleToElastic<T>(string indexName, T data) where T : class;
    Task<bool> IndexExistsAsync(string indexName);
    Task CreateIndexAsync(string indexName, string mapping);
    Task<bool> AddDocumentAsync<T>(string indexName, string documentId, T document) where T : class;
    Task<List<T>> SearchAsync<T>(string indexName, string fieldName, string value) where T : class;
}