using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DContainer.Castle.Test.Mock
{
    public interface IBody
    {
        string Content { get; set; }
    }

    public class Body : IBody
    {
        public string Content { get; set; }

        public Body()
        {
            this.Content = Guid.NewGuid().ToString();
        }
    }
}
