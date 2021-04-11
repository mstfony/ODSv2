
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
    public class AlertActionLogRepository : EfEntityRepositoryBase<AlertActionLog, ProjectDbContext>, IAlertActionLogRepository
    {
        public AlertActionLogRepository(ProjectDbContext context) : base(context)
        {
        }

        public async Task<List<AlertActionLog>> GetAlertActionLogByAaId(int id)
        {
            var list = await Context.AlertActionLogs.Where(m => m.AlertActionUserId == id).ToListAsync();
            return list;
        }
    }
}
