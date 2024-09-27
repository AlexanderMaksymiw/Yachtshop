using AlexAPI.Data.DAL.Repository;
using AlexAPI.Models;

namespace AlexAPI.Data.DAL.WorkUnits
{
    public class YachtWorkUnit : GenericWorkUnit
    {
        private GenericRepository<Yacht> yachtRepository;
        private GenericRepository<Location> locationRepository;
        public YachtWorkUnit(ApplicationDbContext context) : base(context)
        {
        }

        public GenericRepository<Yacht> YachtRepository
        {
            get
            {
                if (yachtRepository == null)
                {
                    yachtRepository = new GenericRepository<Yacht>(_context);
                }
                return yachtRepository;
            }
        }

        public GenericRepository<Location> LocationRepository
        {
            get
            {
                if (locationRepository == null)
                {
                    locationRepository = new GenericRepository<Location>(_context);
                }
                return locationRepository;
            }
        }
    }
}
