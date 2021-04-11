
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataAccess;
using Entities.Concrete;
namespace DataAccess.Abstract
{
    public interface IAlertActionUserRepository : IEntityRepository<AlertActionUser>
    {
        Task<List<AlertActionUser>> GetAlertActionUserByAlertActionId(int id);
        Task<AlertActionUser> GetAlertActionUserByAlertActionIdAndUserId(int alertActionId,int userId);
    }
}