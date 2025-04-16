using SQLite;

namespace SchedulerMAUI
{
    public class Appointment
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool AllDay { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
