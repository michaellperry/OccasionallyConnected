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

        public ScheduleViewModel(Application<Technician> application)
        {
            _application = application;
        }

        public string Technician
        {
            get { return "Michael"; }
        }
    }
}
