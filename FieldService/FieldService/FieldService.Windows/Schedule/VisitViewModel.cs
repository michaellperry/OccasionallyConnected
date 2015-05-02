using FieldService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Schedule
{
    public class VisitViewModel
    {
        private readonly Visit _visit;

        public VisitViewModel(Visit visit)
        {
            _visit = visit;            
        }

        public string Time
        {
            get
            {
                return String.Format("{0:t} - {1:t}", _visit.StartTime, _visit.EndTime);
            }
        }

        public string Address
        {
            get
            {
                return _visit.Incident.Home.Address
                    .OrderBy(a => a.MessageHash)
                    .Select(a => a.Value)
                    .FirstOrDefault();
            }
        }

        public string Incident
        {
            get
            {
                return _visit.Incident.Description
                    .OrderBy(d => d.MessageHash)
                    .Select(d => d.Value)
                    .FirstOrDefault();
            }
        }
    }
}
