# Changelog

All notable changes to this project will be documented in this file.

## [v1.2.2] - 2024-08-31

### Added
- Config option to update miles sailed text on sleep

## [v1.2.1] - 2024-08-29

### Added
- Miles sailed to lifetime stats
- Ports visited to lifetime stats

### Fixed
- NullRef when loading game off of ship and trying to sell an item

## [v1.2.0] - 2024-08-27

### Added
- Stats & Transit UI.
- Notifications for badges and transit records.
- Notification sound

### Changed
- Reorganized code
- Refactored badge checks for ports visited

## [v1.1.0] - 2024-08-13

### Added

- Badges for fish caught.
- Badges for ports visited.
- Assets for badges.

### Changed

- Ports visited bookmark will now shift left if fish caught UI is disabled.
- Refactored code to have external objects only accessed once instead of everytime needed.
- UI have had badge locations added, fish caught UI text was shifted to accomodate space for badges.

## [v1.0.0] - 2024-08-09

### Added

- Fish Caught UI.
- Hide fish names in Fish Caught UI until first caught made configurable.
- Ports Visited UI.
- Hide port names in the Ports Visible UI until first visited made configurable.
- Both UIs can be disabled in config.
