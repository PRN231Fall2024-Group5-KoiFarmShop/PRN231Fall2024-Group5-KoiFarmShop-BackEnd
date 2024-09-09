using Microsoft.AspNetCore.Identity;

namespace Koi.Repositories.Entities
{
    public class Role : IdentityRole<int>
    {
        public Role(string rolename)
        {
            this.Name = rolename;
        }
    }
}