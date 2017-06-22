using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketSystem.System_Classes
{
    /// <summary>
    /// Methods to sort events and tickets data, and find the nearest and cheapest tickets
    /// </summary>
    class TicketFinder
    {
        #region local variables
        protected static int ticketsToFind = 5; //The maximum number of tickets the Ticket Finder show
        #endregion

        #region Properties
        public static int TicketsToFind
        {
            get { return ticketsToFind; }
            set { ticketsToFind = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Find a number of closest events to a pair of coodinates and show their cheapest tickets
        /// </summary>
        /// <param name="coodinates">The specific coodinate</param>
        /// <returns></returns>
        public static List<Ticket> BestAvailableTickets(Vector2 coodinates)
        {
            //Create the list of tickets
            List<Ticket> bestTickets = new List<Ticket>();

            //Get the list of sorted events in distance
            List<Event> sortedEvents = SortedEventsInDistance(World.Events, coodinates);

            //Create counter
            int i = 0;

            //Repeat finding until the found enough tickets (bestTickets.Count == ticketsToFind) or no more events available
            while (bestTickets.Count<ticketsToFind && i < sortedEvents.Count)
            {
                //Filter out events with 0 tickets
                if(sortedEvents[i].TotalTicketsAvailable>0)
                //Add the cheapest Ticket (The first Ticket in the sorted tickets of that event in price) to the bestTickets list
                bestTickets.Add(SortedTicketsInPrice(sortedEvents[i].Tickets)[0]);

                //Add the counter by 1
                i++;
            }           
            //Return the list of best tickets
            return bestTickets;
        }

        /// <summary>
        /// Sort and returns a list of events based on the distance between the event and the coodinates (Accending order)
        /// </summary>
        /// <param name="events">The list of events to sort</param>
        /// <param name="coodinates">The specific coodinate</param>
        /// <returns></returns>
        public static List<Event> SortedEventsInDistance(List<Event> events, Vector2 coodinates)
        {
            List<Event> _events = events; //Copy the list

            //Sort list by comparing distance between inputed location and each event's location
            _events.Sort(
                delegate (Event e1, Event e2)
                {
                    //Compare the distance (lower first)
                    int compare = Mathc.Distance(e1.Location, coodinates).CompareTo(Mathc.Distance(e2.Location, coodinates));

                    //If distance are the same, compare their lowest ticket price (lower first)
                    if (compare == 0)
                    {
                        //If both of them have 0 tickets, return 0 (No change in order)
                        if (e1.Tickets.Count < 1 && e2.Tickets.Count < 1) compare = 0;
                        //If e1 has 0 tickets, return 1 (e1 increase the index)
                        else if(e1.Tickets.Count < 1) compare = 1;
                        //If e2 has 0 tickets, return -1 (e1 reduce the index)
                        else if (e2.Tickets.Count < 1) compare = -1;
                        //If e1 and e2 both have tickets, compare their ticket price
                        else compare = SortedTicketsInPrice(e1.Tickets)[0].Price.CompareTo(SortedTicketsInPrice(e2.Tickets)[0].Price);
                    }
                    return compare;
                });
            return _events;
        }

        /// <summary>
        /// Sort and returns a list of tickets based on the price (Accending order)
        /// </summary>
        /// <param name="tickets">The list of tickets to sort</param>
        /// <returns></returns>
        public static List<Ticket> SortedTicketsInPrice(List<Ticket> tickets)
        {
            List<Ticket> _tickets = tickets;

            //Compare the price(lower first)
            _tickets.Sort(
                delegate (Ticket t1, Ticket t2)
                {
                    return t1.Price.CompareTo(t2.Price);
                });
            return _tickets;
        }
        #endregion
    }
}
