using Assisticant;
using FieldService.Models;
using FieldService.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoverMob;

namespace FieldService
{
    class ViewModelLocator : ViewModelLocatorBase
    {
        private Application<Technician> _application;
        private VisitSelection _selection;

        public ViewModelLocator()
        {
            if (DesignMode)
                _application = Initializer.LoadDesignModeApplication();
            else
                _application = Initializer.LoadApplication();

            _selection = new VisitSelection();
            _selection.SelectedVisit = _application.Root.Visits.FirstOrDefault();
        }

        public object Schedule
        {
            get
            {
                return ViewModel(() => new ScheduleViewModel(
                    _application,
                    _selection));
            }
        }
    }
}
