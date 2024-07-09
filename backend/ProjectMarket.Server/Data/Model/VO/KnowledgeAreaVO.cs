namespace ProjectMarket.Server.Data.Model;

public class KnowledgeAreaVO {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ProjectAdvertisement>? Advertisements { get; set; }
}