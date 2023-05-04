namespace SocialNetwork.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public ICollection<Comment> Commends { get; set; } = new HashSet<Comment>();
    }
}
