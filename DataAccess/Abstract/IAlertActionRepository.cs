
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataAccess;
using Entities.Concrete;
namespace DataAccess.Abstract
{
    public interface IAlertActionRepository : IEntityRepository<AlertAction>
    {
        Task<List<AlertAction>> GetAlertActionListBySensorSettingId(int id);
    }
}