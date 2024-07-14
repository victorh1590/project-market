using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public class ProjectAdvertisementJobRequirementVO {
    required public int ProjectAdvertisement { get; set; }
    required public int JobRequirement { get; set; }
    //
    public List<ProjectAdvertisement>? Advertisements { get; set; }
}
