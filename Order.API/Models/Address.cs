namespace Order.API.Models
{
    public class Address
    {
        public Address(string province, string city,string district, string street,string text, string? phone, string? zipCode)
        {
      
            this.Province = province;
            this.City = city;
            this.District = district;
            this.Street = street;
            this.Text = text;
            this.Phone = phone;
            this.ZipCode = zipCode;
        }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string Text { get; set; }
        public string? Phone { get; set; }
        public string? ZipCode { get; set; }
    }
}