using FieldService.Models;
using RoverMob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Schedule
{
    public class VisitDetailViewModel
    {
        private readonly Application<Technician> _application;
        private readonly Visit _visit;

        public VisitDetailViewModel(
            Application<Technician> application,
            Visit visit)
        {
            _application = application;
            _visit = visit;            
        }

        public string Address
        {
            get
            {
                return _visit.Incident.Home.Address
                    .OrderBy(c => c.MessageHash)
                    .Select(c => c.Value)
                    .FirstOrDefault();
            }
        }

        public string Time
        {
            get
            {
                return String.Format("{0:t} to {1:t}",
                    _visit.StartTime, _visit.EndTime);
            }
        }

        public string Incident
        {
            get
            {
                return _visit.Incident.Description
                    .OrderBy(c => c.MessageHash)
                    .Select(c => c.Value)
                    .FirstOrDefault();
            }
        }

        public IEnumerable<PartsOrderHeaderViewModel> PartsOrders
        {
            get
            {
                return
                    from p in _visit.Incident.PartsOrders
                    select new PartsOrderHeaderViewModel(p);
            }
        }

        public void CreateOutcome()
        {
            _application.EmitMessage(_visit.CreateOutcome());
        }
    }
}
