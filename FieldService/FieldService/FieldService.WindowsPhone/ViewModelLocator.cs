using Assisticant;
using FieldService.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService
{
    class ViewModelLocator : ViewModelLocatorBase
    {
        public object Schedule
        {
            get { return ViewModel(() => new ScheduleViewModel()); }
        }
    }
}
