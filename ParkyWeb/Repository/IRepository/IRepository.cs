using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkyWeb.Repository.IRepository
{
    public interface IRepository<T>
    where T : class
    {
        Task<T> GetAsync(string url, int id);
        Task<IEnumerable<T>> GetAllAsync(string url);
        Task<bool> CreateAsync(string url, T objToCreate);
        Task<bool> DeleteAsync(string url, int id);
        Task<bool> UpdateAsync(string url, T objToUpdate, int id);
    }
}