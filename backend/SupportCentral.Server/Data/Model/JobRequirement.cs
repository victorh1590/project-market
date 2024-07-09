namespace SupportCentral.Server.Data.Model;

public class JobRequirement {
    public int Id { get; set; }
    public string Requirement { get; set; }
    public List<ProjectAdvertisement>? Advertisements { get; set; }
}
