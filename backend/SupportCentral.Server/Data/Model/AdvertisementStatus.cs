namespace SupportCentral.Server.Data.Model;

public class AdvertisementStatus {
    public int Id { get; set; }
    public string Status { get; set; }
    public List<ProjectAdvertisement>? Advertisements { get; set; }
}
