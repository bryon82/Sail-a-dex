# Sail-a-dex

This Sailwind mod is intended to keep track of various player activities. Below are the currently tracked items and what is configurable.

### Requires
* [BepinEx 5.4.23](https://github.com/BepInEx/BepInEx/releases)
* [SailwindModdingHelper 2.0.3](https://thunderstore.io/c/sailwind/p/App24/SailwindModdingHelper/)

## Fish Caught Log
A UI showing how many fish that you caught separated by the different types. Access it by selecting the "fish caught" bookmark in the log.  
A fish is registered as caught once you reel it in and collect it. Fish names will appear as "???" until caught the first time. This can be configured as noted below.

## Ports Visited Log
A UI showing which ports you have visited separated by region. Access it by selecting the "ports visited" bookmark in the log.  
A port is registered as visited once you enter the area where mission goods are normally delivered. Port names can be configured to be hidden.

## Configurable in BepinEx config
* By default the fish names are hidden before being caught for the first time, this can be disabled.
* By default port names are visible, can be configured to be hidden until visited for the first time.
* Both the Fish Caught Log and the Ports Visited Log can be turned off.  
  **WARNING**: Disabling a previously enabled log and then saving the game will erase all previously saved log progress.

## Installation
Place the Sail-a-dex.dll in the Sailwind/BepinEx/Plugins folder.
