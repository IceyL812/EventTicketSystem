﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketSystem.System_Classes
{
    /// <summary>
    /// Handles the coordinates system, and store all the events and tickets data
    /// </summary>
    class World
    {
        #region Local variables
        protected static Range axisX; // The size of the world in Vector2
        protected static Range axisY; // The size of the world in Vector2
        protected static int maxEventAmountInSamecoord; //The maximum quantity of events a pair of coordinates can hold
        protected static List<Event> events; // A list of Event object
        protected static List<Ticket> tickets; //A list of Tickets (inside all the events)
        protected static int nextEventId; //The id for the next event being added to list
        protected static int decimalPlacesForDecimalCoordinateSystem; //The decimal places for the decimal coordinate system
        public enum coordinateSystem
        {
            Integer,
            Decimal
        } //Enum for coordinate System
        #endregion

        #region Properties
        //Properties for set and get variables
        public static Range AxisX
        {
            get { return axisX; }
            set { axisX = value;} 
        }
        public static Range AxisY
        {
            get { return axisY; }
            set { axisY = value; }
        }
        public static int MaxEventAmountInSamecoord { get { return maxEventAmountInSamecoord; } set { maxEventAmountInSamecoord = (value > 1) ? value : 1; } }
        public static List<Event> Events { get { return events; } set { events = value; } }
        public static List<Ticket> Tickets { get { return tickets; } set { tickets = value; } }
        public static int NextEventId { get { return nextEventId; } set { nextEventId = value; } }

        public static coordinateSystem CoordinateSystem { get; set; }
        public static int DecimalPlacesForDecimalCoordinateSystem {
            get { return decimalPlacesForDecimalCoordinateSystem; }
            set
            {
                Range clampRange = new Range(1, 8);
                decimalPlacesForDecimalCoordinateSystem =  (int)Mathc.Clamp(value, clampRange); }
            }

        //Properties for get a calculated value
        public static Vector2 MidPoint
        {
            get
            {
                double x = (AxisX.max + AxisX.min) / 2;
                double y = (AxisY.max + axisY.min) / 2;
                if (CoordinateSystem == coordinateSystem.Integer)
                {
                    x = (int)x;
                    y = (int)y;
                }
                else
                {
                    x = Mathc.ConvertToDecimalPlace(x, decimalPlacesForDecimalCoordinateSystem);
                    y = Mathc.ConvertToDecimalPlace(y, decimalPlacesForDecimalCoordinateSystem);
                }
                return new Vector2(x, y);
            }
        } //The middle point of the world

        public static int TotalEvents { get { return events.Count; } } //The number of total events in the world
        public static int TotalTickets
        {
            get
            {
                int temp = 0;
                foreach (Ticket t in tickets)
                {
                    temp += t.Quantity;
                }
                return temp;
            }
        } //The number of total tickets quantity in the world
        public static Range PriceRangeOfAllEventTickets
        {
            get
            {
                List<Ticket> sortedTickets = TicketFinder.SortedTicketsInPrice(World.Tickets);
                return new Range(sortedTickets[0].Price, sortedTickets[sortedTickets.Count - 1].Price);
            }
        } //The price range of all event tickets

        public static int MaxEventCapacity
        {
            get
            {
                //The Maximum world capcacity of events under integer coordinate system
                int maxEventCapacity = (int)(World.AxisX.size * World.AxisY.size * World.MaxEventAmountInSamecoord);
                //Multiply 10^(Decimal Places) to get the real limit under decimal coordinate system
                if (World.CoordinateSystem == World.coordinateSystem.Decimal) maxEventCapacity *= (int)Math.Pow(10, World.DecimalPlacesForDecimalCoordinateSystem);

                return maxEventCapacity;
            }
        } //Returns the maximum world capcacity of events
        public static int RemainingEventCapacity {
            get { return MaxEventCapacity - TotalEvents; } } //Returns the remaining world capacity (How many more events can be hold)

        #endregion

        #region Methods
        //Constructor
        public World()
        {
            axisX = new Range(-10, 10, Range.RangeType.Axis);
            axisY = new Range(-10, 10, Range.RangeType.Axis);
            maxEventAmountInSamecoord = 1;
            CoordinateSystem = coordinateSystem.Integer;
            DecimalPlacesForDecimalCoordinateSystem = 4;
            events = new List<Event>();
            tickets = new List<Ticket>();
            nextEventId = 0;
        }

        /// <summary>
        /// Add a single Event object to List
        /// </summary>
        /// <param name="evt">Event object to be added</param>
        public static void AddEvent(Event evt)
        {
            //Set the id variable inside the Event object
            evt.ID = nextEventId;
            //Add the event to the list
            events.Add(evt); 

            //Check if the event contains any tickets
            if (evt.Tickets.Count > 0)
            {
                foreach(Ticket t in evt.Tickets)
                {
                    //Add the Ticket object inside the event to the tickets list
                    tickets.Add(t);
                }
            }
            nextEventId++; //increase the nextEventId
        }

        /// <summary>
        /// Add a list of Event objects to List
        /// </summary>
        /// <param name="_events">A list of Event objects to be added</param>
        public static void AddEvents(List<Event> _events)
        {
            foreach (Event e in _events) AddEvent(e); 
        }
        #endregion
    }
}
