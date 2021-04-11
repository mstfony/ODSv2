
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataAccess;
using Entities.Concrete;
namespace DataAccess.Abstract
{
    public interface ISensorSettingRepository : IEntityRepository<SensorSetting>
    {
        Task<List<SensorSetting>> GetSensorSettingListBySensorId(int id);
    }
}