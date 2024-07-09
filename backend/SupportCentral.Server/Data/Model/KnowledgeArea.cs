namespace SupportCentral.Server.Data.Model;

public class KnowledgeArea {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ProjectAdvertisement>? Advertisements { get; set; }
}