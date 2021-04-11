
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class SensorSettingRepository : EfEntityRepositoryBase<SensorSetting, ProjectDbContext>, ISensorSettingRepository
    {
        public SensorSettingRepository(ProjectDbContext context) : base(context)
        {
        }

        public async Task<List<SensorSetting>> GetSensorSettingListBySensorId(int id)
        {
            var list =await Context.SensorSettings.Where(m => m.SensorId == id).ToListAsync();
            return list;
        }
    }
}
