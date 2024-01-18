using Nest;

namespace AdCommunity.Application.Services.ElasticSearch;
public interface IElasticSearchService<T>  where T : class
{
    Task<string> CreateDocumentAsync(T document);
    Task<T> GetDocumentAsync(int id);
    Task<IEnumerable<T>> GetDocumentsAsync();
    Task<string> UpdateDocumentAsync(T document);   
    Task<string> DeleteDocumentAsync(int id);
}