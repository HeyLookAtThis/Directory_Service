using System.Globalization;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.ValueObjects
{
    public record Address
    {
        private const int POSTAL_CODE_LENGTH = 6;

        private Address(
            string postalCode,
            string country,
            string region,
            string city,
            string street,
            string house)
        {
            PostalСode = postalCode;
            Country = country;
            Region = region;
            City = city;
            Street = street;
            House = house;
        }

        public string PostalСode { get; }

        public string Country { get; }

        public string Region { get; }

        public string City { get; }

        public string Street { get; }

        public string House { get; }

        public static Result<Address, Error> Create(
            string postalCode,
            string country,
            string region,
            string city,
            string street,
            string house,
            string invalidField)
        {
            bool isDigit = true;

            foreach (var symbol in postalCode)
            {
                isDigit = Char.IsDigit(symbol);

                if (!isDigit)
                    return Error.Validation(invalidField + ".Address",
                        "почтовый код должен состоять только из чисел", invalidField);
            }

            if (string.IsNullOrWhiteSpace(postalCode.ToString(CultureInfo.InvariantCulture)))
                return Error.Validation(invalidField + ".Address", "укажите почтовый код", invalidField);
            else if (postalCode.ToString(CultureInfo.InvariantCulture).Length != POSTAL_CODE_LENGTH)
                return Error.Validation(invalidField + ".Address", "почтовый код состоит из 6 цифр", invalidField);

            if (string.IsNullOrWhiteSpace(country))
                return Error.Validation(invalidField + ".Address", "укажите страну", invalidField);

            if (string.IsNullOrWhiteSpace(region))
                return Error.Validation(invalidField + ".Address", "укажите регион/область", invalidField);

            if (string.IsNullOrWhiteSpace(city))
                return Error.Validation(invalidField + ".Address", "укажите город", invalidField);

            if (string.IsNullOrWhiteSpace(street))
                return Error.Validation(invalidField + ".Address", "укажите улицу", invalidField);

            if (string.IsNullOrWhiteSpace(house))
                return Error.Validation(invalidField + ".Address", "укажите дом", invalidField);

            return new Address(postalCode, country, region, city, street, house);
        }
    }
}