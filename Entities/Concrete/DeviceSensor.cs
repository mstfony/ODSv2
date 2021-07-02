using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class DeviceSensor : IEntity
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int SensorId { get; set; }
    }
}
