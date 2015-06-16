# Introduction #

So I was bored and wrote this simulator.  Then it occurred to me:

**"What if everybody I play Dominion with wrote an AI for this and we pitted them against each other?"**

Wouldn't that be awesome?

# Details #

First, the basics:

  * The simulator isn't actually done YET - not all the intro cards are implemented
  * It does have enough done to be interesting already, though
  * The simulator and strategies are currently implemented in C#
  * Strategies implement the IStrategy interface, which is detailed below
  * Strategies take action by talking to a PlayerFacade, which is detailed below
  * Strategies also have access to the Supply to see what cards are available

## IStrategy ##

Strategies must implement IStrategy, which can be seen here:
http://code.google.com/p/dominionsim/source/browse/DominionSim/Strategy/IStrategy.cs

## PlayerFacade ##

Strategies get access to hand and deck information, and cause things to happen, by talking to a PlayerFacade, which can be seen here:
http://code.google.com/p/dominionsim/source/browse/DominionSim/Strategy/PlayerFacade.cs

## Supply ##

Supply is pretty crude at the moment and needs a better interface to the Strategies.

# Implemented Cards #

At the moment the following cards are implemented:

  * All treasure (Copper, Silver, Gold)
  * All VPs (Estate, Duchy, Province)
  * Smithy
  * Chapel
  * Workshop
  * Cellar
  * Feast

# Contest #

I propose that we have a contest where everyone interested will write their own strategy and we'll pit them against each other:

  * The basic intro set of 10 cards will be used
  * We can run 2, 3, 4, 5, and 6 player games with the strategies
  * We'll come up with a way where everybody's AI plays everybody else's a consistent number of times
  * I fully expect we might see a different champ in the 2-player bracket than in the 6-player bracket