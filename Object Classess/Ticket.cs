using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventTicketSystem.System_Classes;

namespace EventTicketSystem
{
    /// <summary>
    /// Ticket Object represent tickets in an event with the same price
    /// It stores the Event object, the ticket price, and how many tickets
    /// </summary>
    class Ticket
    {
        #region Local variables
        protected Event evt; //Represent the corresponding event
        protected int quantity; // Represent the quantity of the ticket
        protected double price; // Represent the price of the ticket
        #endregion

        #region Properties
        public Event Evt { get { return evt; } set { evt = value; } }
        public int Quantity {
            get { return quantity; }
            set { quantity = (value > 0) ? value : 0; } //prevent value to be set lower than 0
        }
        public double Price
        {
            get { return price; }
            set {
                double _value = (value > 0.01) ? value : 0.01; //prevent quantity to be set lower than 0.01
                
                price = Mathc.ConvertToDecimalPlace(_value, 2, Mathc.RoundingPreference.RoundUp); //Convert the price into 2 decimal places (Round Up)
            } 
        }
        #endregion

        #region Methods
        public Ticket (double price, int quantity)
        {
            this.Price = price;
            this.Quantity = quantity;
        }
        #endregion
    }
}
