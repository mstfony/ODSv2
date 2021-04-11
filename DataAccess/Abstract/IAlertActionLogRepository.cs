
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataAccess;
using Entities.Concrete;
namespace DataAccess.Abstract
{
    public interface IAlertActionLogRepository : IEntityRepository<AlertActionLog>
    {
        Task<List<AlertActionLog>> GetAlertActionLogByAaId(int id);
    }
}