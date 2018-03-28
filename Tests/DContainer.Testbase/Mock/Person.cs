using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer.TestBase.Mock
{
    public interface IPerson
    {
        Guid PersonId { get; }
    }

    public class Person : IPerson
    {
        public Guid PersonId { get; set; }

        public Person()
        {
            this.PersonId = Guid.NewGuid();
        }
    }
}
