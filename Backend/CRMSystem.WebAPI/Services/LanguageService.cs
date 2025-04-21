using AutoMapper;
using CRMSystem.WebAPI.DTOs.School.Languages;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;

namespace CRMSystem.WebAPI.Services
{
    public class LanguageService(IBasicRepository<Language> languageRepository, IMapper mapper)
    {
        public async Task<IEnumerable<LanguageDto>> GetAllAsync()
        {
            var languages = await languageRepository.GetAllAsync();
            
            return mapper.Map<IEnumerable<LanguageDto>>(languages);
        }

        public async Task<LanguageDto?> GetByIdAsync(Guid id)
        {
            var language = await languageRepository.GetByIdAsync(id);
            
            return language is null ? null : mapper.Map<LanguageDto>(language);
        }

        public async Task<LanguageDto> CreateAsync(CreateLanguageDto dto)
        {
            var language = Language.Create(Guid.NewGuid(), dto.Name);
            var created = await languageRepository.AddAsync(language);
            
            return mapper.Map<LanguageDto>(created);
        }

        public async Task<LanguageDto?> UpdateAsync(Guid id, CreateLanguageDto dto)
        {
            var language = Language.Create(id, dto.Name);
            var updated = await languageRepository.UpdateAsync(id, language);
            
            return updated is null ? null : mapper.Map<LanguageDto>(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await languageRepository.DeleteAsync(id);
        }
    }
}