using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class Sensor : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }

    }
}
