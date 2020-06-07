using System.Collections.Generic;
using ParkyAPI.Models;

namespace ParkyAPI.Repository.IRepository
{
    public interface ITrailRepository
    {
        ICollection<Trail> GetTrails();
        Trail GetTrail(int id);
        bool TrailExists(string name);
        bool TrailExists(int id);
        bool UpdateTrail(Trail trail);
        bool CreateTrail(Trail trail);
        bool DeleteTrail(Trail trail);
        ICollection<Trail> GetTrailsInNationalPark(int id);
        bool Save();
    }
}