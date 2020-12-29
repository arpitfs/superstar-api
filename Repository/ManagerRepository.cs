using ApiWorld.Data;
using ApiWorld.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWorld.Repository
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ManagerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteAsync(string managerId)
        {
            var manager = _dbContext.Managers.Where(x => x.ManagerId == managerId);
            _dbContext.Entry(manager).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            var value = await _dbContext.SaveChangesAsync();
            return (value > 0) ? true : false;
        }

        public IEnumerable<Manager> GetAll() => _dbContext.Managers.ToList();

        public async Task Save(Manager manager)
        {
            await _dbContext.AddAsync(manager);
            _dbContext.SaveChanges();
        }
    }
}
