using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public struct JobRequirementVO {
    public int Id { get; set; }
    public string Requirement { get; set; }

    public List<ProjectAdvertisement>? Advertisements { get; set; }
}
