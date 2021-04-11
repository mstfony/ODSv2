using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Nest;

namespace Entities.Concrete
{
    public class Setting : IEntity
    {
        public int Id { get; set; }
        public int ParameterId { get; set; }
        public double Value { get; set; }

    }
}
