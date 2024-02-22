using AutoMapper;

namespace EmployeeDAA.Api.Extensions
{
    public static class MappingExtensions
    {
        private static IMapper _mapper;

        public static void InitializeMapper(this IMapper mapper)
        {
            _mapper = mapper;
        }
        public static TDestination MapTo<TDestination>(this object source)
        {
            return _mapper.Map<TDestination>(source);
        }

    }
}
