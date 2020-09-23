using System;
namespace WPSTORE.Models.SocialLogin
{
    public class SocialUser
    {
        public int Id { get; set; }
        public string SocialId { get; set; }
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiresIn { get; set; }

        public string Name { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PictureUrl { get; set; }

        public bool LoggedInWithSocialAccount { get; set; }

        public SocialProvider Provider { get; set; }
    }
}
