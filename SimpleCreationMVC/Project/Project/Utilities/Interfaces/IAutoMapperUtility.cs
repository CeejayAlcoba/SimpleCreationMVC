
namespace Utilities.Interfaces
{
    public interface IAutoMapperUtility
    {
        TDestination Map<TSource,TDestination>(TSource source);
        List<TDestination> MapList<TSource,TDestination>(IEnumerable<TSource> source);
    }
}
