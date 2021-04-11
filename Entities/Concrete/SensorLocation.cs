using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class SensorLocation : IEntity
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public int LocationId { get; set; }
    }
}
