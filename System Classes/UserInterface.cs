using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventTicketSystem.System_Classes;

namespace EventTicketSystem
{
    /// <summary>
    /// Handles User Interfaces (Menu, Prompt, etc.)
    /// </summary>
    class UserInterface
    {
        #region Local variables
        StringBuilder sb;
        #endregion

        #region Properties
        bool showDataSummary { get; set; }
        bool showCurrentSettings { get; set; }
        #endregion

        #region Methods - Menus
        /// <summary>
        /// Main menu UI
        /// </summary>
        void mainMenu()
        {
            Console.Clear();
            if(showCurrentSettings)sb.Append(currentSettings());
            if(showDataSummary)sb.Append(dataSummary());

            sb.AppendLine("===============Main Menu=============");
            sb.AppendLine("1) Ticket Finder");
            sb.AppendLine("2) World Settings");
            sb.AppendLine("3) Data Generator Settings");
            sb.AppendFormat("4) {0} Current Application Settings\n", (showCurrentSettings)? "Hide":"Show");
            sb.AppendFormat("5) {0} Data Summary\n", (showDataSummary) ? "Hide" : "Show");

            sb.AppendLine("\nPlease choose your option from above: ");

            Console.WriteLine(sb.ToString());
            sb.Clear();

            //Process Option
            switch (getOptionInput(5)) {
                case 1:
                    ticketFinderMenu();
                    break;
                case 2:
                    worldSettings();
                    break;
                case 3:
                    dataGeneratorSettings();
                    break;
                case 4:
                    showCurrentSettings = !showCurrentSettings;
                    mainMenu();
                    break;
                case 5:
                    showDataSummary = !showDataSummary;
                    mainMenu();
                    break;
            }       
        }

        /// <summary>
        /// TicketFinder UI
        /// </summary>
        void ticketFinderMenu()
        {
            Console.Clear();
            if (showCurrentSettings) sb.Append(currentSettings());
            if (showDataSummary) sb.Append(dataSummary());

            //Display text
            sb.AppendLine("===============Tickets Finder=============");
            sb.AppendFormat("This Ticket Finder can find you the cheapest ticket of up to {0} nearest events of a specific position\n", TicketFinder.TicketsToFind);
            sb.AppendFormat("Please insert a coordinates within [X-Axis:({0}) Y-Axis:({1})] , with this format: x,y):\n", World.AxisX, World.AxisY);
            if (World.CoordinateSystem == World.coordinateSystem.Integer) sb.AppendLine("Only Integer Numbers are allowed under Integer coordinate System");
            else sb.AppendFormat("Numbers with more than {0} decimal places would be automatically celling to {0} decimal places\n", World.DecimalPlacesForDecimalCoordinateSystem);
            sb.AppendLine("You can also insert 'cancel' to go back to main menu");

            Console.WriteLine(sb.ToString());
            sb.Clear();

            //Process Input
            string input = Console.ReadLine();
            Vector2 location;
            string errorMsg;
            while (!validLocationInput(input, World.CoordinateSystem == World.coordinateSystem.Integer, out location, out errorMsg))
            {
                Console.WriteLine("Input invalid ({0}).\nPlease reinsert a correct value: ", errorMsg);
                input = Console.ReadLine();
            }

            //Check if the user typed cancel
            if(validCancel(input))
            {
                mainMenu();
            }
            else //Find tickets
            {
                //Convert the location to appropiate decimal places
                if(World.CoordinateSystem == World.coordinateSystem.Decimal) location = Mathc.ConvertToDecimalPlace(location,World.DecimalPlacesForDecimalCoordinateSystem);

                //Get the list of the ticket list
                List<Ticket> ticketlist = TicketFinder.BestAvailableTickets(location);

                //Formats
                string headerformat = "|{0,10}|\t{1,20}|\t{2,10}|\t{3,15}|\t{4,15}|\n";
                string valueformat = "|{0,10}|\t{1,20}|\t{2,10}|\t{3,15}|\t{4,16}|\n";

                //Print headers
                sb.AppendFormat("\nHere are {0} nearest events to ({1}) and their cheapest tickets\n\n", ticketlist.Count, location);
                sb.Append("-------------------------------------------------------------------------------------------------\n");
                sb.AppendFormat(headerformat, "Event ID", "coordinates", "Distance", "Ticket Price", "Ticket Quanitity");
                sb.Append("-------------------------------------------------------------------------------------------------\n");

                //Print each event and ticket value
                foreach (Ticket t in ticketlist)
                {
                    int id = t.Evt.ID;
                    Vector2 coord = t.Evt.Location;
                    double distance = Mathc.Distance(coord, location);
                    //Convert the distance to appropiate decimal places if coordinate system is 'decimal' (Double vairables)
                    if (World.CoordinateSystem == World.coordinateSystem.Decimal) distance = Mathc.ConvertToDecimalPlace(distance, World.DecimalPlacesForDecimalCoordinateSystem);
                    double price = t.Price;
                    int quantity = t.Quantity;
                    sb.AppendFormat(valueformat, id, "(" + coord+ ")", distance, price.ToString("C", new CultureInfo("en-US")), quantity);
                }
                sb.Append("-------------------------------------------------------------------------------------------------\n");
                Console.WriteLine(sb.ToString());
                sb.Clear();
                //Ask user if they are trying to find another ticket
                Console.WriteLine("1) Find another ticket");
                Console.WriteLine("2) Back to main menu");

                switch (getOptionInput(2))
                {
                    case 1:
                        ticketFinderMenu();
                        break;
                    case 2:
                        mainMenu();
                        break;
                }
            }          
        }

        /// <summary>
        /// World Settings UI
        /// </summary>
        void worldSettings()
        {
            Console.Clear();
            if (showCurrentSettings) sb.Append(currentSettings());
            if (showDataSummary) sb.Append(dataSummary());

            sb.AppendLine("===============World Settings=============");
            sb.AppendFormat("1) Switch coordinate System to use {0} numbers\n",(World.CoordinateSystem == World.coordinateSystem.Integer) ? "Decimal" : "'Integer'");
            sb.AppendLine("2) Change the decimal places for coordinate System with Decimal Numbers");
            sb.AppendLine("3) Change World coordinate Range");
            sb.AppendLine("4) Change Maximum events a location could hold");
            sb.AppendLine("5) Change the Maximum tickets that Ticket Finder show");
            sb.AppendLine("6) Back to Main Menu");

            sb.AppendLine("\nPlease choose your option from above: ");

            Console.WriteLine(sb.ToString());
            sb.Clear();

            switch (getOptionInput(6))
            {
                case 1:
                    changeWorldcoordSystem();
                    break;
                case 2:
                    changeDecimalPlacesForDecimalcoordSystem();
                    break;
                case 3:
                    changeWorldcoordRange();
                    break;
                case 4:
                    changeMaximumEventsInSameLocation();
                    break;
                case 5:
                    changeMaximumTicketsToFind();
                    break;
                case 6:
                    mainMenu();
                    break;
            }         
        }

