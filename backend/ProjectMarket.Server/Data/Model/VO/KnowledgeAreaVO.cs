using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public struct KnowledgeAreaVO {
    public int Id { get; set; }
    public string Name { get; set; }

    public List<ProjectAdvertisement>? Advertisements { get; set; }
}