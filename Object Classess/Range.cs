using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketSystem
{
    class Range
    {
        #region Local variables

        #endregion

        #region Properties
        public double min { get; set;}
        public double max { get; set;}
        
        //Represent the size of the range
        //The size of an Axis range +1
        public double size { get { return Math.Abs(max - min) + ((rangeType == RangeType.Axis)? 1: 0); }} 

        public enum RangeType { Numbers, Axis } //The type of the range
        public RangeType rangeType { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="min">Minimum Value</param>
        /// <param name="max"><Maximum Value</param>
        /// <param name="rangeType">The Range Type</param>
        public Range(double min, double max, RangeType rangeType = RangeType.Numbers)
        {
            if (max < min) max = min;

            this.min = min;
            this.max = max;
            this.rangeType = rangeType;
        }

        public override string ToString()
        {
            return String.Format("{0} to {1}", min, max);
        }

        /// <summary>
        /// Check a value is within this range
        /// </summary>
        /// <param name="value"> The value</param>
        /// <param name="excludeMinAndMax">True if the value is not allowed to be equal to min and max value</param>
        /// <returns></returns>
        public bool WithinRange(double value, bool excludeMinAndMax = false)
        {
            if (excludeMinAndMax) return (value >= min && value <= max);
            else return (value >= min && value <= max);
        }


        //Operator overload methods
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator == (Range r1, Range r2)
        {
            return (r1.min == r2.min && r1.min == r2.max);
        }

        public static bool operator !=(Range r1, Range r2)
        {
            return (r1.min != r2.min || r1.max != r2.max);
        }
        #endregion
    }
}

