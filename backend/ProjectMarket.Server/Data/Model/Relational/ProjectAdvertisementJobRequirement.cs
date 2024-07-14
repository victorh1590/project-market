using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Model.Relational;

public class ProjectAdvertisementJobRequirement {
    required public ProjectAdvertisement ProjectAdvertisement { get; set; }
    required public JobRequirement JobRequirement { get; set; }
}
