using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public struct AdvertisementStatus {
    required public int AdvertisementStatusId { get; set; }
    required public string Status { get; set; }
}
