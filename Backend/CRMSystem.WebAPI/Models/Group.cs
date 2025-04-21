namespace CRMSystem.WebAPI.Models
{
    public class Group
    {
        public Guid Id { get; private set; }
        public string GroupName { get; private set; }
        public Guid LanguageId { get; private set; }

        private Group(Guid id, string groupName, Guid languageId)
        {
            Id = id;
            GroupName = groupName;
            LanguageId = languageId;
        }

        public static Group Create(Guid id, string groupName, Guid languageId) =>
            new(id, groupName, languageId);
    }
}