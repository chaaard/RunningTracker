# .NET Core Web API Running Tracker

## Versioning
.NET Core Version
```sh
6
```

## Project Structure
```
├── RunningTracker
│   ├── Properties
│       └── launchSettings.json
│   ├── Controllers
│       ├── RunningActivityController.cs
│       └── UserController.cs
│   ├── appsettings.json
│   └── Program.cs
├── RunningTracker.Application
│   ├── DTOs
│       ├── RunningActivityDto.cs
│       ├── UserCreateDto.cs
│       └── UserUpdateDto.cs
│   ├── Interfaces
│       ├── IRunningActivityService.cs
│       └── IUserService.cs
│   └── Services
│       ├── RunningActivityService.cs
│       └── UserService.cs
├── RunningTracker.Domain
│   └── Models
│       ├── RunningActivity.cs
│       └── User.cs
├── RunningTracker.Infrastructure
│   ├── Data
│       └── RunningTrackerContext.cs
│   ├── Interfaces
│       ├── IRunningActivityRepository.cs
│       └── IUserRepository.cs
│   ├── Migrations
│       ├── 20240717092138_InitialCreate.cs
│       └── RunningTrackerContextModelSnapshot.cs
│   └── Repositories
│       ├── RunningActivityRepository.cs
│       └── UserRepository.cs
├── RunningTracker.UnitTesting
│   ├── RunningActivityControllerTests.cs
│   ├── UserControllerTests.cs
│   └── Usings.cs
├── README.md
├── RunningTracker.sln
```

## Setting Up

```sh
$ git clone https://github.com/chaaard/RunningTracker.git
$ cd RunningTracker
```

followed by

```sh
$ Add-Migration InitialCreate
$ Update-Database
```
