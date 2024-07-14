using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public class AdvertisementStatus {
    required public int AdvertisementStatusId { get; set; }
    required public string Status { get; set; }
    //
    public List<ProjectAdvertisement>? Advertisements { get; set; }
}
