using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public class KnowledgeAreaVO {
    required public int KnowledgeAreaId { get; set; }
    required public string Name { get; set; }
    //
    public List<ProjectAdvertisement>? Advertisements { get; set; }
}