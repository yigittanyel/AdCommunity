using Microsoft.Extensions.Configuration;
using Nest;

namespace AdCommunity.Application.Services.ElasticSearch;

public class ElasticSearchService<T> : IElasticSearchService<T> where T : class
{
    private readonly IElasticClient _elasticClient;

    public ElasticSearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<string> CreateDocumentAsync(T document)
    {
        var response = await _elasticClient.IndexDocumentAsync(document);
        return response.IsValid ? "Document created successfully" : "Document creation failed";
    }

    public async Task<string> DeleteDocumentAsync(int id)
    {
        var response = await _elasticClient.DeleteAsync(new DocumentPath<T>(id));
        return response.IsValid ? "Document deleted successfully" : "Document deletion failed";
    }

    public async Task<T> GetDocumentAsync(int id)
    {
        var response= await _elasticClient.GetAsync(new DocumentPath<T>(id));
        return response.Source;
    }

    public async Task<IEnumerable<T>> GetDocumentsAsync()
    {
        var searchResponse = await _elasticClient.SearchAsync<T>(s => s
                                                            .MatchAll()
                                                            .Size(10000));
        return searchResponse.Documents;
    }

    public async Task<string> UpdateDocumentAsync(T document)
    {
        var response= await _elasticClient.UpdateAsync(new DocumentPath<T>(document), u => u
                                                            .Doc(document)
                                                            .RetryOnConflict(3));
        return response.IsValid ? "Document updated successfully" : "Document update failed";
    }
}
