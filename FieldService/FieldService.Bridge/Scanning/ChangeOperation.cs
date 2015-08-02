using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Bridge.Scanning
{
    public enum ChangeOperation
    {
        Delete,
        Insert,
        UpdateOldValue,
        UpdateNewValue
    }
}
