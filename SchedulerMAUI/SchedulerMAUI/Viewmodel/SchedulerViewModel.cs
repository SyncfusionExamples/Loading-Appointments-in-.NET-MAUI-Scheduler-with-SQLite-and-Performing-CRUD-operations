using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;

namespace SchedulerMAUI
{
    public class SchedulerViewModel 
    {
        public SchedulerViewModel()
        {
            this.GenerateAppointments();

            var dataBaseAppointments = App.Database.GetSchedulerAppointment();
            if (dataBaseAppointments != null)
            {
                foreach (Appointment appointment in dataBaseAppointments)
                {
                    Appointments.Add(new SchedulerAppointment()
                    {
                        StartTime = appointment.From,
                        EndTime = appointment.To,
                        Subject = appointment.EventName,
                        IsAllDay = appointment.AllDay,
                        Id = appointment.ID
                    });
                }
            }
        }

        public ObservableCollection<SchedulerAppointment> Appointments { get; set; }

        private void GenerateAppointments()
        {
            if (Appointments == null)
            {
                Appointments = new ObservableCollection<SchedulerAppointment>()
            {
                new SchedulerAppointment() { StartTime = DateTime.Now.Date.AddHours(9), EndTime = DateTime.Now.Date.AddHours(10), Subject = "Meeting" }
            };

            }
        }
    }
}