        /// <summary>
        /// Data generator UI
        /// </summary>
        void dataGeneratorSettings()
        {
            Console.Clear();
            if (showCurrentSettings) sb.Append(currentSettings());
            if (showDataSummary) sb.Append(dataSummary());

            sb.AppendLine("===============Data Generator Settings=============");
            sb.AppendLine("1) Generate additional Events and Tickets data");
            sb.AppendLine("2) Clear and Regenerate All Events and Tickets data");
            sb.AppendLine("3) Change the quantity range of events generation");
            sb.AppendLine("4) Change the quantity range of tickets generation");
            sb.AppendLine("5) Change the price range of tickets generation");
            sb.AppendLine("6) Add one event at a specific location");
            sb.AppendLine("7) Back to Main Menu");

            sb.AppendLine("\nPlease choose your option from above: ");

            Console.WriteLine(sb.ToString());
            sb.Clear();

            switch (getOptionInput(7))
            {
                case 1:
                    generateData(false);
                    break;
                case 2:
                    generateData(true);
                    break;
                case 3:
                    changeEventQuantityRange();
                    break;
                case 4:
                    changeTicketQuantityRange();
                    break;
                case 5:
                    changeTicketPriceRange();
                    break;
                case 6:
                    addCustomEvent();
                    break;
                case 7:
                    mainMenu();
                    break;
            }
        }
        #endregion

        #region Methods - Info & Settings

        /// <summary>
        /// Show all the summary of all events and tickets data in the World
        /// </summary>
        /// <returns>Events and tickets data in the World</returns>
        string dataSummary()
        {
            StringBuilder tempsb = new StringBuilder();

            tempsb.AppendLine("=============Data Summary==============");
            tempsb.AppendFormat("Total Events in the World: {0}\n", World.TotalEvents);
            tempsb.AppendFormat("Total Tickets in the World: {0}\n", World.TotalTickets);

            List<Event> sortedEvents = TicketFinder.SortedEventsInDistance(World.Events, World.MidPoint);
            tempsb.AppendFormat("Nearest event to the Middle point ({0}): Event ID: {1} at ({2})\n", World.MidPoint,sortedEvents[0].ID, sortedEvents[0].Location);
            tempsb.AppendFormat("Farthest event from the Middle point ({0}): Event ID: {1} at ({2})\n", World.MidPoint, sortedEvents[sortedEvents.Count - 1].ID, sortedEvents[sortedEvents.Count - 1].Location);

            List<Ticket> sortedTickets = TicketFinder.SortedTicketsInPrice(World.Tickets);
            tempsb.AppendFormat("Lowest ticket price of all events: {0} ({1} ticket(s))\n", sortedTickets[0].Price.ToString("C", new CultureInfo("en-US")), sortedTickets[0].Quantity);
            tempsb.AppendFormat("Highest ticket price of all events: {0} ({1} ticket(s))\n\n", sortedTickets[sortedTickets.Count - 1].Price.ToString("C", new CultureInfo("en-US")), sortedTickets[sortedTickets.Count - 1].Quantity);

            return tempsb.ToString();
        }

        /// <summary>
        /// Show all the application settings
        /// </summary>
        /// <returns>Application settings</returns>
        string currentSettings()
        {
            StringBuilder tempsb = new StringBuilder();
            tempsb.AppendLine("===========Current Application Setting============");
            tempsb.AppendLine("World Settings:");
            tempsb.AppendFormat("World coordinate system: {0} numbers\n", (World.CoordinateSystem == World.coordinateSystem.Integer) ? "Integer" : "Decimal");
            tempsb.AppendFormat("Decimal places for coordinate system using Decimal Numbers: {0}\n", World.DecimalPlacesForDecimalCoordinateSystem);
            tempsb.AppendFormat("World coordinate range: [X-Axis:({0}) Y-Axis:({1})]\n", World.AxisX, World.AxisY);
            tempsb.AppendFormat("Maximum events a location could hold: {0}\n", World.MaxEventAmountInSamecoord);
            tempsb.AppendFormat("World Capacity of Events: Max: {0}, Currently remaining: {1} \n", World.MaxEventCapacity, World.RemainingEventCapacity);
            tempsb.AppendFormat("Maximum tickets that Ticket Finder shows: {0}\n", TicketFinder.TicketsToFind);

            tempsb.AppendLine("\nRandom Data Generator Settings:");
            tempsb.AppendFormat("Quantity range of Events (Before adjustment): {0}\n", DataGenerator.EventAmountRange);
            tempsb.AppendFormat("Quantity range of Events (After adjustment to fit world capacity): {0}\n", DataGenerator.EventAmountRangeAdjusted);
            tempsb.AppendFormat("Quantity range of tickets in each event: {0}\n", DataGenerator.TicketAmountRange);
            tempsb.AppendFormat("Price range of the tickets: {0}\n\n", DataGenerator.TicketPriceRange);
            return tempsb.ToString();
        }


        //World Settings

        /// <summary>
        /// Switching the coordinate system between 'Integer' and 'Decimal'
        /// </summary>
        void changeWorldcoordSystem()
        {
            Console.WriteLine("Changing coordinate System Mode requires regenerating all the events and tickets data.");

            //Check if the event amount range would be automatically adjusted
            if (DataGenerator.EventAmountRange == DataGenerator.EventAmountRangeAdjusted)
            {
                Console.WriteLine("The quantity range of event generation would be automatically adjusted from ({0}) to ({1})",
                    DataGenerator.EventAmountRange, DataGenerator.EventAmountRangeAdjusted);
            }

            //Prompt for Y or N
            Console.WriteLine("Do you wish to continue? (Y/N)");
            if (getYOrN())
            {
                World.CoordinateSystem = (World.CoordinateSystem == World.coordinateSystem.Integer) ? World.coordinateSystem.Decimal : World.coordinateSystem.Integer;
    
                //Clear and regenerate all the data (Events and tickets)
                DataGenerator.GenerateData(clearAllData: true);

                Console.WriteLine("World coordinate System has been successfully changed to {0} numbers", (World.CoordinateSystem == World.coordinateSystem.Integer) ? "Integer" : "Decimal");
                Console.WriteLine("All events and tickets data has been regenerated");
                Console.WriteLine("(Press [Enter] To Continue)");
                Console.ReadLine();
                //Go back to world settings menu
                worldSettings();
            }
            else
            {
                //Show Cancel Text
                cancelAction();
                //Go back to world settings menu
                worldSettings();
                return;
            }
        }

