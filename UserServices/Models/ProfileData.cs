namespace UserServices.Models
{
    public class ProfileData
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
