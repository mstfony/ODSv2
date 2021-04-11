
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
    public class AlertActionUserRepository : EfEntityRepositoryBase<AlertActionUser, ProjectDbContext>, IAlertActionUserRepository
    {
        public AlertActionUserRepository(ProjectDbContext context) : base(context)
        {
        }

        public async Task<List<AlertActionUser>> GetAlertActionUserByAlertActionId(int id)
        {
            var list = await Context.AlertActionUsers.Where(m => m.AlertActionId == id).ToListAsync();
            return list;
        }

        public async Task<AlertActionUser> GetAlertActionUserByAlertActionIdAndUserId(int alertActionId, int userId)
        {

            var list = await Context.AlertActionUsers.FirstOrDefaultAsync(m => m.AlertActionId == alertActionId && m.UserId==userId);
            return list;
        }
    }
}
