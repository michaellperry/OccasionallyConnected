using FieldService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Schedule
{
    public class VisitHeaderViewModel
    {
        private readonly Visit _visit;

        public VisitHeaderViewModel(Visit visit)
        {
            _visit = visit;
        }

        public Visit Visit
        {
            get { return _visit; }
        }

        public string Time
        {
            get
            {
                return string.Format("{0:t} to {1:t}",
                    _visit.StartTime,
                    _visit.EndTime);
            }
        }

        public string Address
        {
            get
            {
                return _visit.Incident.Home.Address
                    .OrderBy(d => d.MessageHash)
                    .Select(d => d.Value)
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var that = (VisitHeaderViewModel)obj;
            return this._visit == that._visit;
        }

        public override int GetHashCode()
        {
            return _visit.GetHashCode();
        }
    }
}
