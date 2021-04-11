
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class ParameterRepository : EfEntityRepositoryBase<Parameter, ProjectDbContext>, IParameterRepository
    {
        public ParameterRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
