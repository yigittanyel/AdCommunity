using System.Reflection;

namespace AdCommunity.Core.CustomMapper;

public class YtMapper : IYtMapper
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        Type sourceType = typeof(TSource);
        Type destinationType = typeof(TDestination);

        if (sourceType == destinationType)
        {
            return (TDestination)(object)source;
        }

        PropertyInfo[] sourceProperties = sourceType.GetProperties();
        PropertyInfo[] destinationProperties = destinationType.GetProperties();

        TDestination destination = Activator.CreateInstance<TDestination>();

        foreach (PropertyInfo sourceProperty in sourceProperties)
        {
            PropertyInfo destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);

            if (destinationProperty != null)
            {
                destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
            }
        }

        return destination;
    }

    public void Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        if (source == null || destination == null)
        {
            throw new ArgumentNullException("source and destination must not be null");
        }

        var sourceProperties = typeof(TSource).GetProperties();
        var destinationProperties = typeof(TDestination).GetProperties();

        foreach (var sourceProperty in sourceProperties)
        {
            var destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name);

            if (destinationProperty != null && destinationProperty.PropertyType == sourceProperty.PropertyType)
            {
                var value = sourceProperty.GetValue(source);
                destinationProperty.SetValue(destination, value);
            }
        }
    }

    public List<TDestination> MapList<TSource, TDestination>(List<TSource> sourceList)
    {
        if (sourceList == null)
        {
            throw new ArgumentNullException(nameof(sourceList));
        }

        List<TDestination> destinationList = new List<TDestination>();

        foreach (TSource source in sourceList)
        {
            TDestination destinationItem = Map<TSource, TDestination>(source);
            destinationList.Add(destinationItem);
        }

        return destinationList;
    }
}