using FluentNHibernate.Mapping;
using Microsoft.AspNet.Identity;
using System;

namespace TestElma.Models
{
    public class User : IUser<Guid>
    {
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password{ get; set; }

        public class Map : ClassMap<User>
        {
            public Map()
            {
                Table("Users");
                Id(x => x.Id, "UserId").GeneratedBy.Guid();
                Map(x => x.UserName).Not.Nullable();
                Map(x => x.Email);
                Map(x => x.Password).Not.Nullable();
            }
        }
    }
}