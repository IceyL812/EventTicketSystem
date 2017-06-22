using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicketSystem
{
    /// <summary>
    /// An Event which stores the location of the event and all Tickets for this event (in different prices)
    /// </summary>
    class Event
    {
        #region Local variables

        protected int id; //Unique identifier of the event
        protected Vector2 location; //Vector2 represents the location
        protected List<Ticket> tickets; //List of tickets which stores tickets
        protected int totalTicketsAvailable; //The number of all available tickets
        #endregion

        #region Properties
        public int ID {
            get { return id; }
            set { id = value; }
        }
        public Vector2 Location {
            get { return location; }
        }
        public List<Ticket> Tickets
        {
            get { return tickets; }
            set { tickets = value; }
        }
        public int TotalTicketsAvailable {
            get { return totalTicketsAvailable; }
            set { totalTicketsAvailable = (value > 0) ? value : 0; } //prevent value < 0
        }
        #endregion

        #region Methods
        //Constructor
        public Event (Vector2 location)         
        {
            this.location = location;
            tickets = new List<Ticket>();
        }

        /// <summary>
        /// Add a single Ticket object to the list
        /// </summary>
        /// <param name="ticket">Ticket object</param>
        public void AddTicket(Ticket ticket)
        {
            //Add the quantity of total tickets available
            TotalTicketsAvailable += ticket.Quantity;

            //Checks if there is a ticket with the same price
            foreach (Ticket t in tickets) { 
                if (t.Price == ticket.Price)
                {
                    //Add the quantity of the ticket to the exist ticket
                    t.Quantity += ticket.Quantity; 
                    return;
                }
            }

            //Add the new ticket into list if a same price ticket does not exist
            ticket.Evt = this;
            this.tickets.Add(ticket); 
        }

        /// <summary>
        /// Add a list of Ticket objects to the list
        /// </summary>
        /// <param name="ticket">A list of Ticket objects</param>
        public void AddTickets(List<Ticket> _tickets)
        {
            foreach (Ticket t in _tickets) AddTicket(t);
        }
        #endregion
    }
}