        /// <summary>
        /// Change the decimal places for decimal coordinate system
        /// </summary>
        void changeDecimalPlacesForDecimalcoordSystem()
        {
            Console.WriteLine("Please insert a integer number (1 to 8) for the new decimal places for the decimal coordinate system.");
            Console.WriteLine("You can also type 'cancel' to cancel this action.");
            string input = Console.ReadLine();
            int result;

            //Check if the input are valid integers and output the value to result
            bool isInt = int.TryParse(input, out result);
            //Check if the format is correct
            bool validInputFormat = !String.IsNullOrWhiteSpace(input) && (validCancel(input) || isInt);
            //Check if the additional requirement (result >= 1 and <= 8)
            bool additionalRequirement = validCancel(input);
            if (isInt) additionalRequirement = (result >=1 && result <= 8);

            //Repeat the process if either the format is invalid or failed to meet the additional requirement
            while (!validInputFormat || !additionalRequirement)
            {
                //Show error message
                string errorMsg = "";
                if (!isInt) errorMsg = "(Value has to be an integer number)";
                else if (result < 1 || result > 8) errorMsg = "(Value cannot be less than 1 or greater than 8)";

                Console.WriteLine("Input invalid{0}.\nPlease reinsert a correct value: ", errorMsg);

                //Ask the user to input again
                input = Console.ReadLine();

                //Reverify the arguments
                isInt = int.TryParse(input, out result);
                validInputFormat = !String.IsNullOrWhiteSpace(input) && (validCancel(input) || isInt);
                additionalRequirement = validCancel(input);
                if (isInt) additionalRequirement = (result >= 1 && result <= 8);
            }

            //Check if the input is 'Cancel')
            if (validCancel(input))
            {
                cancelAction();
                worldSettings();
                return;
            }
            else
            {
                Console.WriteLine("Changing the decimal places for the decimal coordinate system from {0} to {1}", World.DecimalPlacesForDecimalCoordinateSystem, result);
                
                if (World.CoordinateSystem == World.coordinateSystem.Decimal)
                {
                    Console.WriteLine("Changing the decimal places under decimal coordinate system requires regenerating all the events and tickets data.");
                }

                //Confirm the user's intention
                Console.WriteLine("Do you wish to continue? (Y/N)");
                if (getYOrN())
                {
                    World.DecimalPlacesForDecimalCoordinateSystem = (int)result;
                    Console.WriteLine("The decimal places for the decimal coordinate system has been changed to {0}", World.DecimalPlacesForDecimalCoordinateSystem);

                    //Regenerate all events and tickets data if under decimal coordinate system 
                    if (World.CoordinateSystem == World.coordinateSystem.Decimal)
                    {
                        DataGenerator.GenerateData(clearAllData: true);
                        Console.WriteLine("All events and tickets data has been regenerated");
                    }

                    Console.WriteLine("(Press [Enter] To Continue)");
                    Console.ReadLine();
                    worldSettings();
                }
                else
                {
                    cancelAction();
                    worldSettings();
                    return;
                }
            }
        }

        /// <summary>
        /// Change the range of the world coordinate system
        /// </summary>
        void changeWorldcoordRange()
        {
            //New ranges for X-axis and Y-axis
            Range newX;
            Range newY;

            //Set new range of X-Axis
            Console.WriteLine("Please insert the new range of X-Axis, following this format: min,max");
            if (World.CoordinateSystem == World.coordinateSystem.Integer) Console.WriteLine("Only Integer Numbers are allowed under Integer coordinate System");
            else Console.WriteLine("Numbers with more than {0} decimal places would be automatically converted to {0} decimal places", World.DecimalPlacesForDecimalCoordinateSystem);
            Console.WriteLine("You can also type 'cancel' to cancel this action.");

            string input = Console.ReadLine();
            string errorMsg;

            //Check if the format is valid and output the new X-Axis
            bool validFormat = validRangeFormat(input, World.CoordinateSystem == World.coordinateSystem.Integer, out newX, out errorMsg, Range.RangeType.Axis) || validCancel(input);
           
            //Check if the additional requirement has been made (Max value has to be greater than Min)
            bool additionalRequirement = validCancel(input);
            if (!ReferenceEquals(newX, null)) additionalRequirement = newX.min < newX.max;

            //Repeat asking users if not achieved
            while (!validFormat || !additionalRequirement) {
                if (!additionalRequirement && String.IsNullOrEmpty(errorMsg)) errorMsg = "Max value has to be greater than Min value";
                Console.WriteLine("Input invalid ({0}).\nPlease reinsert a correct value: ", errorMsg);
                input = Console.ReadLine();

                validFormat = validRangeFormat(input, World.CoordinateSystem == World.coordinateSystem.Integer, out newX, out errorMsg, Range.RangeType.Axis) || validCancel(input);
                if (!ReferenceEquals(newX, null)) additionalRequirement = newX.min < newX.max;
            }

            //Check if user typed cancel
            if (validCancel(input))
            {
                cancelAction();
                worldSettings();
                return;
            }

            //Set new range of Y-Axis
            Console.WriteLine("Please insert the new range of Y-Axis, following this format: min,max");
            if (World.CoordinateSystem == World.coordinateSystem.Integer) Console.WriteLine("Only Integer Numbers are allowed under Integer coordinate System");
            else Console.WriteLine("Numbers with more than {0} decimal places would be automatically converted to {0} decimal places", World.DecimalPlacesForDecimalCoordinateSystem);
            Console.WriteLine("You can also type 'cancel' to cancel this action.");

            input = Console.ReadLine();
            //Check if the format is valid and output the new X-Axis
            validFormat = validRangeFormat(input, World.CoordinateSystem == World.coordinateSystem.Integer, out newY, out errorMsg, Range.RangeType.Axis) || validCancel(input);
            additionalRequirement = validCancel(input);
            //Check if the additional requirement has been made (Max value has to be greater than Min)
            if (!ReferenceEquals(newY, null)) additionalRequirement = newY.min < newY.max;

            while (!validFormat || !additionalRequirement)
            {
                if (!additionalRequirement && String.IsNullOrEmpty(errorMsg)) errorMsg = "Max value has to be greater than Min value";
                Console.WriteLine("Input invalid ({0}).\nPlease reinsert a correct value: ", errorMsg);
                input = Console.ReadLine();
                //Reverify
                validFormat = validRangeFormat(input, World.CoordinateSystem == World.coordinateSystem.Integer, out newY, out errorMsg, Range.RangeType.Axis) || validCancel(input);
                additionalRequirement = false;
                if (!ReferenceEquals(newY, null)) additionalRequirement = newY.min < newY.max;
            }

            //Check if user typed cancel
            if (validCancel(input))
            {
                cancelAction();
                worldSettings();
                return;
            }

            //Convert the value to the appropiate decimal places under decimal coordinate system
            if (World.CoordinateSystem == World.coordinateSystem.Decimal)
            {
                newX = Mathc.ConvertToDecimalPlace(newX, World.DecimalPlacesForDecimalCoordinateSystem);
                newY = Mathc.ConvertToDecimalPlace(newY, World.DecimalPlacesForDecimalCoordinateSystem);
            }

            Console.WriteLine("Changing the world range from [X-Axis({0}) Y-Axis:({1})] to [X-Axis({2}) Y-Axis:({3})]",
                               World.AxisX, World.AxisY, newX, newY);

            //Check if any one of the axis has been reduced
            bool reducedaxis = (newX.max < World.AxisX.max || newX.min> World.AxisX.min || newY.max < World.AxisY.max || newY.min > World.AxisY.min);
      
            if (reducedaxis)
            {
                Console.WriteLine("Reducing one or more limit of any axis would delete every events and their tickets data outside the new range.");
            }

            Console.WriteLine("Do you wish to continue? (Y/N)");
            if (getYOrN())
            {
                //Apply the new axis
                World.AxisX = newX;
                World.AxisY = newY;

                if (reducedaxis)
                {
                    //Remove the events out of the range

                    //Create a list of event being removed
                    List<Event> eventsToBeRemoved = new List<Event>();
                    foreach (Event e in World.Events)
                    {
                        if (!World.AxisX.WithinRange(e.Location.x) || !World.AxisX.WithinRange(e.Location.y))
                        {
                            //Add the events being removed (Exception occurs when removing the events straight from here)
                            eventsToBeRemoved.Add(e);
                        }
                    }

                    //Remove events from the world which are included in the eventsToBeRemoved list
                    foreach (Event e in eventsToBeRemoved)
                    {
                        if (e.Tickets.Count > 0)
                        {
                            foreach(Ticket t in e.Tickets)
                            {
                                World.Tickets.Remove(t);
                            }
                        }
                        World.Events.Remove(e);
                    }

                    Console.WriteLine("{0} events out of the new world coordinates and their tickets data have been removed.", eventsToBeRemoved.Count);
                }

                Console.WriteLine("The world coordinates range have been changed to [X-Axis({0}) Y-Axis:({1})] successfully", World.AxisX, World.AxisY);
                Console.WriteLine("(Press [Enter] to continue)");
                Console.ReadLine();
                worldSettings();
                return;
            }
            else
            {
                cancelAction();
                worldSettings();
                return;
            }
        }
        
