using AutoMapper;
using CRMSystem.WebAPI.Interfaces;
using CRMSystem.WebAPI.Models;
using CRMSystem.WebAPI.DTOs.School.Students;

namespace CRMSystem.WebAPI.Services
{
    public class StudentRegistrationService(
        IBasicRepository<Person> personRepository,
        IBasicRepository<Student> studentRepository,
        IBasicRepository<Contract> contractRepository,
        IBasicRepository<Contact> contactRepository,
        IBasicRepository<StudentGroup> studentGroupRepository,
        IBasicRepository<Group> groupRepository,
        IBasicRepository<Language> languageRepository,
        IBasicRepository<LessonDay> lessonDayRepository,
        IGroupLessonDayRepository groupLessonDayRepository,
        IContractNumberGenerator contractNumberGenerator,
        IMapper mapper)
    {
        public async Task<RegisterStudentResultDto> CreateStudentAsync(RegisterStudentDto dto)
        {
            var language = Language.Create(Guid.NewGuid(), dto.LanguageName);
            var createdLanguage = await languageRepository.AddAsync(language);
            
            var group = Group.Create(Guid.NewGuid(), dto.GroupName, createdLanguage.Id);
            var createdGroup = await groupRepository.AddAsync(group);
            
            var lessonDays = new List<LessonDay>();

            var lessonDayNames = string.IsNullOrWhiteSpace(dto.LessonDays)
                ? []
                : dto.LessonDays.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            foreach (var dayOfWeek in lessonDayNames)
            {
                var lessonDay = LessonDay.Create(Guid.NewGuid(), dayOfWeek);
                var createdLessonDay = await lessonDayRepository.AddAsync(lessonDay);

                var groupLessonDay = GroupLessonDay.Create(createdGroup.Id, createdLessonDay.Id);
                await groupLessonDayRepository.AddAsync(groupLessonDay);

                lessonDays.Add(createdLessonDay);
            }

            Person? createdParent;
            Person createdPerson;
            
            if (dto.IsParentRegister)
            {
                var parent = Person.Create(Guid.NewGuid(), dto.ParentFullName, DateTime.MinValue);
                createdParent = await personRepository.AddAsync(parent);

                var person = Person.Create(Guid.NewGuid(), dto.FullName, dto.BirthDate);
                createdPerson = await personRepository.AddAsync(person);
            }
            else
            {
                var person = Person.Create(Guid.NewGuid(), dto.FullName, dto.BirthDate);
                createdPerson = await personRepository.AddAsync(person);
                createdParent = null;
            }
            
            var parentId = createdParent?.Id;
            var grade = dto?.Grade.HasValue == true && dto.Grade > 0 ? dto.Grade : null;
            
            var student = Student.Create(Guid.NewGuid(), createdPerson.Id, parentId, grade, dto.IsPaid);
            var createdStudent = await studentRepository.AddAsync(student);
            
            var contractNumber = await contractNumberGenerator.Generate(dto.SignDate);
            var contract = Contract.Create(Guid.NewGuid(), contractNumber, dto.SignDate, dto.PaymentAmount, createdStudent.Id);
            var createdContract = await contractRepository.AddAsync(contract);
            
            var contact = Contact.Create(Guid.NewGuid(), createdPerson.Id, dto.Phone, dto.Email);
            var createdContact = await contactRepository.AddAsync(contact);
            
            var studentGroup = StudentGroup.Create(Guid.NewGuid(), createdStudent.Id, createdGroup.Id, dto.Level, dto.PairNumber);
            var createdStudentGroup = await studentGroupRepository.AddAsync(studentGroup);

            return mapper.Map<RegisterStudentResultDto>(
                (createdStudent, createdPerson, createdContact, createdContract, createdGroup, createdLanguage, lessonDays, createdStudentGroup, createdParent)
            );
        }
        
        public async Task<RegisterStudentResultDto?> GetStudentByIdAsync(Guid studentId)
        {
            var student = await studentRepository.GetByIdAsync(studentId);
            if (student == null) return null;

            var person = await personRepository.GetByIdAsync(student.PersonId);
            var contact = (await contactRepository.GetAllAsync())
                .FirstOrDefault(c => c.PersonId == person?.Id);
            var contract = (await contractRepository.GetAllAsync())
                .FirstOrDefault(c => c.StudentId == student.Id);
            var studentGroup = (await studentGroupRepository.GetAllAsync())
                .FirstOrDefault(sg => sg.StudentId == student.Id);
            var group = studentGroup != null ? await groupRepository.GetByIdAsync(studentGroup.GroupId) : null;
            var language = group != null ? await languageRepository.GetByIdAsync(group.LanguageId) : null;
            var parent = student.ParentId.HasValue ? await personRepository.GetByIdAsync(student.ParentId.Value) : null;
            var lessonDays = group != null
                ? await groupLessonDayRepository.GetLessonDaysByGroupIdAsync(group.Id)
                : [];

            if (person == null || contact == null || contract == null || studentGroup == null || group == null || language == null)
                return null;

            return mapper.Map<RegisterStudentResultDto>(
                (student, person, contact, contract, group, language, lessonDays, studentGroup, parent)
            );
        }
        
        public async Task<IEnumerable<RegisterStudentResultDto>> GetAllStudentsAsync(StudentFilterDto? filter = null)
        {
            var students = await studentRepository.GetAllAsync(filter);
            var results = new List<RegisterStudentResultDto>();

            foreach (var student in students)
            {
                var dto = await GetStudentByIdAsync(student.Id);
                if (dto != null) results.Add(dto);
            }

            return results;
        }

        public async Task<RegisterStudentResultDto?> UpdateStudentAsync(Guid id, RegisterStudentDto dto)
        {
            var student = await studentRepository.GetByIdAsync(id);
            if (student == null) return null;

            var person = await personRepository.GetByIdAsync(student.PersonId);
            if (person == null) return null;

            var contact = (await contactRepository.GetAllAsync())
                .FirstOrDefault(c => c.PersonId == person.Id);
            var contract = (await contractRepository.GetAllAsync())
                .FirstOrDefault(c => c.StudentId == student.Id);
            var studentGroup = (await studentGroupRepository.GetAllAsync())
                .FirstOrDefault(sg => sg.StudentId == student.Id);
            
            person.Update(dto.FullName, dto.BirthDate);
            await personRepository.UpdateAsync(person.Id, person);
            
            var parentId = student.ParentId;
            if (dto.IsParentRegister && !string.IsNullOrWhiteSpace(dto.ParentFullName))
            {
                if (student.ParentId.HasValue)
                {
                    var parentToUpdate = await personRepository.GetByIdAsync(student.ParentId.Value);
                    if (parentToUpdate != null)
                    {
                        parentToUpdate.Update(dto.ParentFullName, DateTime.MinValue);
                        await personRepository.UpdateAsync(parentToUpdate.Id, parentToUpdate);
                    }
                }
                else
                {
                    var newParent = Person.Create(Guid.NewGuid(), dto.ParentFullName, DateTime.MinValue);
                    var createdParent = await personRepository.AddAsync(newParent);
                    parentId = createdParent.Id;
                }
            }
            else parentId = null;
            
            var grade = dto.Grade.HasValue && dto.Grade > 0 ? dto.Grade : null;
            student.Update(person.Id, parentId, grade, dto.IsPaid);
            await studentRepository.UpdateAsync(student.Id, student);
            
            if (contact != null)
            {
                contact.Update(dto.Phone, dto.Email);
                await contactRepository.UpdateAsync(contact.Id, contact);
            }
            
            if (contract != null)
            {
                contract.Update(dto.SignDate, dto.PaymentAmount);
                await contractRepository.UpdateAsync(contract.Id, contract);
            }
            
            var language = (await languageRepository.GetAllAsync())
                .FirstOrDefault(l => l.Name == dto.LanguageName)
                    ?? await languageRepository.AddAsync(Language.Create(Guid.NewGuid(), dto.LanguageName));
            
            var group = (await groupRepository.GetAllAsync())
                .FirstOrDefault(g => g.GroupName == dto.GroupName && g.LanguageId == language.Id)
                    ?? await groupRepository.AddAsync(Group.Create(Guid.NewGuid(), dto.GroupName, language.Id));
            
            var existingLessonDays = await groupLessonDayRepository.GetLessonDaysByGroupIdAsync(group.Id);
            foreach (var ld in existingLessonDays)
                await groupLessonDayRepository.DeleteAsync(group.Id, ld.Id);

            var lessonDayNames = string.IsNullOrWhiteSpace(dto.LessonDays)
                ? []
                : dto.LessonDays.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            foreach (var day in lessonDayNames)
            {
                var lessonDay = LessonDay.Create(Guid.NewGuid(), day);
                var createdLessonDay = await lessonDayRepository.AddAsync(lessonDay);
                await groupLessonDayRepository.AddAsync(GroupLessonDay.Create(group.Id, createdLessonDay.Id));
            }
            
            if (studentGroup != null)
            {
                studentGroup.Update(student.Id, group.Id, dto.Level, dto.PairNumber);
                await studentGroupRepository.UpdateAsync(studentGroup.Id, studentGroup);
            }
            else
            {
                var newStudentGroup = StudentGroup.Create(Guid.NewGuid(), student.Id, group.Id, dto.Level, dto.PairNumber);
                await studentGroupRepository.AddAsync(newStudentGroup);
            }
            
            var parentPerson = parentId.HasValue ? await personRepository.GetByIdAsync(parentId.Value) : null;
            var lessonDays = await groupLessonDayRepository.GetLessonDaysByGroupIdAsync(group.Id);

            var result = mapper.Map<RegisterStudentResultDto>(
                (student, person, contact ?? Contact.Create(Guid.NewGuid(), person.Id, dto.Phone, dto.Email),
                 contract ?? await CreateContractAsync(dto, student.Id),
                 group, language, lessonDays,
                 studentGroup ?? StudentGroup.Create(Guid.NewGuid(), student.Id, group.Id, dto.Level, dto.PairNumber),
                 parentPerson)
            );

            return result;
        }

        public async Task DeleteStudentAsync(Guid id)
        {
            var student = await studentRepository.GetByIdAsync(id);
            if (student == null) return;

            var person = await personRepository.GetByIdAsync(student.PersonId);
            
            var contact = (await contactRepository.GetAllAsync())
                .FirstOrDefault(c => c.PersonId == person?.Id);
            
            var contract = (await contractRepository.GetAllAsync())
                .FirstOrDefault(c => c.StudentId == student.Id);
            
            var studentGroup = (await studentGroupRepository.GetAllAsync())
                .FirstOrDefault(sg => sg.StudentId == student.Id);

            if (contact != null)
                await contactRepository.DeleteAsync(contact.Id);
            if (contract != null)
                await contractRepository.DeleteAsync(contract.Id);
            if (studentGroup != null)
                await studentGroupRepository.DeleteAsync(studentGroup.Id);
            
            await studentRepository.DeleteAsync(student.Id);
            
            if (person != null)
                await personRepository.DeleteAsync(person.Id);
        }
        
        private async Task<Contract> CreateContractAsync(RegisterStudentDto dto, Guid studentId)
        {
            var contractNumber = await contractNumberGenerator.Generate(dto.SignDate);
            return Contract.Create(Guid.NewGuid(), contractNumber, dto.SignDate, dto.PaymentAmount, studentId);
        }
    }
}