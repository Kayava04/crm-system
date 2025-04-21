using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Languages;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Mappers
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<LanguageEntity, Language>().ConstructUsing(e =>
                Language.Create(e.Id, e.Name));
            
            CreateMap<Language, LanguageEntity>();
            CreateMap<Language, LanguageDto>();
            CreateMap<CreateLanguageDto, Language>();
        }
    }
}