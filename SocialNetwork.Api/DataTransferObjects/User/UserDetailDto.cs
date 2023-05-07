using SocialNetwork.Api.DataTransferObjects.Post;

namespace SocialNetwork.Api.DataTransferObjects.User
{
    public class UserDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public ICollection<PostDetailDto> Posts { get; set; } = new HashSet<PostDetailDto>();
    }
}
