using Nest;

namespace AdCommunity.Application.Services.ElasticSearch.Mappings;

public static class Mappings
{
    public static CreateIndexDescriptor CommunityMapping(this CreateIndexDescriptor descriptor)
    {
        return descriptor.Map<AdCommunity.Domain.Entities.Aggregates.Community.Community>(m => m.Properties(p => p
                      .Keyword(k => k.Name(n => n.Id))
                      .Text(t => t.Name(n => n.Name))
                      .Text(t => t.Name(n => n.Description))
                      .Text(t => t.Name(n => n.Location))
                      .Date(t => t.Name(n => n.CreatedOn))));
        }
}
