using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer.TestBase.Mock
{
    public interface IUser
    {
        Guid Id { get; }

        IPerson Person { get; set; }
    }

    public class User : IUser
    {
        public Guid Id { get; private set; }

        public User()
        {
            this.Id = Guid.NewGuid();
        }

        public IPerson Person { get; set; }
    }
}
