# University Equipment Rental Service

## Project overview

This project is a C# console application that supports a university equipment rental service through both an interactive menu and a built-in demonstration scenario.

The system allows:
- registering users,
- registering equipment,
- renting equipment,
- returning equipment,
- marking equipment as unavailable,
- checking active and overdue rentals,
- generating reports,
- saving and loading application data from a JSON file.

The solution was designed not only to work correctly, but also to show a clear separation of responsibilities between the domain model, business logic, persistence layer, and console interface.

---

## Supported functionality

The application supports the following operations:

- add a new user,
- add a new equipment item,
- display all equipment,
- display only available equipment,
- rent equipment to a user,
- return equipment and calculate a possible late penalty,
- mark equipment as unavailable,
- display active rentals for a selected user,
- display overdue rentals,
- generate a summary report,
- save application data to JSON,
- load application data from JSON.

---

## Domain model

The domain model contains the following main elements:

### Equipment
Common abstract concept for all equipment types.

Implemented equipment types:
- `Laptop`
- `Projector`
- `Camera`

Each equipment item stores:
- unique identifier,
- name,
- availability state,
- unavailable flag,
- unavailable reason.

Each concrete equipment type contains at least two type-specific fields:
- `Laptop`: RAM, operating system
- `Projector`: lumens, resolution
- `Camera`: megapixels, lens type

### User
Common abstract concept for system users.

Implemented user types:
- `Student`
- `Employee`

Each user stores:
- unique identifier,
- first name,
- last name,
- user type

### Rental
Represents a rental operation and stores:
- rental identifier,
- user identifier,
- equipment identifier,
- rental date,
- due date,
- actual return date,
- penalty value

---

## Business rules

The application implements the following business rules:

- a student may have at most 2 active rentals,
- an employee may have at most 5 active rentals,
- unavailable equipment cannot be rented,
- rental is blocked if the user exceeds the active rental limit,
- a late return results in a penalty,
- penalty calculation and user limits are defined in dedicated services so they are easy to modify.

### Penalty rule used in this project
The penalty is:
- **10 currency units per day of delay**

---

## Project structure

The project is organized into the following folders:

### `Domain`
Contains the core domain model:
- `Equipment`
- `Laptop`
- `Projector`
- `Camera`
- `User`
- `Student`
- `Employee`
- `Rental`

### `Services`
Contains business logic and application services:
- `EquipmentService`
- `UserService`
- `RentalService`
- `RentalRulesService`
- `PenaltyPolicy`
- `ReportService`

### `ConsoleUI`
Contains console interaction logic:
- `ConsoleMenu`
- `DemoScenarioRunner`

### `Persistence`
Contains JSON persistence logic:
- `JsonStorageService`
- `AppData` and DTO classes

### `Common`
Contains shared utility classes:
- `OperationResult`
- `IdGenerator`

---

## Design decisions

### Why abstract classes were used
`Equipment` and `User` are modeled as abstract classes because they represent common domain concepts with shared data and behavior, while `Laptop`, `Projector`, `Camera`, `Student`, and `Employee` are more specific variants.

Inheritance was used where it naturally follows from the domain model, rather than only to make the code look more object-oriented.

### Why services were separated
The business logic is not placed in `Program.cs` or in one large application class.

Instead, responsibilities are separated:
- `EquipmentService` manages equipment-related operations,
- `UserService` manages users,
- `RentalService` handles rental workflow,
- `RentalRulesService` provides rental limits,
- `PenaltyPolicy` calculates penalties,
- `ReportService` is responsible for reporting only.

This makes the code easier to understand, test, and modify.

### Why JSON persistence uses DTOs
The domain model contains abstract classes and specialized subclasses, so direct JSON serialization/deserialization would be less clean and more fragile.

For that reason, JSON persistence is handled through dedicated DTO classes stored in `AppData`.  
This keeps the persistence layer separate from the domain model and makes the save/load process more explicit.

---

## Cohesion, coupling, and class responsibilities

This project attempts to address cohesion, coupling, and class responsibilities in the following way:

### Cohesion
Each class has one main purpose:
- domain classes represent domain state,
- services contain business logic,
- report generation is handled in a dedicated reporting service,
- console input/output is handled in the console UI layer,
- persistence is handled in a separate persistence layer.

### Coupling
Classes are not tightly connected without reason:
- `RentalService` does not store equipment or users on its own,
- it uses `EquipmentService` and `UserService`,
- rules and penalties are delegated to dedicated classes instead of being hard-coded in many places.

This reduces coupling and avoids duplicating logic.

### Class responsibilities
Responsibilities are intentionally divided:
- `Program.cs` only bootstraps the application,
- `ConsoleMenu` handles interactive usage,
- `DemoScenarioRunner` handles the demonstration scenario,
- services handle business behavior,
- domain classes represent data and limited self-contained state transitions.

---

## Run instructions

### Requirements
- .NET SDK installed

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run
```

After starting the application, you can choose one of two modes:
1. **Run demo scenario**
2. **Run interactive menu**

---

## Demo scenario

The application includes a demonstration scenario that presents:
- adding multiple users,
- adding multiple equipment items,
- a correct rental,
- an invalid rental attempt,
- a return on time,
- a delayed return with a penalty,
- final reports.

This mode is available from the startup screen.

---

## JSON persistence

The project includes JSON save/load support.

Sample application data included with the project is stored in:

`Data/appdata.json`

To load the sample state:
1. run the application,
2. choose **Run interactive menu**,
3. select **Load data from JSON**

This restores example users, equipment items, and rentals from the JSON file.

---

## Notes

- The project includes both an interactive console menu and a demo scenario.
- The repository history is organized into multiple meaningful commits.
- The implementation uses multiple working branches merged into `main`.