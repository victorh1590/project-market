using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public struct AdvertisementStatusVO {
    public int Id { get; set; }
    public string Status { get; set; }

    public List<ProjectAdvertisement>? Advertisements { get; set; }
}
