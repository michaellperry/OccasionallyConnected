using FieldService.Models;
using RoverMob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Common;

namespace FieldService.Schedule
{
    class ScheduleViewModel
    {
        private readonly Application<Technician> _application;
        private readonly VisitSelection _selection;
        private readonly AuthenticationManager _authenticationManager;
        
        public ScheduleViewModel(
            Application<Technician> application,
            VisitSelection selection,
            AuthenticationManager authenticationManager)
        {
            _application = application;
            _selection = selection;
            _authenticationManager = authenticationManager;
        }

        public void Begin()
        {
            _authenticationManager.Authenticate();
        }

        public string Technician
        {
            get { return "Michael"; }
        }

        public IEnumerable<VisitHeaderViewModel> Visits
        {
            get
            {
                if (_application.Root != null)
                    return
                        from v in _application.Root.Visits
                        select new VisitHeaderViewModel(v);
                else
                    return new List<VisitHeaderViewModel>();
            }
        }

        public VisitHeaderViewModel SelectedVisit
        {
            get
            {
                if (_selection.SelectedVisit == null)
                    return null;
                else
                    return new VisitHeaderViewModel(
                        _selection.SelectedVisit);
            }
            set
            {
                if (value == null)
                    _selection.SelectedVisit = null;
                else
                    _selection.SelectedVisit = value.Visit;
            }
        }

        public VisitDetailViewModel VisitDetail
        {
            get
            {
                if (_selection.SelectedVisit == null)
                    return null;
                else
                    return new VisitDetailViewModel(
                        _selection.SelectedVisit);
            }
        }

        public bool Busy
        {
            get { return _authenticationManager.Busy; }
        }

        public string LastException
        {
            get
            {
                Exception exception = _authenticationManager.Exception ??
                    _application.Exception;
                if (exception == null)
                    return null;
                else
                    return exception.Message;
            }
        }
    }
}
