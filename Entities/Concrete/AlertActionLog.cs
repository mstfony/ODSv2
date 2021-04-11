using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Nest;

namespace Entities.Concrete
{
   public class AlertActionLog : IEntity
    {
        public int Id { get; set; }
        public int AlertActionUserId { get; set; }
        public DateTime DateTime { get; set; }

    }
}
