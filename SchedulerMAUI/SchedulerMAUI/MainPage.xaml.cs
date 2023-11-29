using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;

namespace SchedulerMAUI
{
    public partial class MainPage : ContentPage
    {
        private SchedulerAppointment appointment;
        private DateTime selectedDate;
        public AppointmentEditorModel AppointmentEditorModel { get; set; }
        public MainPage()
        {
            InitializeComponent();
            this.Scheduler.DisplayDate = DateTime.Today.Date.AddHours(8);
            this.AppointmentEditorModel = new AppointmentEditorModel();
            this.sfPopup.BindingContext = this;
        }

        private void Scheduler_Tapped(object sender, SchedulerTappedEventArgs e)
        {
            if (e.Appointments != null && e.Appointments.Count > 0)
            {
                appointment = (SchedulerAppointment)e.Appointments[0];
                selectedDate = appointment.StartTime;
            }
            else
            {
                appointment = null;
                selectedDate = (DateTime)e.Date;
            }
            this.UpdateEditor();
            sfPopup.Show();
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (appointment == null)
            {
                this.sfPopup.IsOpen = false;
                return;
            }

            ((ObservableCollection<SchedulerAppointment>)this.Scheduler.AppointmentsSource).Remove(this.appointment);
            var deleteAppointment = new Appointment() { From = appointment.StartTime, To = appointment.EndTime, AllDay = appointment.IsAllDay, Notes = appointment.Notes, EventName = appointment.Subject, ID = (int)appointment.Id };
            App.Database.DeleteSchedulerAppointmentAsync(deleteAppointment);
            this.sfPopup.IsOpen = false;
        }

        private void SwitchAllDay_Toggled(object sender, ToggledEventArgs e)
        {
            if (AppointmentEditorModel.IsAllDay)
            {
                AppointmentEditorModel.StartTime = new TimeSpan(0, 0, 0);
                AppointmentEditorModel.IsEditorEnabled = false;
                AppointmentEditorModel.EndTime = new TimeSpan(0, 0, 0);
            }
            else
            {
                AppointmentEditorModel.IsEditorEnabled = true;
            }
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            this.sfPopup.IsOpen = false;
        }
        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            var endDate = AppointmentEditorModel.EndDate;
            var startDate = AppointmentEditorModel.StartDate;
            var endTime = AppointmentEditorModel.EndTime;
            var startTime = AppointmentEditorModel.StartTime;

            if (endDate < startDate)
            {
                Application.Current.MainPage.DisplayAlert("", "End date should be greater than start date", "OK");
            }
            else if (endDate == startDate)
            {
                if (endTime <= startTime)
                {
                    Application.Current.MainPage.DisplayAlert("", "End time should be greater than start time", "OK");
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
            if (appointment == null)
            {
                appointment = new SchedulerAppointment();
                appointment.Subject = AppointmentEditorModel.Subject;
                appointment.StartTime = AppointmentEditorModel.StartDate.Date.Add(AppointmentEditorModel.StartTime);
                appointment.EndTime = AppointmentEditorModel.EndDate.Date.Add(AppointmentEditorModel.EndTime);
                appointment.IsAllDay = AppointmentEditorModel.IsAllDay;
                appointment.Notes = AppointmentEditorModel.Notes;

                if (this.Scheduler.AppointmentsSource == null)
                {
                    this.Scheduler.AppointmentsSource = new ObservableCollection<SchedulerAppointment>();
                }

                appointment.Id = (this.Scheduler.AppointmentsSource as ObservableCollection<SchedulerAppointment>).Count;

                ((ObservableCollection<SchedulerAppointment>)this.Scheduler.AppointmentsSource).Add(appointment);
            }
            else
            {
                appointment.Subject = AppointmentEditorModel.Subject;
                appointment.StartTime = AppointmentEditorModel.StartDate.Date.Add(AppointmentEditorModel.StartTime);
                appointment.EndTime = AppointmentEditorModel.EndDate.Date.Add(AppointmentEditorModel.EndTime);
                appointment.IsAllDay = AppointmentEditorModel.IsAllDay;
                appointment.Notes = AppointmentEditorModel.Notes;
            }

            //// - add or edit the appointment in the database collection
            var editAppointment = new Appointment() { From = appointment.StartTime, To = appointment.EndTime, AllDay = appointment.IsAllDay, Notes = appointment.Notes, EventName = appointment.Subject, ID = (int)appointment.Id };
            App.Database.SaveSchedulerAppointmentAsync(editAppointment);

            this.sfPopup.IsOpen = false;
        }
        private void UpdateEditor()
        {
            if (appointment != null)
            {
                AppointmentEditorModel.Subject = appointment.Subject;
                AppointmentEditorModel.Notes = appointment.Notes;
                AppointmentEditorModel.StartDate = appointment.StartTime;
                AppointmentEditorModel.EndDate = appointment.EndTime;
                AppointmentEditorModel.IsEditorEnabled = true;

                if (!appointment.IsAllDay)
                {
                    AppointmentEditorModel.StartTime = new TimeSpan(appointment.StartTime.Hour, appointment.StartTime.Minute, appointment.StartTime.Second);
                    AppointmentEditorModel.EndTime = new TimeSpan(appointment.EndTime.Hour, appointment.EndTime.Minute, appointment.EndTime.Second);
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
                AppointmentEditorModel.StartDate = selectedDate;
                AppointmentEditorModel.StartTime = new TimeSpan(selectedDate.Hour, selectedDate.Minute, selectedDate.Second);
                AppointmentEditorModel.EndDate = selectedDate;
                AppointmentEditorModel.EndTime = new TimeSpan(selectedDate.Hour + 1, selectedDate.Minute, selectedDate.Second);
            }
        }
    }
}

