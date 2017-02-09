# Contractor needs to reject job
**A description of the starting situation**
An order has a contractor, but that contractor cannot take it.

**A description of the normal flow of events**
1. A contractor receives a work order and realizes they can't or that it won't be feasible
2. The contractor rejects the work order
3. They will then be prompted for a reason
  1. System may notify HR / administration
4. Contractor is marked as unavailable
5. Order is put back into system (See _System finds ideal contractor)_

**A description of what can go wrong**

1. The contractor has accidentally rejects it
  1. They will then be prompted with an "are you sure/give a reason" dialog
2. The contractor realizes they can take it
  1. Unless the order has been redistributed and a work order has been sent out, there is an undo feature


**Information about other concurrent activities**
The system then begins working on finding a new ideal driver. The rejection is kept in a record where an admin can see. If the contractor is abusing this feature, an admin will be able to see and take action.

**A description of the state when the scenario finishes.**
The system then finds a new contractor. The contractor will get a notification about the details of their rejection