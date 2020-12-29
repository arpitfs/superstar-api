using ApiWorld.Domain;
using System.Collections.Generic;

namespace ApiWorld.Repository
{
    public interface IManagerRepository
    {
        IEnumerable<Manager> GetAll();
    }
}