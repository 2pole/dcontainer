using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer.TestBase.Mock
{
    public interface IRole
    {
        Guid RoleId { get; }
    }

    public class Role : IRole
    {
        public Guid RoleId { get; set; }

        public Role()
        {
            this.RoleId = Guid.NewGuid();
        }
    }
}
