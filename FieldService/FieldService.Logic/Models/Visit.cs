using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Models
{
    public class Visit
    {
        private readonly string _visitId;
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;

        public Visit(string visitId, DateTime startTime, DateTime endTime)
        {
            _visitId = visitId;
            _startTime = startTime;
            _endTime = endTime;
        }

        public DateTime StartTime
        {
            get { return _startTime; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
        }
    }
}
