using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;

namespace SchedulerMAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.Scheduler.DisplayDate = DateTime.Today.Date.AddHours(8);
        }

        private void Scheduler_Tapped(object sender, SchedulerTappedEventArgs e)
        {
            if (e.Element == SchedulerElement.Header) return;

            if (this.BindingContext is SchedulerViewModel schedulerViewModel)
            {
                SchedulerAppointment appointment;
                DateTime selectedDate;

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
                schedulerViewModel.UpdateEditor(appointment, selectedDate);
                sfPopup.Show();
            }
        }

        private void SwitchAllDay_Toggled(object sender, ToggledEventArgs e)
        {
            if (this.BindingContext is SchedulerViewModel schedulerViewModel)
            {
                var appointmentEditorModel = schedulerViewModel.AppointmentEditorModel;

                if (appointmentEditorModel == null)
                {
                    return;
                }

                if (appointmentEditorModel.IsAllDay)
                {
                    appointmentEditorModel.StartTime = new TimeSpan(0, 0, 0);
                    appointmentEditorModel.IsEditorEnabled = false;
                    appointmentEditorModel.EndTime = new TimeSpan(0, 0, 0);
                }
                else
                {
                    appointmentEditorModel.IsEditorEnabled = true;
                }
            }
        }
    }
}

