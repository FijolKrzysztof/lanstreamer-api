namespace lanstreamer_api.services.Abstract;

public interface IBaseConverter
{
    TDestination Convert<TDestination>(object obj);
    IBaseConverter ChainConvert<TDestination>(object sourceObject);
    IBaseConverter Convert<TDestination>();
    TDestination To<TDestination>();
}