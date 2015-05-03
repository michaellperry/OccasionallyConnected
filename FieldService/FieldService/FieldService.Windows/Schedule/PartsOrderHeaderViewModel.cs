using FieldService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Schedule
{
    public class PartsOrderHeaderViewModel
    {
        private readonly PartsOrder _partsOrder;

        public PartsOrderHeaderViewModel(PartsOrder partsOrder)
        {
            _partsOrder = partsOrder;            
        }

        public string Description
        {
            get { return _partsOrder.Description; }
        }

        public bool Received
        {
            get { return _partsOrder.OrderReceived; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var that = (PartsOrderHeaderViewModel)obj;
            return this._partsOrder == that._partsOrder;
        }

        public override int GetHashCode()
        {
            return _partsOrder.GetHashCode();
        }
    }
}
