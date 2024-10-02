using System.ComponentModel.DataAnnotations;
using Truck_Visit_Management_API.Data.Models;

namespace Truck_Visit_Management_API.Controllers.Models
{
    public class CreateTruckVisitRequest
    {
        [Required]
        public TruckVisit.TruckVisitStatus Status { get; set; }

        [Required]
        public List<VisitActivityRequest> Activities { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "License plate cannot be longer than 10 characters.")]
        public string LicensePlate { get; set; }

        [Required]
        public int DriverId { get; set; }

        [Required]
        public string CreatedBy { get; set; }
    }

    public class UpdateTruckVisitRequest
    {
        [Required]
        public TruckVisit.TruckVisitStatus Status { get; set; }

        [Required]
        public string UpdatedBy { get; set; }
    }

    public class CreateDriverRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class VisitActivityRequest
    {
        public VisitActivity.ActivityType ActivityType { get; set; }
        public string UnitNum { get; set; }
    }
}
