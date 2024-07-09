namespace ProjectMarket.Server.Data.Model;

public class AdvertisementStatusVO {
    public int Id { get; set; }
    public string Status { get; set; }
    public List<ProjectAdvertisement>? Advertisements { get; set; }
}
