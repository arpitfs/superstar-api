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

        public IEnumerable<SuperStar> GetAll() => _dbContext.SuperStars.ToList();

        public async Task Save(SuperStar superStar)
        {
            await _dbContext.AddAsync(superStar);
            _dbContext.SaveChanges();
        }
    }
}
