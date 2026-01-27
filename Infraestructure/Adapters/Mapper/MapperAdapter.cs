using AutoMapper;
using Domain.Contracts.Adapter.Mapper;

namespace Adapters.Mapper;

public class MapperAdapter : IMapperAdapter
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TSource, TDestination>();
        });
        IMapper mapper = config.CreateMapper();
        return mapper.Map<TDestination>(source);
    }

    public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TSource, TDestination>();
        });
        IMapper mapper = config.CreateMapper();
        return mapper.Map<List<TDestination>>(source);
    }
}