using AutoMapper;
using LoginAPI.DTOs;

namespace LoginAPI.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // RegisterDto -> User dönüşümü
        CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Şifreyi manuel hash'leyeceğiz

        // User -> RegisterDto dönüşümü (isteğe bağlı)
        CreateMap<User, RegisterDto>();

        // LoginDto -> User dönüşümü
        CreateMap<LoginDto, User>();

        // User -> LoginDto dönüşümü (isteğe bağlı)
        CreateMap<User, LoginDto>();
    }
}
