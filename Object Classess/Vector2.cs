using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketSystem
{
    /// <summary>
    /// Stores a pair of double numbers, used for representing a pair coodinates
    /// </summary>
    class Vector2
    {
        #region Local variables

        #endregion

        #region Properties
        public double x { get; set; }
        public double y { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">value of x</param>
        /// <param name="y">value of y</param>
        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 zero { get {return new Vector2(0,0); } }

        //Get absolute values of x and y
        public Vector2 Abs { get { return new Vector2(Math.Abs(x), Math.Abs(y)); } }

        public override string ToString()
        {
            return String.Format("{0},{1}", x, y);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //Operator overload methods

        public static Vector2 operator +  (Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2 operator - (Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2 operator * (Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }

        public static Vector2 operator * (Vector2 v1, double number)
        {
            return new Vector2(v1.x * number, v1.y * number);
        }

        public static Vector2 operator / (Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x / v2.x, v1.y / v2.y);
        }

        public static bool operator == (Vector2 v1, Vector2 v2)
        {
            return (v1.x == v2.x && v1.y == v2.y);
        }

        public static bool operator != (Vector2 v1, Vector2 v2)
        {
            return (v1.x != v2.x || v1.y != v2.y);
        }
        #endregion
    }
}
