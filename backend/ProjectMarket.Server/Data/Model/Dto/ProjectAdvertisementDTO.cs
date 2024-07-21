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
    public string Title { get; set; } = title;
    public string? Description { get; set; } = description;
    public DateTime OpenedOn { get; set; } = openedOn;
    public DateTime? Deadline { get; set; } = deadline;
    public int PaymentOfferId { get; set; } = paymentOfferId;
    public int CustomerId { get; set; } = customerId;
    public string StatusName { get; set; } = statusName;
    public List<string> SubjectNames { get; set; } = subjectNames;
    public List<string>? RequirementNames { get; set; } = requirementNames;
}
