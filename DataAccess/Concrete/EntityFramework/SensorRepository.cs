
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class SensorRepository : EfEntityRepositoryBase<Sensor, ProjectDbContext>, ISensorRepository
    {
        public SensorRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
