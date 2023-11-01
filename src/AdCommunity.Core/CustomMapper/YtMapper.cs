namespace AdCommunity.Core.CustomMapper;

public class YtMapper : IYtMapper
{
    private Dictionary<(Type SourceType, Type DestinationType), Delegate> _mappings = new Dictionary<(Type SourceType, Type DestinationType), Delegate>();

    public void CreateMap<TSource, TDestination>()
    {
        Func<TSource, TDestination> mappingFunction = CreateMappingFunction<TSource, TDestination>();
        _mappings[(typeof(TSource), typeof(TDestination))] = mappingFunction;
    }

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        Delegate mappingFunction;
        if (_mappings.TryGetValue((typeof(TSource), typeof(TDestination)), out mappingFunction))
        {
            return ((Func<TSource, TDestination>)mappingFunction)(source);
        }

        throw new InvalidOperationException($"Mapping from {typeof(TSource)} to {typeof(TDestination)} is not configured.");
    }

    private Func<TSource, TDestination> CreateMappingFunction<TSource, TDestination>()
    {
        var sourceProperties = typeof(TSource).GetProperties();
        var destinationProperties = typeof(TDestination).GetProperties();

        return source =>
        {
            var destination = Activator.CreateInstance<TDestination>();

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties
                    .FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);

                if (destinationProperty != null)
                {
                    destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                }
            }

            return destination;
        };
    }
}