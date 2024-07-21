namespace ProjectMarket.Server.Data.Model.Dto;

public class ProjectAdvertisementDto(
    int? id,
    string title,
    string? description,
    DateTime? deadline,
    int paymentOfferId,
    int customerId,
    string statusName,
    List<string> subjectNames,
    List<string>? requirementNames,
    DateTime openedOn)
{
    public int? ProjectAdvertisementId { get; init; } = id;
    public string Title { get;} = title;
    public string? Description { get;} = description;
    public DateTime OpenedOn { get;} = openedOn;
    public DateTime? Deadline { get;} = deadline;
    public int PaymentOfferId { get;} = paymentOfferId;
    public int CustomerId { get;} = customerId;
    public string StatusName { get;} = statusName;
    public List<string> SubjectNames { get;} = subjectNames;
    public List<string>? RequirementNames { get;} = requirementNames;
}
