using ApiWorld.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiWorld.Repository
{
    public interface IManagerRepository
    {
        IEnumerable<Manager> GetAll();
        Task<bool> DeleteAsync(string managerId);
    }
}