using FieldService.Models;
using RoverMob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Schedule
{
    class ScheduleViewModel
    {
        private readonly Application<Technician> _application;
        private readonly VisitSelection _selection;
        
        public ScheduleViewModel(
            Application<Technician> application,
            VisitSelection selection)
        {
            _application = application;
            _selection = selection;
        }

        public string Technician
        {
            get { return "Michael"; }
        }

        public IEnumerable<VisitHeaderViewModel> Visits
        {
            get
            {
                return
                    from v in _application.Root.Visits
                    select new VisitHeaderViewModel(v);
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
    }
}
