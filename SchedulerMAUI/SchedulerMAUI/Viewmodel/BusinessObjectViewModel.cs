using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SchedulerMAUI
{
    public class BusinessObjectViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SchedulerAppointment> appointments;

        public BusinessObjectViewModel()
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

        /// <summary>
        /// Property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<SchedulerAppointment> Appointments
        {
            get
            {
                return appointments;
            }
            set
            {
                appointments = value;
                this.RaiseOnPropertyChanged(nameof(Appointments));
            }
        }

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

        /// <summary>
        /// Invoke method when property changed.
        /// </summary>
        /// <param name="propertyName">property name</param>
        private void RaiseOnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
