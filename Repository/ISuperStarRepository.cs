using ApiWorld.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiWorld.Repository
{
    public interface ISuperStarRepository
    {
        Task Save(SuperStar superStar);
        IEnumerable<SuperStar > GetAll();
        Task<bool> DeleteAsync(string superStarId);
        Task<bool> ClaimedUserAsync(string superStarId);
    }
}
