# The company needs to identify an ideal contractor
**A description of the starting situation**
There is an order for something to be shipped. The order details have been given to the system to determine an ideal contractor to take the shipment, as well as when and where the shipment will be picked up.

**A description of the normal flow of events**

1. The system looks uses an advanced, efficient, and innovative algorithm to find a contractor who will be able to deliver the order quickly while minimizing internal costs
2. Once a contractor is found, a work order is sent (See _Send work order to contractor_)

**A description of what can go wrong**

1. There are no contractors in the system/no contractors are found
  1. Inform the other department to start looking for contractors
2. There are too many orders
  1. Send and notification to the customer to inform them of the potential delay
  2. Give customers estimates on overview (See _Customer Puts in an Order)_

**Information about other concurrent activities**
This will not interfere with other processes.

**A description of the state when the scenario finishes.**
When the ideal contractor is found, optimal contractor is notified with a work order.

