
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class SettingRepository : EfEntityRepositoryBase<Setting, ProjectDbContext>, ISettingRepository
    {
        public SettingRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
