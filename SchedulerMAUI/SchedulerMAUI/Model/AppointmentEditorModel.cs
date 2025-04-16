using System.ComponentModel;

namespace SchedulerMAUI
{
    public class AppointmentEditorModel : INotifyPropertyChanged
    {
        private string? subject , notes;
        private TimeSpan startTime , endTime;
        private bool isAllDay , isEditorEnabled= true;
        private DateTime startDate, endDate;

        public string? Subject
        {
            get { return subject; }
            set
            {
                subject = value;
                RaisePropertyChanged(nameof(this.Subject));
            }
        }

        public string? Notes
        {
            get { return notes; }
            set
            {
                notes = value;
                RaisePropertyChanged(nameof(this.Notes));
            }
        }

        public TimeSpan StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                RaisePropertyChanged(nameof(this.StartTime));
            }
        }

        public TimeSpan EndTime
        {
            get { return endTime; }
            set 
            {
                endTime = value;
                RaisePropertyChanged(nameof(this.EndTime));
            }
        }


        public bool IsAllDay
        {
            get { return isAllDay; }
            set
            {
                isAllDay = value;
                RaisePropertyChanged(nameof(this.IsAllDay));
            }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value; 
                RaisePropertyChanged(nameof(this.StartDate));
            }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set 
            {
                endDate = value;
                RaisePropertyChanged(nameof(this.EndDate));
            }
        }

        public bool IsEditorEnabled
        {
            get { return isEditorEnabled; }
            set 
            {
                isEditorEnabled = value;
                RaisePropertyChanged(nameof(this.IsEditorEnabled));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void RaisePropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
