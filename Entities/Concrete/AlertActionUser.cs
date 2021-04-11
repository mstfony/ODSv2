using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class AlertActionUser : IEntity
    {
        public int Id { get; set; }
        public int AlertActionId { get; set; }
        public int UserId { get; set; }
    }
}
