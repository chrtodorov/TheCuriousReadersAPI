using BusinessLayer.Enumerations;

namespace BusinessLayer.Responses
{
    public class UserResponse
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public AccountStatus Status { get; set; }
    }
}