        /// <summary>
        /// Change the maximum events in the same location (coordination)
        /// </summary>
        void changeMaximumEventsInSameLocation()
        {
            Console.WriteLine("Please insert a integer number (Greater than 0) for the new maximum amount of events a location can hold.");
            Console.WriteLine("You can also type 'cancel' to cancel this action.");
            string input = Console.ReadLine();
            int result;

            //Check formats and additional requirements
            bool isInt = int.TryParse(input, out result);
            bool validInputFormat = !String.IsNullOrWhiteSpace(input) && (validCancel(input) || isInt);
            bool additionalRequirement = validCancel(input);
            if (isInt)  additionalRequirement = result > 0;

            //Repeat asking user if the input is invalid
            while (!validInputFormat || !additionalRequirement)             
            {
                string errorMsg="";
                if (!isInt) errorMsg = "(Value has to be an integer number)";
                else if (result < 1) errorMsg = "(Value cannot be less than 1)";
                
                Console.WriteLine("Input invalid{0}.\nPlease reinsert a correct value: ", errorMsg);
                input = Console.ReadLine();
                isInt = int.TryParse(input, out result);
                validInputFormat = !String.IsNullOrWhiteSpace(input) && (validCancel(input) || isInt);
                additionalRequirement = validCancel(input);
                if (isInt) additionalRequirement = result > 0;
            }

            //Check if the user typed Cancel
            if(validCancel(input))
            {
                cancelAction();
                worldSettings();
                return;
            }
            else
            {
                Console.WriteLine("Changing the maximum amount of events a location can hold from {0} to {1}", World.MaxEventAmountInSamecoord, result);

                //Create a Dictionary to store locations and events at the same location
                Dictionary<Vector2, int> _events = new Dictionary<Vector2, int>();
                //Create a list to store locations with events more than the new limit value
                List<Vector2> overCrowdedLocation = new List<Vector2>();

                if (result < World.MaxEventAmountInSamecoord)
                {
                    foreach(Event e in World.Events)
                    {
                        if(_events.Count>0 && _events.ContainsKey(e.Location))
                        {
                            _events[e.Location]++;
                            if (_events[e.Location] > World.MaxEventAmountInSamecoord) overCrowdedLocation.Add(e.Location);
                        }else
                        {
                            _events.Add(e.Location, 1);
                        }
                    }
                    //Warn user when there are 1 or more location(s) which holds events more than the new limit value
                    if (overCrowdedLocation.Count>0) Console.WriteLine("{0} location(s) has more than {1} events.\nThese events and tickets data have to be removed to proceed.", overCrowdedLocation.Count);
                }

                Console.WriteLine("Do you wish to continue? (Y/N)");
                if (getYOrN())
                {
                    //Apply value
                    World.MaxEventAmountInSamecoord = (int)result;

                    //Remove the repeated events
                    List<Event> eventsToBeRemoved = new List<Event>();
                    if (overCrowdedLocation.Count > 0)
                    {
                        foreach (Event e in World.Events)
                        {
                            foreach (Vector2 v in overCrowdedLocation)
                            {
                                if (e.Location == v)
                                {
                                    eventsToBeRemoved.Add(e);
                                }
                            }
                        }

                        foreach(Event e in eventsToBeRemoved)
                        {
                            if (e.Tickets.Count > 0)
                            {
                                foreach (Ticket t in e.Tickets)
                                {
                                    World.Tickets.Remove(t);
                                }
                            }
                            World.Events.Remove(e);
                        }
                        Console.WriteLine("{0} events and their tickets data have been removed", eventsToBeRemoved.Count);
                    }
                    Console.WriteLine("The maximum amount of events a location can hold has been changed to {0}", World.MaxEventAmountInSamecoord);
                    Console.WriteLine("(Press [Enter] to continue)");
                    Console.ReadLine();
                    worldSettings();
                    return;
                }
                else
                {
                    cancelAction();
                    worldSettings();
                    return;
                }
            }
        }
       
