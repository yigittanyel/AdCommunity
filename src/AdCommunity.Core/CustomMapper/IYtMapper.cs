namespace AdCommunity.Core.CustomMapper;

public interface IYtMapper
{
    void CreateMap<TSource, TDestination>(); 
    TDestination Map<TSource, TDestination>(TSource source);
}