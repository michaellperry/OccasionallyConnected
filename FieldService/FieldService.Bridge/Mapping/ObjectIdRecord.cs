using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Bridge.Mapping
{
    public class ObjectIdRecord
    {
        public string TypeName { get; set; }
        public int Id { get; set; }
        public Guid ObjectId { get; set; }
    }
}
