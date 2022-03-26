using AutoMapper;
using TelegramBot.AutoMapper;

public static class MyMapper
{
    private static readonly Mapper _mapper;
    static MyMapper()
    {
        var config = new MapperConfiguration(x =>
        {
            x.AddProfile<DbProfile>();
        });
        _mapper = new Mapper(config);
    }

    public static TDestination Map<TSource, TDestination>(TSource source)
        => _mapper.Map<TSource, TDestination>(source);
}