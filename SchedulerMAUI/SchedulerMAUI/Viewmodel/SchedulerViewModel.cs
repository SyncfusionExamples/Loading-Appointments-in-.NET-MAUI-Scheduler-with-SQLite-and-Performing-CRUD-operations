using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SchedulerMAUI
{
    public class SchedulerViewModel : INotifyPropertyChanged
    {
        private SchedulerAppointment? appointment;
        private DateTime selectedDate;

        public Command? AddAppointment { get; set; }

        public Command? DeleteAppointment { get; set; }

        public Command? CancelEditAppointment { get; set; }

        public AppointmentEditorModel AppointmentEditorModel { get; set; } = new AppointmentEditorModel();

        public ObservableCollection<SchedulerAppointment>? Appointments { get; set; } = new ObservableCollection<SchedulerAppointment>();

        private bool isOpen;
        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
                OnPropertyChanged(nameof(this.IsOpen));
            }
        }

        public SchedulerViewModel()
        {
            AddAppointment = new Command(AddAppointmentDetails);
            DeleteAppointment = new Command(DeleteSchedulerAppointment);
            CancelEditAppointment = new Command(CancelEdit);
            this.AppointmentEditorModel = new AppointmentEditorModel();

            Appointments = new ObservableCollection<SchedulerAppointment>();

            var dataBaseAppointments = App.Database.GetSchedulerAppointment();
            if (dataBaseAppointments != null && dataBaseAppointments.Count > 0)
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
            else
            {
                this.GenerateAppointments();
            }
        }

        private void GenerateAppointments()
        {
            SchedulerAppointment appointment1 = new SchedulerAppointment() { StartTime = DateTime.Now.Date.AddHours(9), EndTime = DateTime.Now.Date.AddHours(10), Subject = "Meeting" };
            SchedulerAppointment appointment2 = new SchedulerAppointment() { StartTime = DateTime.Now.Date.AddDays(-1).AddHours(9), EndTime = DateTime.Now.Date.AddDays(-1).AddHours(10), Subject = "Meeting" };
            this.Appointments?.Add(appointment1);
            this.Appointments?.Add(appointment2);

            var editAppointment = new Appointment() { From = appointment1.StartTime, To = appointment1.EndTime, AllDay = appointment1.IsAllDay, Notes = appointment1.Notes, EventName = appointment1.Subject, ID = (int)appointment1.Id };
            var editAppointment1 = new Appointment() { From = appointment2.StartTime, To = appointment2.EndTime, AllDay = appointment1.IsAllDay, Notes = appointment2.Notes, EventName = appointment2.Subject, ID = (int)appointment2.Id };

            App.Database.SaveSchedulerAppointmentAsync(editAppointment);
            App.Database.SaveSchedulerAppointmentAsync(editAppointment1);


        }

        private void DeleteSchedulerAppointment()
        {
            if (appointment == null)
            {
                this.IsOpen = false;
                return;
            }

            //// Remove the appointments in the Scheduler.
            Appointments?.Remove(this.appointment);
            //// Delete appointment in the database
            var deleteAppointment = new Appointment() { From = appointment.StartTime, To = appointment.EndTime, AllDay = appointment.IsAllDay, Notes = appointment.Notes, EventName = appointment.Subject, ID = (int)appointment.Id };
            App.Database.DeleteSchedulerAppointmentAsync(deleteAppointment);
            this.IsOpen = false;
        }

        private void CancelEdit()
        {
            this.IsOpen = false;
        }

        private async void AddAppointmentDetails()
        {
            var endDate = AppointmentEditorModel?.EndDate;
            var startDate = AppointmentEditorModel?.StartDate;
            var endTime = AppointmentEditorModel?.EndTime;
            var startTime = AppointmentEditorModel?.StartTime;

            if (endDate < startDate)
            {
                await DisplayAlert("", "End date should be greater than start date", "OK");
            }
            else if (endDate == startDate)
            {
                if (endTime <= startTime)
                {
                       await DisplayAlert("", "End time should be greater than start time", "OK");
                }
                else
                {
                    AppointmentDetails();
                }
            }
            else
            {
                AppointmentDetails();
            }
        }
        private void AppointmentDetails()
        {
            if (AppointmentEditorModel.Subject == null || AppointmentEditorModel.Notes == null)
                return;

            if (appointment == null)
            {
                appointment = new SchedulerAppointment();
                appointment.Subject = AppointmentEditorModel.Subject;
                appointment.StartTime = AppointmentEditorModel.StartDate.Date.Add(AppointmentEditorModel.StartTime);
                appointment.EndTime = AppointmentEditorModel.EndDate.Date.Add(AppointmentEditorModel.EndTime);
                appointment.IsAllDay = AppointmentEditorModel.IsAllDay;
                appointment.Notes = AppointmentEditorModel.Notes;

                if (this.Appointments == null)
                {
                    this.Appointments = new ObservableCollection<SchedulerAppointment>();
                }

                appointment.Id = Appointments.Count;
                //// Add the appointments in the Scheduler.
                Appointments.Add(appointment);
            }
            else
            {
                appointment.Subject = AppointmentEditorModel.Subject;
                appointment.StartTime = AppointmentEditorModel.StartDate.Date.Add(AppointmentEditorModel.StartTime);
                appointment.EndTime = AppointmentEditorModel.EndDate.Date.Add(AppointmentEditorModel.EndTime);
                appointment.IsAllDay = AppointmentEditorModel.IsAllDay;
                appointment.Notes = AppointmentEditorModel.Notes;
            }

            SaveSchedulerAppointmentAsync();

            this.IsOpen = false;
        }

        private void SaveSchedulerAppointmentAsync()
        {
            //// - add or edit the appointment in the database collection
            if(appointment ==  null) { return; }
            var editAppointment = new Appointment() { From = appointment.StartTime, To = appointment.EndTime, AllDay = appointment.IsAllDay, Notes = appointment.Notes, EventName = appointment.Subject, ID = (int)appointment.Id };
            App.Database.SaveSchedulerAppointmentAsync(editAppointment);
        }

        internal void UpdateEditor(SchedulerAppointment appointment, DateTime selectedDate)
        {
            this.appointment = appointment;
            this.selectedDate = selectedDate;
            if (this.appointment != null)
            {
                AppointmentEditorModel.Subject = this.appointment.Subject;
                AppointmentEditorModel.Notes = this.appointment.Notes;
                AppointmentEditorModel.StartDate = this.appointment.StartTime;
                AppointmentEditorModel.EndDate = this.appointment.EndTime;
                AppointmentEditorModel.IsEditorEnabled = true;

                if (!this.appointment.IsAllDay)
                {
                    AppointmentEditorModel.StartTime = new TimeSpan(this.appointment.StartTime.Hour, this.appointment.StartTime.Minute, this.appointment.StartTime.Second);
                    AppointmentEditorModel.EndTime = new TimeSpan(this.appointment.EndTime.Hour, this.appointment.EndTime.Minute, this.appointment.EndTime.Second);
                    AppointmentEditorModel.IsAllDay = false;
                    AppointmentEditorModel.IsEditorEnabled = true;
                }
                else
                {
                    AppointmentEditorModel.StartTime = new TimeSpan(12, 0, 0);
                    AppointmentEditorModel.EndTime = new TimeSpan(12, 0, 0);
                    AppointmentEditorModel.IsEditorEnabled = false;
                    AppointmentEditorModel.IsAllDay = true;
                }
            }
            else
            {
                AppointmentEditorModel.Subject = "";
                AppointmentEditorModel.Notes = "";
                AppointmentEditorModel.IsAllDay = false;
                AppointmentEditorModel.StartDate = this.selectedDate;
                AppointmentEditorModel.StartTime = new TimeSpan(this.selectedDate.Hour, this.selectedDate.Minute, this.selectedDate.Second);
                AppointmentEditorModel.EndDate = this.selectedDate;
                AppointmentEditorModel.EndTime = new TimeSpan(this.selectedDate.Hour + 1, this.selectedDate.Minute, this.selectedDate.Second);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Displays an alert dialog to the user.
        /// </summary>
        /// <param name="title">The title of the alert dialog.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="cancel">The text for the cancel button.</param>
        /// <returns>A task representing the asynchronous alert display operation.</returns>
        private Task DisplayAlert(string title, string message, string cancel)
        {
            return App.Current?.Windows?[0]?.Page!.DisplayAlert(title, message, cancel)
                   ?? Task.FromResult(false);
        }
    }
}
