
using AutoMapper;
using Utilities.Interfaces;

namespace Utilities.Classes
{
    public class AutoMapperUtility : IAutoMapperUtility
    {
         public TDestination Map<TSource,TDestination>(TSource source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(typeof(TSource), typeof(TDestination));
            });
            var mapper = new Mapper(config);
            return mapper.Map<TSource,TDestination>(source);
        }

        public List<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source)
        {
            if (source == null || !source.Any())
            {
                return new List<TDestination>();
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(typeof(TSource), typeof(TDestination));
            });
            var mapper = new Mapper(config);

            return mapper.Map<IEnumerable<TSource>,List<TDestination>>(source);
        }
    }
}
