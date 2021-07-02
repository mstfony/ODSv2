
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class DeviceSensorRepository : EfEntityRepositoryBase<DeviceSensor, ProjectDbContext>, IDeviceSensorRepository
    {
        public DeviceSensorRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
