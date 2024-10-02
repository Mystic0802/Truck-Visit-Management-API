using Microsoft.AspNetCore.Routing.Constraints;
using System.Text.RegularExpressions;
using Truck_Visit_Management_API.Controllers.Models;

namespace Truck_Visit_Management_API.Data.Models
{
    public interface ITruckVisit
    {
        int Id { get; }
        TruckVisit.TruckVisitStatus Status { get; set; }
        List<VisitActivity> Activities { get; set; }
        string LicensePlate { get; set; }
        Driver TruckDriver { get; set; }
        DateTime UpdatedTime { get; set; }
        string UpdatedBy { get; set; }
        DateTime CreatedTime { get; set; }
        string CreatedBy { get; set; }
    }

    public class TruckVisit : ITruckVisit
    {
        private static int _nextId = 0;

        public int Id { get; }
        public TruckVisitStatus Status { get; set; }
        public List<VisitActivity> Activities { get; set; }
        public string LicensePlate { get; set; }
        public Driver TruckDriver { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }

        public enum TruckVisitStatus
        {
            PreRegistered = 0,
            AtGate,
            OnSite,
            Completed
        }

        public TruckVisit(TruckVisitStatus status, List<VisitActivity> activities, string licensePlate, Driver driver, string createdBy)
        {
            Id = Interlocked.Increment(ref _nextId);
            Status = status;
            Activities = activities;
            LicensePlate = licensePlate;
            TruckDriver = driver;
            UpdatedBy = CreatedBy = createdBy;
            UpdatedTime = CreatedTime = DateTime.Now;
        }

        public TruckVisit(CreateTruckVisitRequest request)
        {
            Id = Interlocked.Increment(ref _nextId);
            Status = request.Status;
            Activities = new List<VisitActivity>();
            foreach (var a in request.Activities)
            {
                Activities.Add(new VisitActivity(a.ActivityType, a.UnitNum));
            }
            LicensePlate = request.LicensePlate;

            UpdatedBy = CreatedBy = request.CreatedBy;
            UpdatedTime = CreatedTime = DateTime.Now;
        }
    }
}
