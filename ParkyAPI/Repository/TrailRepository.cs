using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly AppDbContext _context;

        public TrailRepository(AppDbContext context)
        {
            _context = context;
        }


        public ICollection<Trail> GetTrails()
        {
            return _context.Trails.
                Include(t => t.NationalPark).
                ToList();
        }

        public Trail GetTrail(int id)
        {
            return _context.Trails.
                Include(t => t.NationalPark).
                FirstOrDefault(t => t.Id == id);
        }

        public bool TrailExists(string name)
        {
            return _context.Trails.Any(t => t.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool TrailExists(int id)
        {
            return _context.Trails.Any(t => t.Id == id);
        }

        public bool UpdateTrail(Trail trail)
        {
            _context.Trails.Update(trail);
            return Save();
        }

        public bool CreateTrail(Trail trail)
        {
            _context.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _context.Trails.Remove(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int id)
        {
            return _context.Trails.
                Include(t => t.NationalPark).
                Where(np => np.Id == id).
                ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }
    }
}