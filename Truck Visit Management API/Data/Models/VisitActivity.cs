namespace Truck_Visit_Management_API.Data.Models
{
    public interface IVisitActivity
    {
        int Id { get; }
        VisitActivity.ActivityType Type { get; }
        string UnitNumber { get; }
    }

    public class VisitActivity(VisitActivity.ActivityType activityType, string unitNum) : IVisitActivity
    {
        private static int _nextId = 0;

        public int Id { get; } = Interlocked.Increment(ref _nextId);
        public ActivityType Type { get; private set; } = activityType;
        public string UnitNumber { get; private set; } = unitNum;

        public enum ActivityType
        {
            Delivery,
            Collection
        }

        public static bool IsValidUnitNumber(string unitNum)
        {
            if (unitNum == null) return false;

            // Very basic format validation.
            var length = unitNum.Trim().Length;
            if (length <= 0 || unitNum.Length > 9)
                return false;

            return true;
        }
    }

}
