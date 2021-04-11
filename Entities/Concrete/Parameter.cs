using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Nest;

namespace Entities.Concrete
{
    public class Parameter : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
