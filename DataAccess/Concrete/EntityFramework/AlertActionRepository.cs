
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
    public class AlertActionRepository : EfEntityRepositoryBase<AlertAction, ProjectDbContext>, IAlertActionRepository
    {
        public AlertActionRepository(ProjectDbContext context) : base(context)
        {
        }

        public async Task<List<AlertAction>> GetAlertActionListBySensorSettingId(int id)
        {
            var list = await Context.AlertActions.Where(m => m.SensorSettingId == id).ToListAsync();
            return list;
        }
    }
}
