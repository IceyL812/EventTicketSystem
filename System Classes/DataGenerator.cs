using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketSystem.System_Classes
{
    /// <summary>
    /// Handles Events and Tickets Data Generation
    /// </summary>
    class DataGenerator
    {
        #region Local variables
        protected static Range eventAmountRange; //Represents the range of how many events are generated
        protected static Range ticketAmountRange; //Represents the range of how many tickets are generated
        protected static Range ticketPriceRange; //Represents the range of ticket price
        #endregion

        #region Properties
        //Properties to set or get variable value
        public static Range EventAmountRange { get { return eventAmountRange; } set { eventAmountRange = value; } }
        public static Range TicketAmountRange { get { return ticketAmountRange; } set { ticketAmountRange = value; } }
        public static Range TicketPriceRange { get { return ticketPriceRange; } set { ticketPriceRange = value; } }

        //Properties to get calculated value
        public static Range EventAmountRangeAdjusted
        {
            get
            {
                //Set the current world capcacity of events under integer coordinate system
                int limit = World.RemainingEventCapacity;

                //Set the adjusted range (Auto adjusted and cannot be changed by user) by limiting the non-adjusted range(Can be set by user)
                double min = Math.Min(eventAmountRange.min, limit);
                double max = Math.Min(eventAmountRange.max, limit);
                return new Range(min, max);
            }
        } //Returns the adjusted event generation range (Fits the world capacity)

        #endregion

        #region Methods
        //Constructor
        public DataGenerator()
        {
            //Initiates the variables
            EventAmountRange = new Range(30, 500);
            TicketAmountRange = new Range(0, 1500);
            TicketPriceRange = new Range(0.01, 300);
            //Generate Events and Tickets Data at start up
            GenerateData();
        }

        /// <summary>
        /// Generates Events and Tickets Data
        /// </summary>
        /// <param name="eventsToGenerate">A user specified number of evnets being generated. Assign a negative number for a random quantity.</param>
        /// <param name="clearAllData">True: Wipe all the exsiting events and tickets data</param>
        public static void GenerateData(int eventsToGenerate = -1, bool clearAllData = false)
        {
            List<Event> temp; //A dummy variable to let GenerateData(out temp, clearAllData); output the value
            GenerateData(out temp, eventsToGenerate, clearAllData);
        }

        /// <summary>
        /// Generates Events and Tickets Data, then return the list of events that have been generated
        /// </summary>
        /// <param name="eventsGenerated">Output the list of events had been generated</param>
        /// <param name="eventsToGenerate">A user specified number of evnets being generated. Assign a negative number for a random quantity.</param>
        /// <param name="clearAllData">True: Wipe all the exsiting events and tickets data</param>

        public static void GenerateData(out List<Event> eventsGenerated, int eventsToGenerate = -1, bool clearAllData = false)
        {
            if (clearAllData)
            {
                //Clear all events and tickets data in the corresponding lists in World
                World.Events.Clear();
                World.Tickets.Clear();
                //Reset the next event id variable
                World.NextEventId = 0;
            }

            //Quantity of events being generated
            int eventsQuantity; 

            //Check if the 'eventsToGenerate' (user set) has a genuine value (greater than 0)
            if (eventsToGenerate > 0)
            {
                //Apply the number and limit it to the remaining capcity of the world
                eventsQuantity= (int)Math.Min(eventsToGenerate, World.RemainingEventCapacity);
            }
            else //User has not specified a number, randomize the quantity to generate
            {
                //Set the quantity range of events generation
                Range generateRange = EventAmountRangeAdjusted;
                //Randomize the quantity of events being generated (int) from the range
                eventsQuantity = (int)Mathc.RandomRange(generateRange);
            }

            //Generate the events with the random value and output it
            eventsGenerated = GenerateEvents(eventsQuantity);
            //Add the events that have been generated
            World.AddEvents(eventsGenerated);
        }

        /// <summary>
        /// Generates and returns a single event in a specific location
        /// </summary>
        /// <param name="location">The specific location the event being generated</param>
        /// <returns>A single Event object</returns>
        public static Event GenerateEvent(Vector2 location)
        {
            Event evt = new Event(location); // Create a new event object
            int totalTickets = (int)Mathc.RandomRange(ticketAmountRange); //Randomize the quantity of total ticket
            evt.AddTickets(GenerateTickets(totalTickets, TicketPriceRange)); //Generate a list of Ticket objects and pass it to the Event object
            return evt;
        }

        /// <summary>
        /// Generates and return a list of events
        /// </summary>
        /// <param name="eventQuantity">The total quantity of events being generated</param>
        /// <returns>A list of Event Objects</returns>
        public static List <Event> GenerateEvents(int eventQuantity)
        {
            //Create a list of events to store the generated events
            List<Event> _events = new List<Event>();

            for(int i = 0; i < eventQuantity; i++)
            {
                 //Create a new event and add to the events list
                _events.Add(GenerateEvent(Randomcoordinates(_events)));
            }
            return _events;
        }

        /// <summary>
        /// Generates and return a list of Ticket objects with random quantities and price
        /// </summary>
        /// <param name="totalTickets">The total quanity of tickets (NOT a Ticket object)</param>
        /// <param name="priceRange">The price range of tickets</param>
        /// <returns>A list of tickets</returns>
        static List<Ticket> GenerateTickets(int totalTickets, Range priceRange)
        {
            //Create a new list to store Ticket objects for the event
            List<Ticket> _tickets = new List<Ticket>(); 

            //Generate Ticket objects until the totalTickets is 0
            while (totalTickets > 0) 
            {
                //Represent the quantity of tickets being generated
                int ticketQuantity = (int)Mathc.RandomRange(1, totalTickets);
                //Randomize the price with the range
                double ticketPrice = Mathc.RandomRange(priceRange);
                //Generate a new Ticket Object
                _tickets.Add(new Ticket(ticketPrice, ticketQuantity));
                //Reduce the totalTickets value
                totalTickets -= ticketQuantity; 
            }
            return _tickets; //Return the whole list
        }

        /// <summary>
        ///Generate and return Random coordinates 
        /// </summary>
        /// <param name="GeneratedEvents"></param>
        /// <returns>A random and valid coordinates</returns>
        public static Vector2 Randomcoordinates(List<Event> GeneratedEvents = null)
        {
            Vector2 coordinates;     
            //Repeat generating coordinates until it is valid
            do
            {
                double x;
                double y;
                switch (World.CoordinateSystem) {
                    default:
                    case World.coordinateSystem.Integer:
                        //Randomize the x and y value within the world axis range
                        //Adding 1 to the max value are exclusive when convert to int (Unless the random is exactly 1 which is impossible)
                        x = Mathc.RandomRange(World.AxisX.min, World.AxisY.max + 1);
                        y = Mathc.RandomRange(World.AxisY.min, World.AxisY.max + 1);
                        //Create a new coordinates(Vector2)
                        coordinates = new Vector2(Math.Floor(x), Math.Floor(y));
                        break;
                    //Randomize the x and y value within the world axis range
                    case World.coordinateSystem.Decimal:
                        x = Mathc.RandomRange(World.AxisX);
                        y = Mathc.RandomRange(World.AxisY);
                        //Convert the coordinates into decimal places and create a new coordinates (Vector2)
                        coordinates = new Vector2(Mathc.ConvertToDecimalPlace(x,World.DecimalPlacesForDecimalCoordinateSystem), Mathc.ConvertToDecimalPlace(y,World.DecimalPlacesForDecimalCoordinateSystem));
                        break;
                      }

            } while (!validCoordinates(coordinates, GeneratedEvents)); //Check if the coordinates are valid, repeat if invalid

            return coordinates;
        }

        /// <summary>
        /// Method to check if coordinates are valid (not repeated)
        /// </summary>
        /// <param name="coord">The coordinates (Vector2) to be checked</param>
        /// <param name="GenerateEvents">List of events which have been generated</param>
        /// <returns>True if the coordinates are not repeated</returns>
        public static bool validCoordinates(Vector2 coord, List<Event> GenerateEvents = null)
        {
            //Instatiate the repeated counter
            int repeated = 0;
            //Loop through all the Event objects stored in the World
            foreach (Event evt in World.Events) 
            {
                //Check if the generated coord is the same
                if (evt.Location == coord) 
                {
                    //Add 1 to the counter
                    repeated++;
                    //Check if repeated is already greater or equals to the max limit
                    if (repeated >= World.MaxEventAmountInSamecoord) 
                    {
                        //Return false and break loop
                        return false; 
                    }
                }        
            }

            if (!ReferenceEquals(GenerateEvents, null))
            {
                //Loop through all the Event objects stored in the list of events generated
                foreach (Event evt in GenerateEvents) 
                {
                    //Check if the generated coord is the same
                    if (evt.Location == coord) 
                    {
                        //Add 1 to the counter
                        repeated++;
                        //Check if repeated is already greater or equals to the max limit
                        if (repeated >= World.MaxEventAmountInSamecoord) 
                        {
                            return false; //Return false and break loop
                        }
                    }
                }
            }
            return true; //Return true (valid) if the value is less than the max limit
        }
        #endregion
    }
}
