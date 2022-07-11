# Dining Philosophers

### The problem

A number of philosophers sit at a round table in a Chinese restaurant. Each philosopher has a plate of food. A philosopher may at any time be *thinking* or *eating*, but never both.

Each philosopher will think for a random duration, and then they wish to eat immediately for a different random duration. The plate of food never runs out and is infinite.

A philosopher may only eat when they have a *pair of chopsticks* with which to do so. There are a limited number of pairs of chopsticks available which is always less than the number of philosophers dining.

In order to eat, a philosopher must a) stop thinking, and b) acquire a pair of chopsticks to start eating. 

When a philosopher finishes eating, they must return their chopsticks to the center of the table.

The philosophers do not speak to each other at any time or coordinate their activities in any way. Each can see when a pair of chopsticks is available or not. 

### The task

*Write a program which represents the Dining Philosophers problem and provides a solution that allows all the philosophers to continue to think and eat forever, without any individual starving to death (ie getting stuck). Your solution must allow for all the available pairs of chopsticks to be in use simultaneously.*

To make the exercise practical, assume the following constraints:

* There are always more than one, and never more than six philosophers at the table
* The random times for thinking and eating are always at least one second, and never more than five seconds
* There is always at least one pair of chopsticks available

The observant among you will have already seen recognised this is basically an implementation of a synchronised thread pool. In order to achieve the most efficient solution it will be necessary to use threads in your implementation. You can't just use an existing thread pool implementation in your programming framework, BTW, as that would be too easy :-) You may, however, use monitor / mutex structures.

Your code should display the status of each philosopher and pairs of chopsticks as a log, along these lines:

*Philosopher 1 is thinking.*
*Chopsticks 4 are now available.*
*Philosopher 2 wants to eat.*
*Chopsticks 4 are now in use.*
*Philosopher 2 is eating.*
*Philosopher 1 wants to eat.*

Etc etc. 
