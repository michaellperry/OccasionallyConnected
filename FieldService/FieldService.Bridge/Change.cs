using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Bridge
{
    public class Change<T>
    {
        public ChangeOperation Operation { get; set; }
        public T Record { get; set; }
    }
}
