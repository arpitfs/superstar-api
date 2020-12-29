using ApiWorld.Data;
using ApiWorld.Domain;
using System.Collections.Generic;
using System.Linq;

namespace ApiWorld.Repository
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ManagerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Manager> GetAll() => _dbContext.Managers.ToList();
    }
}
