using BusinessLayer.Models;
using DataAccess.Entities;

namespace DataAccess.Mappers
{
    public static class AddressMapper
    {
        public static AddressEntity ToAddressEntity(this Address address)
        {
            return new AddressEntity
            {
                Country = address.Country,
                City = address.City,
                Street = address.Street,
                StreetNumber = address.StreetNumber,
                BuildingNumber = address.BuildingNumber,
                ApartmentNumber = address.ApartmentNumber,
                AdditionalInfo = address.AdditionalInfo,
            };
        }
    }
}
