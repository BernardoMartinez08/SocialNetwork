namespace SocialNetwork.Api.DataTransferObjects.User
{
    public class UserCreateDto
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
    }
}
