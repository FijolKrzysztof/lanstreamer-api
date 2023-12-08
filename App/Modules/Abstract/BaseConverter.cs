using AutoMapper;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace lanstreamer_api.services.Abstract;

public abstract class BaseConverter : IBaseConverter
{
    private readonly IMapper _mapper;
    private object _sourceObject = null!;
    
    protected BaseConverter(IConfigurationProvider configuration)
    {
        _mapper = new Mapper(configuration);
    }
    
    public TDestination Convert<TDestination>(object obj)
    {
        return _mapper.Map<TDestination>(obj);
    }
    
    public IBaseConverter ChainConvert<TDestination>(object sourceObject)
    {
        _sourceObject = _mapper.Map<TDestination>(sourceObject);
        return this;
    }

    public IBaseConverter Convert<TDestination>()
    {
        _sourceObject = _mapper.Map<TDestination>(_sourceObject);
        return this;
    }

    public TDestination To<TDestination>()
    {
        return _mapper.Map<TDestination>(_sourceObject);
    }
}