        /// <summary>
        /// Change the maximum tickets the Ticket Finder can find
        /// </summary>
        void changeMaximumTicketsToFind()
        {
            Console.WriteLine("Please insert a integer number (Greater than 0) for the new maximum number of tickets the Ticket Finder show.");
            Console.WriteLine("You can also type 'cancel' to cancel this action.");
            string input = Console.ReadLine();
            int result;
            //Check if format is valid and additional requirement has been met
            bool isInt = int.TryParse(input, out result);
            bool validInputFormat = !String.IsNullOrWhiteSpace(input) && (validCancel(input) || isInt);
            bool additionalRequirement = validCancel(input);
            if (isInt) additionalRequirement = result >= 1;
            //Repeat asking user if invalid input
            while (!validInputFormat || !additionalRequirement)
            {
                string errorMsg = "";
                if (!isInt) errorMsg = "(Value has to be an integer number)";
                else if (result < 1) errorMsg = "(Value cannot be less than 1)";

                Console.WriteLine("Input invalid{0}.\nPlease reinsert a correct value: ", errorMsg);
                input = Console.ReadLine();

                //Reverify
                isInt = int.TryParse(input, out result);
                validInputFormat = !String.IsNullOrWhiteSpace(input) && (validCancel(input) || isInt);
                additionalRequirement = validCancel(input);
                if (isInt) additionalRequirement = result >= 1;
            }
            //Check if the user typed 'Cancel'
            if(validCancel(input))
            {
                cancelAction();
                worldSettings();
                return;
            }
            else
            {
                Console.WriteLine("Changing the maximum number of tickets the Ticket Finder show from {0} to {1}", TicketFinder.TicketsToFind, (int)result);

                Console.WriteLine("Do you wish to continue? [Y/N]");
                if (getYOrN())
                {
                    TicketFinder.TicketsToFind = (int)result;
                    Console.WriteLine("The maximum number of tickets the Ticket Finder show has been changed to {0}", TicketFinder.TicketsToFind);
                    Console.WriteLine("Press [Enter] Key to continue");
                    Console.ReadLine();
                    worldSettings();
                }
                else
                {
                    cancelAction();
                    worldSettings();
                    return;
                }
            }
        }

        /// <summary>
        /// Call Data Generator to generate events and tickets data 
        /// </summary>
        /// <param name="clearAllData">True: Wipe all existing data; False: Keeps all exsiting data</param>
        void generateData(bool clearAllData)
        {

            if (clearAllData) //Clear all data before generating new data
            {
                Console.WriteLine("All the events and tickets data would be deleted and regenerated.");
            }
            else //Keep all data while generating new data
            {
                //Check if the world capacity is 0
                if(DataGenerator.EventAmountRangeAdjusted.max <= 0)
                {
                    //Cancel the action since no more room can fit
                    Console.WriteLine("Maximum world coordinates has been reached. Unable to generate more events.");
                    Console.WriteLine("Please try increasing the range of the world coordinates system, or the maximum number of a location can hold.");
                    cancelAction();
                    dataGeneratorSettings();
                }
                else
                {
                    Console.WriteLine("Additional events and tickets would be generated and previous data are not affected.");
                }
            }

            Console.WriteLine("Do you wish to continue? [Y/N]");
            if(getYOrN())
            {
                Console.WriteLine("Do you wish to specify the how many events are being generated? [Y/N]");
                Console.WriteLine("Otherwise the quanitity would be randomized");

                //The quantity of events being generate
                //A negative number as default which signals the data generator to generate a random number
                //User can specify a positive number for the quantity after inputting [Y]
                int eventQuantity = -1;

                if (getYOrN())  //The user would like to specify the quantity
                {
                    //Set the maximum value of data can be generated
                    int MaximumValue = (clearAllData) ? World.MaxEventCapacity : World.RemainingEventCapacity;

                    Console.WriteLine("Please insert a integer number (From 1 to {0}) for how many events are being generated.", MaximumValue);

                    Console.WriteLine("You can also type 'cancel' to cancel the whole process.");
                    string input = Console.ReadLine();
                    int result;
                    //Check if format is valid and additional requirement has been met
                    bool isInt = int.TryParse(input, out result);
                    bool validInputFormat = !String.IsNullOrWhiteSpace(input) && (validCancel(input) || isInt);
                    bool additionalRequirement = validCancel(input);
                    //Limit the input number to be >= 1 and <= the max capacity
                    if (isInt) additionalRequirement = result >= 1 && result <= MaximumValue;
                    
                    //Repeat asking user if invalid input
                    while (!validInputFormat || !additionalRequirement)
                    {
                        string errorMsg = "";
                        if (!validInt(result)) errorMsg = "Value has to be an integer number";
                        else if (result < 1 || result>DataGenerator.EventAmountRangeAdjusted.max) errorMsg = String.Format("Value has to between 1 to {0}", DataGenerator.EventAmountRangeAdjusted.max);

                        Console.WriteLine("Input invalid({0}).\nPlease reinsert a correct value: ", errorMsg);
                        input = Console.ReadLine();

                        //Reverify
                        isInt = int.TryParse(input, out result);
                        validInputFormat = !String.IsNullOrWhiteSpace(input) && (validCancel(input) || isInt);
                        additionalRequirement = validCancel(input);
                        if (isInt) additionalRequirement = result >= 1 && result <= MaximumValue;
                    }

                    //Check if the user inputed cancel
                    if (validCancel(input))
                    {
                        cancelAction();
                        dataGeneratorSettings();
                        return;
                    }
                    //Set the quantity of events being generated to the specified number
                    eventQuantity = result;
                }

                //Create a list to store Event object being generated
                List<Event> eventsGenerated;

                //Generate new data and output the list of events that have been generated
                //Clear all data before data generation if clearAllData is true
                DataGenerator.GenerateData(out eventsGenerated, eventQuantity, clearAllData);
                if(clearAllData) Console.WriteLine("All the previous events and tickets data have been deleted.");
                
                //Print how many events have been generated
                Console.WriteLine("{0} events have been generated.", eventsGenerated.Count);

                //Create a list of ticket
                List<Ticket> ticketsGenerated = new List<Ticket>();
                //Calculate how many tickets (Not Ticket objects) has been generated
                int totaltickets = 0;
                foreach(Event e in eventsGenerated)
                {
                    if (e.Tickets.Count > 0)
                    {
                        ticketsGenerated.AddRange(e.Tickets);
                        foreach (Ticket t in e.Tickets)
                        {
                            totaltickets += t.Quantity;
                        }
                    }
                }
                //Print how many tickets have been generated
                Console.WriteLine("{0} ticket(s) has been generated to these events", totaltickets);

                //Sort the tickets in price
                List<Ticket> sortedTickets = TicketFinder.SortedTicketsInPrice(ticketsGenerated);
                //Print the price range of those tickets
                Console.WriteLine("Tickets price for these events range from {0} to {1}", sortedTickets[0].Price.ToString("C", new CultureInfo("en-US")), sortedTickets[sortedTickets.Count - 1].Price.ToString("C", new CultureInfo("en-US")));
                Console.WriteLine("Press [Enter] Key to continue");
                Console.ReadLine();
                dataGeneratorSettings();
            }
            else
            {
                cancelAction();
                dataGeneratorSettings();
                return;
            }
        }

