using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoverMob.Messaging;

namespace FieldService.Bridge.Mapping
{
    public class MessageHashRecord
    {
        public string TypeName { get; set; }
        public string PropertyName { get; set; }
        public int Id { get; set; }
        public string Value { get; set; }
        public MessageHash MessageHash { get; set; }
    }
}
