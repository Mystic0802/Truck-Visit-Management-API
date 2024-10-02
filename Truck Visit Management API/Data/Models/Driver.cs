namespace Truck_Visit_Management_API.Data.Models
{
    public interface IDriver
    {
        int Id { get; }
        string FirstName { get; }
        string LastName { get; }
    }

    public class Driver(string fName, string lName) : IDriver
    {
        private static int _nextId = 0;

        public int Id { get; private set; } = Interlocked.Increment(ref _nextId);
        public string FirstName { get; private set; } = fName;
        public string LastName { get; private set; } = lName;
    }
}
