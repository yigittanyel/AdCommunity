using Nest;

namespace AdCommunity.Application.Services.ElasticSearch;
public interface IElasticSearchService<T>  where T : class
{
    Task CheckIndex(string indexName, Action<CreateIndexDescriptor> customMappingFunc = null);
    Task InsertDocument(string indexName, T document);
    Task DeleteIndex(string indexName);
    Task DeleteByIdDocument(string indexName, T document);
    Task InsertBulkDocuments(string indexName, List<T> documents);
    Task<T> GetDocument(string indexName, string id);
    Task<List<T>> GetDocuments(string indexName);
}