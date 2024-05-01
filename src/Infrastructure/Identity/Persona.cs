using Shared.Enums;

namespace Infrastructure.Identity
{
    public class Persona : BaseIdentity
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FullName { get => $"{FirstName} {LastName}"; }
        public string? PhotoUrl { get; set; }
        public PersonaType PesonaType { get; set; }


        public Persona(DateTime created, bool isDeleted)
        {
            Created = created;
            IsDeleted = isDeleted;
            Id = Guid.NewGuid().ToString();
        }
        public Persona() : this(DateTime.UtcNow, false) { }

        public void Delete(string deletor)
        {
            IsDeleted = true;
            Deleted = DateTime.UtcNow;
            DeletedBy = deletor;
        }

        public void Edit(string editor)
        {
            Modified = DateTime.UtcNow;
            LastModifiedBy = editor;
        }
    }

}
