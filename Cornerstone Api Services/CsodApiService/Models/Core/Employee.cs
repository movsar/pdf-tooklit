namespace CornerstoneApiServices.Models.Core
{
    public class Employee
    {
        public int? Id { get; set; }
        public Address? Address { get; set; }
        public int? ApproverId { get; set; }
        public List<CustomField>? CustomFields { get; set; }
        public EmployeeMetaData? EmployeeMetaData { get; set; }
        public string? ExternalId { get; set; }
        public string? Fax { get; set; }
        public string? FirstName { get; set; }
        public string? Guid { get; set; }
        public string? HomePhone { get; set; }
        public string? LastName { get; set; }
        public int? ManagerId { get; set; }
        public string? MiddleName { get; set; }
        public string? MobilePhone { get; set; }
        public List<Ous>? Ous { get; set; }
        public string? PersonalEmail { get; set; }
        public string? Prefix { get; set; }
        public string? PrimaryEmail { get; set; }
        public List<Relation>? Relations { get; set; }
        public Settings? Settings { get; set; }
        public string? Suffix { get; set; }
        public string? UserName { get; set; }
        public string? WorkPhone { get; set; }
        public WorkerStatus? WorkerStatus { get; set; }
    }
}