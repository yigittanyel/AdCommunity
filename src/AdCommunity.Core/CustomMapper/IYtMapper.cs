namespace AdCommunity.Core.CustomMapper;

public interface IYtMapper
{
    TDestination Map<TSource, TDestination>(TSource source); 
    List<TDestination> MapList<TSource, TDestination>(List<TSource> sourceList);
    void Map<TSource, TDestination>(TSource source, TDestination destination);
}
