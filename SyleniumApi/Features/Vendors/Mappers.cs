using SyleniumApi.Data.Entities;

namespace SyleniumApi.Features.Vendors;

public static class Mappers
{
    public static GetVendorResponse ToGetResponse(this Vendor vendor)
    {
        return new GetVendorResponse(vendor.Id, vendor.Name);
    }

    public static CreateVendorResponse ToCreateResponse(this Vendor vendor)
    {
        return new CreateVendorResponse(vendor.Id, vendor.Name);
    }

    public static UpdateVendorResponse ToUpdateResponse(this Vendor vendor)
    {
        return new UpdateVendorResponse(vendor.Id, vendor.Name);
    }
}