using System;
using WPSTORE.Models.SocialLogin;

namespace WPSTORE.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string SocialId { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime ExpiresIn { get; set; }
        public string PictureUrl { get; set; }
        public bool LoggedInWithSocialAccount { get; set; }
        public SocialProvider Provider { get; set; }
    }
}
