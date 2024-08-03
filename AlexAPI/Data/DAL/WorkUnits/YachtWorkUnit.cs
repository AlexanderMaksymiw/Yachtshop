using AlexAPI.Data.DAL.Repository;
using AlexAPI.Models;

namespace AlexAPI.Data.DAL.WorkUnits
{
    public class YachtWorkUnit : GenericWorkUnit
    {
        private GenericRepository<Yacht> yachtRepository;
        private GenericRepository<YachtDetail> yachtDetailRepository;
        private GenericRepository<YachtBrochure> yachtBrochureRepository;
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

        public GenericRepository<YachtDetail> YachtDetailRepository
        {
            get
            {
                if (yachtDetailRepository == null)
                {
                    yachtDetailRepository = new GenericRepository<YachtDetail>(_context);
                }
                return yachtDetailRepository;
            }
        }

        public GenericRepository<YachtBrochure> YachtBrochureRepository
        {
            get
            {
                if (yachtBrochureRepository == null)
                {
                    yachtBrochureRepository = new GenericRepository<YachtBrochure>(_context);
                }
                return yachtBrochureRepository;
            }
        }
    }
}
