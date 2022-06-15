# Chain of Responsibility

This classic design pattern avoids coupling the sender of a request to its receiver by giving more than one object a chance to handle the request. Chain the receiving objects and pass the request along the chain until an object handles it.
Here's a real-world example which you should use as the basis of your implementation:

A business purchases items from suppliers using Purchase Orders. Each Purchase Order has a unique id, is for an amount (assume in Pounds Sterling only), has a supplier name, a description of the goods ordered, a unit cost (again in Pounds) and a quantity. A Purchase Order is only for one type of item at a time, ie you can’t order Gloves and Masks on the same PO. 
* Purchase Orders can be approved by a Director, provided the total amount is <= £5000, the unit cost <= £1000, and the quantity is <= 100.
* Otherwise, Purchase Orders can be approved by the CFO, provided the total amount is <= £50000, the unit cost <= £10000, and the quantity is <= 1000.
* Otherwise, Purchase Orders must be approved by the CEO, provided the total amount is <= £500000.
* Otherwise, a board meeting must be called and the Purchase Order must be approved by a Director, the CFO and the CEO.

Create a program that implements the Chain of Responsibility using this model, and these user stories:
1.	As an employee, I want to submit Purchase Orders to Finance, so that I can purchase items required for my job function
2.	As a Finance team member, I want to send Purchase Orders for approval, so that I can arrange the payment to the supplier
3.	As the CFO, I want purchase orders to be approved by the appropriate person, based on our purchasing rules, so that spending can be controlled

You are not required to create a user interface, but your solution must display the Purchase Order details and show the progress from actor to actor in the chain of responsibility indicating whether that actor approved the purchase order and if not, why not. 