        /// <summary>
        /// Change the range of generating events (Before auto adjustment)
        /// </summary>
        void changeEventQuantityRange()
        {
            Console.WriteLine("Please insert the new range for the event quantities in integer numbers with the format: min,max");
            Console.WriteLine("You can also type 'cancel' to cancel this action.");

            string input = Console.ReadLine();
            Range result;
            string errorMsg;

            //Check if the format is valid and the additional requirement has been met
            bool validInputFormat = validRangeFormat(input, true, out result, out errorMsg) || validCancel(input);
            bool additionalRequirement = validCancel(input);
            if (!ReferenceEquals(result, null)) additionalRequirement = result.min >= 0 && result.max >= 0;

            //Repeat asking user if invalid
            while (!validInputFormat && !additionalRequirement)
            {
                if (!additionalRequirement && String.IsNullOrEmpty(errorMsg)) errorMsg = "Both values shoule be greater than or equal to 0";
                Console.WriteLine("Input invalid ({0}). Please insert a correct range with a correct format:", errorMsg);
                input = Console.ReadLine();

                //Reverify the arguments
                validInputFormat = validRangeFormat(input, true, out result, out errorMsg) || validCancel(input);
                additionalRequirement = validCancel(input);
                if (!ReferenceEquals(result, null)) additionalRequirement = result.min >= 0 && result.max >= 0;
            }

            //Check if user typed 'Cancel')
            if(validCancel(input))
            {
                cancelAction();
                dataGeneratorSettings();
                return;
            }
            else
            {
                bool autoAdjusted = false; //Represent whether if the range will be auto adjusted

                Console.WriteLine("Changing the range for the event quantities from {0} to {1}.", DataGenerator.EventAmountRange, result);

                //Calculate the adjusted range based on the new range
                Range newRange;
                int limit = World.RemainingEventCapacity;

                double min = Math.Min(result.min, limit);
                double max = Math.Min(result.max, limit);
                newRange = new Range(min,max);

                //Check if the range will be auto adjusted (Is the new range differs from the new adjusted range)
                if (result != newRange)
                {
                    Console.WriteLine("The new range would be automatically adjusted to {0} to fit the world capacity for events.", newRange);
                    autoAdjusted = true;
                }
                

                Console.WriteLine("Do you wish to continue? [Y/N]");
                if (getYOrN())
                {
                    //Apply value
                    DataGenerator.EventAmountRange = result;
                    Console.WriteLine("The range for the event quantities have been changed to {0}.", DataGenerator.EventAmountRange, result);
                    if(autoAdjusted)Console.WriteLine("And automatically adjusted to {0} in order to fit the world capacity", newRange);
                    Console.WriteLine("Press [Enter] Key to continue");
                    Console.ReadLine();
                    dataGeneratorSettings();
                }
                else
                {
                    cancelAction();
                    dataGeneratorSettings();
                    return;
                }

            }
        }

        /// <summary>
        /// Change the range of generating tickets
        /// </summary>
        void changeTicketQuantityRange()
        {
            Console.WriteLine("Please insert the new range for the ticket quantities for each event in integer numbers with the format: min,max");
            Console.WriteLine("You can also type 'cancel' to cancel this action.");
            string input = Console.ReadLine();
            Range result;
            string errorMsg;

            //Check if the format is valid and the additional requirements have been met
            bool validInputFormat = validRangeFormat(input, true, out result, out errorMsg) || validCancel(input);
            bool additionalRequirement = validCancel(input);
            if (!ReferenceEquals(result, null)) additionalRequirement = result.min >= 0 && result.max >= 0;
            //Repeat asking using if input invalid
            while (!validInputFormat || !additionalRequirement)
            {
                if (!additionalRequirement && String.IsNullOrEmpty(errorMsg)) errorMsg = "Both values shoule be greater or equal than 0";
                Console.WriteLine("Input invalid ({0}). Please insert a correct range with a correct format:", errorMsg);
                input = Console.ReadLine();

                //Reverify the arguments
                validInputFormat = validRangeFormat(input, true, out result, out errorMsg) || validCancel(input);
                additionalRequirement = validCancel(input);
                if (!ReferenceEquals(result, null)) additionalRequirement = result.min >= 0 && result.max >= 0;
            }

            //Check if user typed cancel
            if(validCancel(input))
            {
                cancelAction();
                dataGeneratorSettings();
                return;
            }
            else
            {
                Console.WriteLine("Changing the range for the ticket quantities for each event from ({0}) to ({1}).", DataGenerator.TicketAmountRange, result);
                Console.WriteLine("Do you wish to continue? [Y/N]");
                if (getYOrN())
                {
                    //Apply value
                    DataGenerator.TicketAmountRange = result;
                    Console.WriteLine("The range for the ticket quantities for each event have been changed to ({0}).", DataGenerator.TicketAmountRange);
                    Console.WriteLine("Press [Enter] Key to continue");
                    Console.ReadLine();
                    dataGeneratorSettings();
                }
                else
                {
                    cancelAction();
                    dataGeneratorSettings();
                    return;
                }

            }
        }

