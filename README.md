# Event Ticket System

**Introduction**

This system allows user to find a number of nearest events and show their cheapest tickets in a World with 2 axis, while all data generates randomly at the start of the system. This is a console application with a simple menu user interface written in C#.

This system aims not only functional, but also flexible and expandable. Developers can configure settings or regenerate data during runtime. With the good use of Object-Oriented Programming and well code documenting, developers can amend the codes or add functionalities easily.

Please note that after exiting the application, all settings resets to default and all data would be lost (Be regenerated when opening the application again).

**Build Instruction**

The built application is available in the 'build' folder (EventTicketSystem.exe).

To build the application, please follow the instruction:
1. Download the whole branch
2. Open the solution (EventTicketSystem.sln) with Visual Studio
3. Click 'Start (F5)' and a Debug build should be built and start automatically

**Programme structure**

World represents the world in the system, storing the coordinates system and all events data. It also stores all tickets data for getting information like the price range of all tickets.

Event and Ticket are separated into 2 classes to be instantiated as new instances to store data. 

Event class represent an event in the world, which stores id (int), coordinates (Vector2) and a list of Ticket objects (List<Ticket>). It also contains methods to add event into the list.

Ticket object represent a group of tickets at the same price, which stores the price of the tickets, quantity of the tickets and the corresponding event.

Both Vector2 and Range classes store a pair double variables, which respectively represent a pair of coordinates and a range of numbers.

Data generator in charges generating random data, including Events, Tickets and coordinates. Then add the data into the lists in World object. 

Ticket Finder class provides methods to sort Ticket and Event lists, as well as finding the tickets.

Mathc is a custom class which provides additional methods to solve math problems, such as calculating distance and round numbers into decimal places.

User Interface shows the menu, information and process user inputs.

**User Interface**

The user interface of the application is menu based. To access functionalities or settings, please type in the corresponding number of the options.

In the main menu, Option 4 and 5 are used for toggling the visibility of information of 'Application Settings' and 'Data Summary'. By default, 'Data Summary' are shown and 'Application Settings' are hidden.

**Ticket Finder**

Option 1 is the Ticket Finder which is the core function of the application. Simply input the coordinates in the format x,y then the nearest events and the cheapest tickets will show. 

The ticket finding mechanism is to sort all event data (A list of Event objects) by the Manhattan distances between the event locations and coordinates user inputted. If there are more than one event having the same distance, the event are sorted with their cheapest ticket price in an ascending order. 

After the sorting the event list in distance, starting from the first index of the sorted events list, the list of Ticket objects within the Event object would be sorted in price in ascending order. The first index of the Ticket list would be the cheapest price for the event. This process ignores the events has no tickets and loops until enough tickets have been found (5 by default) or there is no more events in the list.

**Settings**

The default settings meets the first requirements of the test:
- Both X and Y axis ranges from -10 to 10
- The Coordinate system uses integer numbers
- Each pair of coordinates only holds 1 event at maximum
- Each Event has 0 or more tickets (Randomly generated)
- Each Tickets priced in US dollars with the minimum value of 0.01 (Randomly generated)
- Distance are computed as Manhattan distance
- Ticket Finder shows up to 5 nearest events and their cheapest tickets

There are lots of settings for user to set, including the range of the world axis, how many events can be at the same coordinates, and the coordinates system uses integer number or decimal numbers. Users are encouraged to explore those settings.
