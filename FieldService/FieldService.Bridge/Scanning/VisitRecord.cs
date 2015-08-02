using FieldService.Bridge.Utility;
using System;

namespace FieldService.Bridge.Scanning
{
    public class VisitRecord
    {
        public int VisitId { get; private set; }
        public int TechnicianId { get; private set; }
        public int IncidentId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public static VisitRecord FromDataRow(DataRow row)
        {
            return new VisitRecord
            {
                VisitId = row.GetInt32("VisitId"),
                TechnicianId = row.GetInt32("TechnicianId"),
                IncidentId = row.GetInt32("IncidentId"),
                StartTime = row.GetDateTime("StartTime"),
                EndTime = row.GetDateTime("EndTime")
            };
        }
    }
}
