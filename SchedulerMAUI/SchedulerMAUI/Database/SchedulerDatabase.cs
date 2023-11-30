using SQLite;

namespace SchedulerMAUI
{
    public class SchedulerDatabase
    {
        readonly SQLiteConnection _database;

        public SchedulerDatabase(string dbPath)
        {
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<Appointment>();
        }

        //Get the list of appointments from the database
        public List<Appointment> GetSchedulerAppointment()
        {
            return _database.Table<Appointment>().ToList();
        }

        //Insert an appointment in the database
        public int SaveSchedulerAppointmentAsync(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new Exception("Null");
            }

            return _database.InsertOrReplace(appointment);
        }

        //Delete an appointment in the database 
        public int DeleteSchedulerAppointmentAsync(Appointment appointment)
        {
            return _database.Delete(appointment);
        }
    }
}