        /// <summary>
        /// Change the range of ticket price
        /// </summary>
        void changeTicketPriceRange()
        {
            Console.WriteLine("Please insert the new range for the ticket price with the format: min,max");
            Console.WriteLine("Numbers with more than {0} decimal places would be automatically converted to {0} decimal places (Round up)", 2);
            Console.WriteLine("You can also type 'cancel' to cancel this action.");

            string input = Console.ReadLine();
            Range result;
            string errorMsg;

            //Check if the format is valid and the additional requirements have been met
            bool validInputFormat = validRangeFormat(input, false, out result, out errorMsg) || validCancel(input);
            bool additionalRequirement = validCancel(input);
            if(!ReferenceEquals(result, null)) additionalRequirement = result.min >=0 && result.max>=0;

            //Repeat asking using if input invalid
            while (!validInputFormat || !additionalRequirement)
            {
                if (String.IsNullOrEmpty(errorMsg))
                {
                    if (result.min > 0 && result.max > 0) errorMsg = "Both values have to be greater than or equal to 0";
                    else if (result.min < 0.01) errorMsg = "The minmum value has to be greater than 0.01";
                    else if (result.max > 0) errorMsg = "The minmum value has to be greater than 0";
                }
                Console.WriteLine("Input invalid ({0}). Please insert a correct range with a correct format:", errorMsg);
                input = Console.ReadLine();

                //Reverify the arguments
                validInputFormat = validRangeFormat(input, false, out result, out errorMsg) || validCancel(input);
                additionalRequirement = validCancel(input);
                if (!ReferenceEquals(result, null)) additionalRequirement = result.min >= 0 && result.max >= 0;
            }

            //Check if user typed cancel
            if(validCancel(input))
            {
                cancelAction();
                dataGeneratorSettings();
                return;
            }
            else
            {
                //Convert the value to 2 decimal places (Round Up)
                result = Mathc.ConvertToDecimalPlace(result, 2, Mathc.RoundingPreference.RoundUp);
                Console.WriteLine("Changing the range for the ticket price from $({0}) to $({1}).", DataGenerator.TicketPriceRange, result);
                Console.WriteLine("Do you wish to continue? [Y/N]");
                if (getYOrN())
                {
                    //Apply value
                    DataGenerator.TicketPriceRange = result;
                    Console.WriteLine("The range for the ticket price have been changed to $({0}).", DataGenerator.TicketPriceRange);
                    Console.WriteLine("Press [Enter] Key to continue");
                    Console.ReadLine();
                    dataGeneratorSettings();
                }
                else
                {
                    cancelAction();
                    dataGeneratorSettings();
                    return;
                }

            }
        }

        /// <summary>
        /// Add a single custom event at a specific location in the world
        /// </summary>
        void addCustomEvent()
        {
            Console.WriteLine("You are creating a custom event (Event ID: {0})", World.NextEventId);
            Console.WriteLine("Please insert a coordinates within [X-Axis:({0}) Y-Axis:({1})] , with this format: x,y):", World.AxisX, World.AxisY);
            if (World.CoordinateSystem == World.coordinateSystem.Integer) Console.WriteLine("Only Integer Numbers are allowed under Integer coordinate System");
            else Console.WriteLine("Numbers with more than {0} decimal places would be automatically celling to {0} decimal places", World.DecimalPlacesForDecimalCoordinateSystem);
            Console.WriteLine("You can also insert 'cancel' to cancel the action");

            //Process Input
            string input = Console.ReadLine();
            Vector2 location;
            string errorMsg;

            //Check if the format is valid
            bool validFormat = validLocationInput(input, World.CoordinateSystem == World.coordinateSystem.Integer, out location, out errorMsg) || validCancel(input);
            bool additionalRequirement = validCancel(input);
            if(validFormat && !ReferenceEquals(location,null)) additionalRequirement = DataGenerator.validCoordinates(location);
            //Repeat asking user if input invalid
            while (!validFormat || !additionalRequirement)
            {
                if(!validFormat) Console.WriteLine("Input invalid ({0}).\nPlease reinsert a correct value: ", errorMsg);
                else if (!additionalRequirement)
                Console.WriteLine("({0}) has reached its maximum capacity ({1}) of holding events\nPlease try another coordinates:", location, World.MaxEventAmountInSamecoord);

                input = Console.ReadLine();
                validFormat = validLocationInput(input, World.CoordinateSystem == World.coordinateSystem.Integer, out location, out errorMsg) || validCancel(input);
                additionalRequirement = validCancel(input);
                if (validFormat && !ReferenceEquals(location, null)) additionalRequirement = DataGenerator.validCoordinates(location);
            }

            //Check if the user typed cancel
            if (validCancel(input))
            {
                cancelAction();
                dataGeneratorSettings();
                return;
            }
            else
            {
                //Convert the coordinates to appropiate decimal places
                if (World.CoordinateSystem == World.coordinateSystem.Decimal) location = Mathc.ConvertToDecimalPlace(location, World.DecimalPlacesForDecimalCoordinateSystem);
                Console.WriteLine("Ready to generate event (Event ID: {0}) at ({1})", World.NextEventId, location);
                Console.WriteLine("Tickets would be randomly generated to the event");

                Console.WriteLine("Do you wish to continue? (Y/N)");
                if (getYOrN())
                {
                    //Create new event
                    Event evt = DataGenerator.GenerateEvent(location);
                    //Add event to World
                    World.AddEvent(evt);

                    //Print event info
                    Console.WriteLine("Event (ID: {0}) has been generated at {1}", evt.ID, evt.Location);
                    Console.WriteLine("{0} ticket(s) has been generated to this event", evt.TotalTicketsAvailable);
                    //Sort tickets in the event by price
                    List<Ticket> sortedTickets = TicketFinder.SortedTicketsInPrice(evt.Tickets);
                    Console.WriteLine("Tickets price for this event range from {0} to {1}", sortedTickets[0].Price.ToString("C", new CultureInfo("en-US")), sortedTickets[sortedTickets.Count - 1].Price.ToString("C", new CultureInfo("en-US")));
                    Console.WriteLine("(Press [Enter] To Continue)");
                    Console.ReadLine();
                    dataGeneratorSettings();
                }
                else
                {
                    cancelAction();
                    dataGeneratorSettings();
                    return;
                }
            }
        }
        #endregion

        #region Methods - Generic
        //Constructor
        public UserInterface()
        {
            showDataSummary = true;
            showCurrentSettings = false;
            sb = new StringBuilder();
            mainMenu();
        }

        /// <summary>
        /// Get user input and return as integer
        /// </summary>
        /// <param name="max">The last option (int) in the menu</param>
        /// <param name="min">The first option (int) in the menu</param>
        /// <returns></returns>
        int getOptionInput(int max, int min = 1)
        {
            string input;
            int option;
            //Get value from the user input
            input = Console.ReadLine();

            //Check if input is invalid
            bool validFormat = int.TryParse(input.Trim(), out option);
            bool additionalRequirements = option >= min && option <= max;
            
            //Repeat asking user if input is invalid
            while (String.IsNullOrWhiteSpace(input) || !validFormat || !additionalRequirements ) 
            {
                Console.WriteLine("Input invalid.\nPlease reinsert a correct value: ");

                //Prompt for another input
                input = Console.ReadLine();
                //Reverify arguments
                validFormat = int.TryParse(input.Trim(), out option);
                additionalRequirements = option >= min && option <= max;
            }

            return option;
        }

