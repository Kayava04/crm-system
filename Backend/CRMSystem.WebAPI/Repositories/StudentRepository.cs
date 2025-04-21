using AutoMapper;
using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.DTOs.School.Students;
using CRMSystem.WebAPI.Entities.School;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Repositories
{
    public class StudentRepository(SchoolDbContext context, IMapper mapper)
        : IBasicRepository<Student>
    {
        public async Task<Student?> GetByIdAsync(Guid id)
        {
            var entity = await context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            
            return mapper.Map<Student>(entity);
        }

        public async Task<IEnumerable<Student>> GetAllAsync(IFilterDto? filter = null)
        {
            var query = context.Students
                .Include(s => s.Person)
                    .ThenInclude(p => p.Contacts)
                        .AsNoTracking();

            var studentFilter = filter as StudentFilterDto;

            if (studentFilter != null)
            {
                if (!string.IsNullOrWhiteSpace(studentFilter.FullName))
                    query = query.Where(s => s.Person.FullName
                        .Contains(studentFilter.FullName));

                if (studentFilter.BirthDate.HasValue)
                    query = query.Where(s => s.Person.BirthDate.Date == studentFilter.BirthDate.Value.Date);

                if (!string.IsNullOrWhiteSpace(studentFilter.Phone))
                    query = query.Where(s => s.Person.Contacts
                        .Any(c => c.Phone == studentFilter.Phone));

                if (!string.IsNullOrWhiteSpace(studentFilter.Email))
                    query = query.Where(s => s.Person.Contacts
                        .Any(c => c.Email == studentFilter.Email));

                if (studentFilter.Grade.HasValue)
                    query = query.Where(s => s.Grade == studentFilter.Grade);

                if (!string.IsNullOrWhiteSpace(studentFilter.ParentFullName))
                {
                    query = query.Where(s =>
                        s.ParentId != null && context.Persons
                            .Any(p => p.Id == s.ParentId && p.FullName
                                .Contains(studentFilter.ParentFullName)));
                }

                if (!string.IsNullOrWhiteSpace(studentFilter.LanguageName))
                    query = query.Where(s => s.StudentGroups
                        .Any(sg => sg.Group.Language.Name.Contains(studentFilter.LanguageName)));
                
                if (!string.IsNullOrWhiteSpace(studentFilter.Level))
                    query = query.Where(s => s.StudentGroups
                        .Any(sg => sg.Level.Contains(studentFilter.Level)));
                
                if (!string.IsNullOrWhiteSpace(studentFilter.GroupName))
                    query = query.Where(s => s.StudentGroups
                        .Any(sg => sg.Group.GroupName.Contains(studentFilter.GroupName)));
                
                if (!string.IsNullOrWhiteSpace(studentFilter.LessonDay))
                    query = query.Where(s => s.StudentGroups
                        .Any(sg => sg.Group.GroupLessonDays
                            .Any(gld => gld.LessonDay.DayOfWeek.Contains(studentFilter.LessonDay))));
                
                if (studentFilter.PairNumber.HasValue)
                    query = query.Where(s => s.StudentGroups
                        .Any(sg => sg.PairNumber == studentFilter.PairNumber));
                
                if (!string.IsNullOrWhiteSpace(studentFilter.ContractNumber))
                    query = query.Where(s => s.Contracts
                        .Any(c => c.ContractNumber.Contains(studentFilter.ContractNumber)));
                
                if (studentFilter.IsPaid.HasValue)
                    query = query.Where(s => s.IsPaid == studentFilter.IsPaid.Value);
            }

            return mapper.Map<IEnumerable<Student>>(await query.ToListAsync());
        }

        public async Task<Student> AddAsync(Student student)
        {
            var entity = mapper.Map<StudentEntity>(student);
            await context.Students.AddAsync(entity);
            
            await context.SaveChangesAsync();
            return mapper.Map<Student>(entity);
        }

        public async Task<Student?> UpdateAsync(Guid id, Student student)
        {
            var existing = await context.Students
                .FirstOrDefaultAsync(s => s.Id == id);
            
            if (existing == null) return null;

            context.Entry(existing)
                .CurrentValues.SetValues(mapper.Map<StudentEntity>(student));
            
            await context.SaveChangesAsync();
            return mapper.Map<Student>(existing);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await context.Students
                .FirstOrDefaultAsync(s => s.Id == id);
            
            if (entity != null)
            {
                context.Students.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}