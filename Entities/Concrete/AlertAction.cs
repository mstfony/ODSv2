using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class AlertAction : IEntity
    {
        public int Id { get; set; }
        public int SensorSettingId { get; set; }
        public int AlertId { get; set; }
        public string Message { get; set; }
    }
}
