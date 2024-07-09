namespace ProjectMarket.Server.Data.Model;

public class JobRequirementVO {
    public int Id { get; set; }
    public string Requirement { get; set; }
    public List<ProjectAdvertisement>? Advertisements { get; set; }
}
