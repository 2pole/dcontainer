using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DContainer.Spring.Test
{
    public abstract class TestFixtureBase<T>// : IUseFixture<T>
        where T : new()
    {
        public T FixtureData { get; private set; }

        public virtual void SetFixture(T data)
        {
            this.FixtureData = data;
        }
    }
}
