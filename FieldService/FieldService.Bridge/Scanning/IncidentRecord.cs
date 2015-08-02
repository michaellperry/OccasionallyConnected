using FieldService.Bridge.Utility;
using System;
namespace FieldService.Bridge.Scanning
{
    public class IncidentRecord
    {
        public int IncidentId { get; private set; }
        public int HomeId { get; private set; }
        public string Description { get; private set; }

        public static IncidentRecord FromDataRow(DataRow row)
        {
            return new IncidentRecord
            {
                IncidentId = row.GetInt32("IncidentId"),
                HomeId = row.GetInt32("HomeId"),
                Description = row.GetString("Description")
            };
        }
    }
}
