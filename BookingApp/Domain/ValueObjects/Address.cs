namespace Domain.ValueObjects
{
    public class Address
    {
        public string Country { get; private set; }
        public string CountryCode { get; private set; }
        public string City { get; private set; }
        public string Venue { get; private set; }

        // for EF.Core to map data correctly
        private Address() { }

        public Address(string country, string countryCode, string city, string venue)
        {
            Country = country;
            CountryCode = countryCode;
            City = city;
            Venue = venue;
        }
    }
}