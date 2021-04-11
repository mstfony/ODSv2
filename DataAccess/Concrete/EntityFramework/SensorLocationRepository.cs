
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class SensorLocationRepository : EfEntityRepositoryBase<SensorLocation, ProjectDbContext>, ISensorLocationRepository
    {
        public SensorLocationRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
