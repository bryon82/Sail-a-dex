# Sail-a-dex

This Sailwind mod is intended to keep track of various player activities along with badges that are acquired upon meeting certain criteria. Below are the currently tracked items and what is configurable.

### Requires
* [BepInEx 5.4.23](https://github.com/BepInEx/BepInEx/releases)
* [SailwindModdingHelper 2.0.3](https://thunderstore.io/c/sailwind/p/App24/SailwindModdingHelper/)

## Fish Caught Log

![Screenshot of the Fish Caught UI](https://github.com/bryon82/Sail-a-dex/blob/main/Screenshots/fishCaughtUI.jpg)
A UI showing how many fish that you caught separated by the different types.  
A fish is registered as caught once you reel it in and collect it.  
Badges are acquired upon catching 25, 50, and 100 of each type, 50, 250, and 500 total, and also for catching at least one of each type.  
Access this log by selecting the "fish caught" bookmark in the player log.

## Ports Visited Log

![Screenshot of the Ports Visited UI](https://github.com/bryon82/Sail-a-dex/blob/main/Screenshots/portsVisitedUI.jpg)
A UI showing which ports you have visited separated by region.  
A port is registered as visited once you enter the area where mission goods are normally delivered.  
Badges are acquired upon visiting all ports within a region as well as visiting every port.  
Access this log by selecting the "ports visited" bookmark in the log.  

## Stats & Transit Log

![Screenshot of the Stats & Transit UI](https://github.com/bryon82/Sail-a-dex/blob/main/Screenshots/statsUI.jpg)
A UI showing various stats and transit times between region capital cities.
Cargo mass stat "Record" will be recorded once you are a certain distance from land.
Cargo mass is only the items in crates, barrels, and packages.
If you use PassageDude mod and book travel or teleport via console that will reset your currently tracked transits.

## Notifications

Notifications will pop up along with a ship bell sound on badges earned and fastest transit times recorded. 


## Configurable in BepInEx config
* By default the fish names are hidden before being caught for the first time, this can be disabled.
* By default port names are visible, can be configured to be hidden until visited for the first time.
* Notifications can be disabled.
* Notification sound can be adjusted and disabled.
* All Logs can be disabled individually.  
  **WARNING**: Disabling a previously enabled log and then saving the game will erase all previously saved log progress.

## Installation
Place the Sail-a-dex folder (not the contents, the whole thing) into the Sailwind/BepInEx/Plugins folder.  
If upgrading from v1.0.0 delete the old Sail-a-dex from the BepInEx/plugins folder first.

## Future Features
* Popups on achievement badge earned.
* Looking into tracking other stats (heaviest total cargo load, longest time out at sea, etc) and possibly badges for them.
