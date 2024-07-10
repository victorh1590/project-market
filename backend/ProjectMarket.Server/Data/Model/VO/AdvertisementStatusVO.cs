using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public struct AdvertisementStatusVO {
    required public int Id { get; set; }
    required public string Status { get; set; }
    //
    public List<ProjectAdvertisement>? Advertisements { get; set; }
}
