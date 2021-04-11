
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class SensorValueRepository : EfEntityRepositoryBase<SensorValue, ProjectDbContext>, ISensorValueRepository
    {
        public SensorValueRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
