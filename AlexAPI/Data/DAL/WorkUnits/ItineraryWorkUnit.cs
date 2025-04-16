using AlexAPI.Data.DAL.Repository;
using AlexAPI.Models;

namespace AlexAPI.Data.DAL.WorkUnits
{
    public class ItineraryWorkUnit : GenericWorkUnit
    {
        private GenericRepository<UserItinerary> userItineraryRepository;
        private GenericRepository<Yacht> yachtRepository;
        private GenericRepository<Location> locationRepository;

        public ItineraryWorkUnit(ApplicationDbContext context) : base(context)
        {
        }

        public GenericRepository<UserItinerary> UserItineraryRepository
        {

            get
            {
                if (UserItineraryRepository == null)
                {
                    userItineraryRepository = new GenericRepository<UserItinerary>(_context);
                }
                return UserItineraryRepository;

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
