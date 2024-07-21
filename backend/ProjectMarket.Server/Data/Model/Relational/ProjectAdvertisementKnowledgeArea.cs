using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Data.Model.Relational;

public class ProjectAdvertisementKnowledgeArea {
    required public ProjectAdvertisement ProjectAdvertisement { get; set; }
    required public KnowledgeAreaVo KnowledgeArea { get; set; }
}
