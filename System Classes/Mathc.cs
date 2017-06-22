using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketSystem.System_Classes
{
    /// <summary>
    /// Custom mathematical methods additional to System.Math
    /// </summary>
    class Mathc
    {
        static Random random; //Random variable

        public enum distanceMethod { Manhattan, Euclidean }; //The enum of distance method being used
        public enum RoundingPreference { Standard,RoundDown, RoundUp}; // The enum of rounding preferences
        /// <summary>
        /// Return a random value (double) within a range (min and max)
        /// </summary>
        /// <param name="min">Minimum</param>
        /// <param name="max">Maximum</param>
        /// <returns>A random number (double)</returns>
        public static double RandomRange(double min, double max)
        {
            //Initiate the random variable if it is null
            if(ReferenceEquals(random,null)) random = new Random(System.DateTime.Now.Millisecond);
            //Return the random number (Get the next random number (0.0 to 1.0), multiply the range and add the minimum)
            return random.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Return a random value (double) within a range (Range variable)
        /// </summary>
        /// <param name="range">The range</param>
        /// <returns>A random number (double)</returns>
        public static double RandomRange(Range range)
        {
            //Initiate the random variable if it is null
            if (ReferenceEquals(random, null)) random = new Random(System.DateTime.Now.Millisecond);
            //Return the random number (Get the next random number (0.0 to 1.0), multiply the range and add the minimum)
            return random.NextDouble() * (range.max - range.min) + range.min;
        }

        /// <summary>
        /// Calculate and return the distance (double) between two Vector2
        /// </summary>
        /// <param name="v1">The First coodinates (Vector2)</param>
        /// <param name="v2">The Second coodinates (Vector2)</param>
        /// <param name="DistanceMethod">The calculation method: Euclidean distance (Straight line distance) or Manhattan distance (City blocks distance)</param>
        /// <returns>The distance (double) between two Vector2</returns>
        public static double Distance(Vector2 v1, Vector2 v2, distanceMethod DistanceMethod = distanceMethod.Manhattan)
        {
            switch (DistanceMethod)
            {
                //Calculate the Euclidean distance (Straight line distance) between 2 Vector2 in absolute value
                case distanceMethod.Euclidean:
                    return Math.Sqrt(Math.Pow(Math.Abs(v1.x - v2.x), 2) + Math.Pow(Math.Abs(v1.y - v2.y), 2)); //Equation of Euclidean distance

                //Calculate the Manhattan distance (City blocks distance) between 2 Vector2 in absolute value
                case distanceMethod.Manhattan:
                default:
                    return Math.Abs(v1.x - v2.x) + Math.Abs(v1.y - v2.y); //Equation of Manhttan distance
            }

        }

        /// <summary>
        /// Convert a value (double) to X decimal places (Ceillings the number)
        /// </summary>
        /// <param name="value">The value being converted</param>
        /// <param name="decimalPlace">The target decimal places</param>
        /// <param name="rounding">Rounding preferences</param>
        /// <returns></returns>
        public static double ConvertToDecimalPlace(double value, int decimalPlace = 2, RoundingPreference rounding = RoundingPreference.Standard)
        {
            //The number of 10^X (eg. For 2 decimal places: 10^2 = 10 * 10 = 100 ) 
            int convert = (int)Math.Pow(10, decimalPlace);

            //Get the value before decimal point (Integer)
            int temp1 = (int)value; 

            //Get the value after decimal point
            double temp2 = value - temp1; //Get the remaining value (eg. 12.34567 - 12 = 0.34567)
            temp2 = temp2 * convert; //Multiplying the value by 10^X (eg. 0.34567 * 100)

            //Round the value to an 'integer'
            switch (rounding)
            {
                //Standard Rounding (Add 1 to temp2 if the remain value >= 0.5)
                case RoundingPreference.Standard:
                    temp2 = Math.Round(temp2);
                    break;
                //Round up (Add 1 to temp2 and ignores the remaining value)
                case RoundingPreference.RoundUp:
                    temp2 = Math.Ceiling(temp2);          //Ceiling it to an integer  (34.567 => 35)
                    break;
                //Round down (Ignores the remaining value)
                case RoundingPreference.RoundDown:
                    temp2 = Math.Floor(temp2);            //Flooring it to an integer  (34.567 => 34)
                    break;
            }

            temp2 = temp2 / convert; //Get the value as an integer, and devided by 100 to convert it to 2 digits number (eg. 35/100 = 0.35)

            //Adding up 2 values (before and after) to get the price in 2 decimal
            return temp1 + temp2; 
        }

        /// <summary>
        /// Convert the values in a range (min, max) to X decimal places (Ceillings the number)
        /// </summary>
        /// <param name="range">The range being converted</param>
        /// <param name="decimalPlace">The target decimal places</param>
        /// <param name="rounding">Rounding preferences</param>
        /// <returns></returns>
        public static Range ConvertToDecimalPlace(Range range, int decimalPlace = 2, RoundingPreference rounding = RoundingPreference.Standard)
        {
            return new Range(ConvertToDecimalPlace(range.min, decimalPlace, rounding), ConvertToDecimalPlace(range.max, decimalPlace, rounding), range.rangeType);
        }

        /// <summary>
        /// Convert the values in a Vector2 (x, y) to X decimal places (Ceillings the number)
        /// </summary>
        /// <param name="vector2">The Vector2 being converted</param>
        /// <param name="decimalPlace">The target decimal places</param>
        /// <param name="rounding">Rounding preferences</param>
        /// <returns></returns>
        public static Vector2 ConvertToDecimalPlace(Vector2 vector2, int decimalPlace = 2, RoundingPreference rounding = RoundingPreference.Standard)
        {
            return new Vector2(ConvertToDecimalPlace(vector2.x, decimalPlace,rounding), ConvertToDecimalPlace(vector2.y,decimalPlace,rounding));
        }

        /// <summary>
        /// Clamp a value within a range
        /// </summary>
        /// <param name="value">The value to be clamp</param>
        /// <param name="range">The range</param>
        /// <param name="rounding">Rounding preferences</param>
        /// <returns></returns>
        public static double Clamp (double value, Range range)
        {
            if (value < range.min) value = range.min;
            if (value > range.max) value = range.max;
            return value;
        }

    }
}
