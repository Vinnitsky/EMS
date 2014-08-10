using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace WoaW.TMS.Tasks
{
    public enum EWorkEffortClass
    {
        Undefined = 0,
        //an activity can be broken down into tasks 
        Activity = 1,
        // represents a singe part of work
        Task = 2
    }

    public sealed class WorkEffortType : INotifyPropertyChanged
    {
        #region attributes
        private string _title;
        private string _description;
        private EWorkEffortClass _class;
        #endregion

        #region properties
        public string Id { get; private set; }
        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title)
                    return;

                _title = value;
                RaisePropertyChanged();
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description)
                    return;

                _description = value;
                RaisePropertyChanged();
            }
        }
        public EWorkEffortClass Class
        {
            get { return _class; }
            set
            {
                if (value == _class)
                    return;

                _class = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region constructors
        public WorkEffortType()
        {
            Id = System.Guid.NewGuid().ToString();
        }
        public WorkEffortType(string aTitle, string anId = null)
            : this()
        {
            Id = anId;
            Title = aTitle;
        }

        #endregion

        #region INotifyPropertyChanged implementation
        private void RaisePropertyChanged([CallerMemberName] string aPropertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(aPropertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

}
