using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer.Castle.Test.Mock
{
    public interface IUser
    {
        Guid Id { get; }
    }

    public class User : IUser
    {
        public Guid Id { get; private set; }

        public User()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
