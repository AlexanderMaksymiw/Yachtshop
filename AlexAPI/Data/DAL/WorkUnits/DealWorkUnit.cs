using AlexAPI.Data.DAL.Repository;
using AlexAPI.Models;

namespace AlexAPI.Data.DAL.WorkUnits
{
    public class DealWorkUnit : GenericWorkUnit
    {
        private GenericRepository<CharterDeal> charterDealRepository;
        private GenericRepository<Yacht> yachtRepository;
        private GenericRepository<Location> locationRepository;

        public DealWorkUnit(ApplicationDbContext context) : base(context)
        {
        }

        public GenericRepository<CharterDeal> CharterDealRepository
        {
            get
            {
                if (charterDealRepository == null)
                {
                    charterDealRepository = new GenericRepository<CharterDeal>(_context);
                }
                return charterDealRepository;
            }
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
