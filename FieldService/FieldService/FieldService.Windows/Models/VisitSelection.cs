using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Models
{
    public class VisitSelection
    {
        private Observable<Visit> _selectedVisit =
            new Observable<Visit>();

        public Visit SelectedVisit
        {
            get { return _selectedVisit; }
            set { _selectedVisit.Value = value; }
        }
    }
}