        /// <summary>
        /// Check if the location input is valid
        /// </summary>
        /// <returns>True: User typed 'cancel'</returns>
        bool getYOrN()
        {
            string input;
            bool option = false;
            input = Console.ReadLine(); //Get value from the user input
            input = input.Trim().ToUpper(); //Make input case insensitive

            //Check the input if it is invalid
            while (String.IsNullOrWhiteSpace(input) || !(input == "Y" || input == "N")) 
            {
                Console.WriteLine("Input invalid.\nPlease reinsert either Y or N");
                input = Console.ReadLine();
                input = input.Trim().ToUpper();
            }

            if (input == "Y") option = true;

            return option;
        }
        
        /// <summary>
        /// Check if the value (double) is an Integer without outputing a number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool validInt(double value)
        {
            int dummy;
            return int.TryParse(value.ToString(), out dummy);
        }

        /// <summary>
        /// Check if the input (string) is in a correct format (min,max)
        /// Then outputs the result (Range) if valid;
        /// Output the error message (string) if invalid
        /// </summary>
        /// <param name="input">User input as string</param>
        /// <param name="IntegerValueOnly">Whether the values are required to be integers</param>
        /// <param name="result">The value (Range) after parsing the input</param>
        /// <param name="errorMsg">The error message to show</param>
        /// <param name="rangeType">The range type for the output range</param>
        /// <returns></returns>
        bool validRangeFormat(string input, bool IntegerValueOnly, out Range result, out string errorMsg, Range.RangeType rangeType = Range.RangeType.Numbers)
        {
            bool valid = false;
            result = null;
            double min;
            double max;
            errorMsg = "";

            //Check if it contains ',' (Required in the format)
            if (input.Contains(','))
            {
                //Split the input (string) with ','
                string[] _input = input.Split(',');
                if (_input.Length == 2 //2 parts (numbers)
                    && double.TryParse(_input[0], out min)  //Check if the first part is a number, and export it
                    && double.TryParse(_input[1], out max)) //Check if the second part is a number, and export it
                {
                    //Check if max is higher than or equals to min
                    if (max >= min)
                    {
                        //If it is integer numbers only, check if they are valid integers
                        if (IntegerValueOnly && (!validInt(min) || !validInt(max)))
                        {
                            errorMsg = "Only Integer numbers accepted";
                        }
                        else
                        {
                            //Return valid and create the new range
                            valid = true;
                            result = new Range(min, max, rangeType);
                        }
                    }
                    else
                    {
                        errorMsg = "Maximum value cannot be less than minimum value";
                    }
                }
            }

            if((errorMsg == "") && !valid) errorMsg = "Incorrect format";
            return valid;
        }

        /// <summary>
        /// Check if the input (string) is in a correct format (x,y)
        /// Then outputs the result (Vector2) if valid;
        /// Output the error message (string) if invalid
        /// </summary>
        /// <param name="input">User input as string</param>
        /// <param name="IntegerValueOnly">Whether the values are required to be integers</param>
        /// <param name="result">The value (Vector2) after parsing the input</param>
        /// <param name="errorMsg">The error message to show</param>
        /// <returns></returns>
        bool validVector2Format(string input, bool IntegerValueOnly, out Vector2 result, out string errorMsg)
        {
            bool valid = false;
            result = null;
            double x;
            double y;
            errorMsg = "";

            //Check if it contains ',' (Required in the format)
            if (input.Contains(','))
            {
                //Split the input (string) with ','
                string[] _input = input.Split(',');
                if (_input.Length == 2 //2 parts (numbers)
                    && double.TryParse(_input[0], out x)  //Check if the first part is a number, and export it
                    && double.TryParse(_input[1], out y)) //Check if the second part is a number, and export it
                {
                    //If it is integer numbers only, check if they are valid integers
                    if (IntegerValueOnly && (!validInt(x) || !validInt(y)))
                    {
                        errorMsg = "coordinates with decimal numbers are not allowed in Integer Mode";
                    }
                    else
                    {
                        //Return valid and create new Vector2
                        valid = true;
                        result = new Vector2(x, y);
                    }
                }
            }

            if(errorMsg=="" && !valid) errorMsg = "Incorrect format";
            return valid;
        }

        /// <summary>
        /// Check if the input is 'Cancel' (Case-insensitive)
        /// </summary>
        /// <param name="input">User input as string</param>
        /// <returns></returns>
        bool validCancel(string input)
        {
            //Check if the input is 'cancel', removing the case sensitivity by Trim() and ToUpper()
            return (input.Trim().ToUpper() == "CANCEL");
        }

        /// <summary>
        /// Check if the location is valid
        /// First check if the value is a correct Vector2
        /// Then check if the Vector2 is within the world
        /// </summary>
        /// <param name="input">User input as string</param>
        /// <param name="IntegerValueOnly">Whether the values are required to be integers</param>
        /// <param name="result">The value (Vector2) after parsing the input</param>
        /// <param name="errorMsg">The error message to show</param>
        /// <returns></returns>
        bool validLocationInput(string input, bool IntegerValueOnly, out Vector2 result, out string errorMsg)
        {
            //Initiate local variables
            bool valid = false;
            result = null;
            errorMsg = "";

            //Check if the input is invalid
            if (String.IsNullOrWhiteSpace(input))
            {
                errorMsg = "Null or WhiteSpace";
            }
            else
            {
                //Check if the format is valid
                if (validVector2Format(input, IntegerValueOnly, out result, out errorMsg))
                { 
                    //Check if the coordinates are within the world axis
                    if (!World.AxisX.WithinRange(result.x) || !World.AxisY.WithinRange(result.y))
                    {
                        errorMsg = String.Format("coordinates are outside of the world limit [X-Axis:{0}  Y-Axis:{1}]",
                                                    World.AxisX, World.AxisY);
                    }
                    else
                    {
                        valid = true;
                    }     
                }
                //Check if user typed 'Cancel' (Case insensitive)
                else if (validCancel(input))
                {
                    errorMsg = "";
                    valid = true;
                }
            }
            return valid;
        }

        /// <summary>
        /// Show 'cancel' text on screen
        /// </summary>
        void cancelAction()
        {
            Console.WriteLine("The process has been canceled. No changes has been made.");
            Console.WriteLine("(Press [Enter] to continue)");
            Console.ReadLine();
        }
        #endregion

    }
}
