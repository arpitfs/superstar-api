using ApiWorld.Data;
using ApiWorld.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWorld.Repository
{
    public class SuperStarRepository : ISuperStarRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SuperStarRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteAsync(string superStarId)
        {
            var superStar = _dbContext.SuperStars.Where(x => x.SuperstarId == superStarId);
            _dbContext.Entry(superStar).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            var value = await _dbContext.SaveChangesAsync();
            return (value > 0) ? true : false;
        }

        public IEnumerable<SuperStar> GetAll() => _dbContext.SuperStars.ToList();

        public async Task Save(SuperStar superStar)
        {
            await _dbContext.AddAsync(superStar);
            _dbContext.SaveChanges();
        }
    }
}
