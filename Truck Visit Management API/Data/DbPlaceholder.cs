using Truck_Visit_Management_API.Data.Models;

namespace Truck_Visit_Management_API.Data
{
    public interface IDbPlaceholder
    {
        IRepository<TruckVisit> TruckVisits { get; }
        IRepository<VisitActivity> VisitActivities { get; }
        IRepository<Driver> Drivers { get; }
    }

    /// <summary>
    /// A class to mimic db interaction.
    /// </summary>
    public class DbPlaceholder : IDbPlaceholder
    {
        public IRepository<TruckVisit> TruckVisits { get; } 
        public IRepository<VisitActivity> VisitActivities { get; }
        public IRepository<Driver> Drivers { get; }

        public DbPlaceholder()
        {
            TruckVisits = new Repository<TruckVisit>();
            VisitActivities = new Repository<VisitActivity>();
            Drivers = new Repository<Driver>();
        }

        public DbPlaceholder(IRepository<ITruckVisit> truckVisits, IRepository<IVisitActivity> visitActivities, IRepository<IDriver> drivers)
        {
            TruckVisits = (IRepository<TruckVisit>?)truckVisits;
            VisitActivities = (IRepository<VisitActivity>?)visitActivities;
            Drivers = (IRepository<Driver>?)drivers;
        }
    }
}
