using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Bridge
{
    public class HomeRecord
    {
        public int HomeId { get; private set; }
        public string Address { get; private set; }

        public static HomeRecord FromDataRow(DataRow row)
        {
            return new HomeRecord
            {
                HomeId = row.GetInt32("HomeId"),
                Address = row.GetString("Address")
            };
        }
    }
}
