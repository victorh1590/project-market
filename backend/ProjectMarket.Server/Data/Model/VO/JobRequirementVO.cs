using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public struct JobRequirementVO {
    required public int JobRequirementId { get; set; }
    required public string Requirement { get; set; }
    //
    public List<ProjectAdvertisement>? Advertisements { get; set; }
}
