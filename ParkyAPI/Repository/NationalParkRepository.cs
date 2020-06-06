using System.Collections.Generic;
using System.Linq;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly AppDbContext _context;

        public NationalParkRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public ICollection<NationalPark> GetNationalParks()
        {
            return _context.NationalParks.OrderBy(p => p.Name).ToList();
        }

        public NationalPark GetNationalPark(int id)
        {
            return _context.NationalParks.FirstOrDefault(park => park.Id == id);
        }

        public bool NationalParkExists(string name)
        {
            return _context.NationalParks.Any(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool NationalParkExists(int id)
        {
            return _context.NationalParks.Any(p => p.Id == id);
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _context.NationalParks.Update(nationalPark);
            return Save();
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
             _context.NationalParks.Add(nationalPark);
             return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _context.NationalParks.Remove(nationalPark);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }
    }
}