# APBD Task 2 — University Equipment Rental System

A small C# console application that runs a university equipment rental desk:
it registers equipment and users, rents items out, takes them back (charging a
late penalty when needed), tracks availability, and prints a summary report.

The exercise is graded mainly on **design** — separation of responsibilities,
cohesion, coupling and a sensible use of SOLID — so most of this README explains
*how the code is divided and why*, not just what it does.

> Author: **s28379** · Course: APBD · Tutorial 2

---

## How to run

Requires the **.NET 8 SDK**.

```bash
# from the repository root
dotnet run --project APBD_TASK2
```

This runs the built-in demonstration scenario (see `App/DemoScenario.cs`), which
walks through every required operation, including the two operations that are
supposed to be blocked.

---

## What it does (functional requirements)

| Requirement | Where |
|---|---|
| Add a user | `IUserService.Register` |
| Add equipment of a chosen type | `IEquipmentService.Register` |
| List all equipment with status | `IEquipmentService.GetAll` |
| List only available equipment | `IEquipmentService.GetAvailable` |
| Rent equipment to a user | `IRentalService.Rent` |
| Return equipment + late penalty | `IRentalService.Return` |
| Mark equipment unavailable (damage/maintenance) | `IEquipmentService.MarkUnavailable` |
| Active rentals for a user | `IRentalService.GetActiveRentals` |
| Overdue rentals | `IRentalService.GetOverdueRentals` |
| Summary report | `IReportService.BuildSummary` |

Business rules enforced: a student may hold **2** active rentals, an employee
**5**; unavailable items cannot be rented; exceeding the limit is blocked; a late
return is charged a per-day penalty.

---

## Project structure

```
APBD_TASK2/
├── Program.cs                 thin entry point — just starts the composition root
├── App/
│   ├── Bootstrapper.cs        composition root: builds and wires every object
│   └── DemoScenario.cs        the required end-to-end demo (no business logic)
├── Models/                    the domain (data + the rules that protect it)
│   ├── Equipment.cs           abstract base; owns its status state-machine
│   ├── Laptop.cs / Projector.cs / Camera.cs   the three equipment types
│   ├── User.cs                abstract base; abstract rental limit
│   ├── Student.cs / Employee.cs               the two user types
│   └── Rental.cs              one rental transaction (who/what/when/return)
├── Enums/                     EquipmentStatus, UserType
├── Exceptions/                one typed exception per expected failure
├── Repositories/              storage boundary (interfaces + in-memory impls)
├── Policies/                  IPenaltyPolicy + LatePenaltyPolicy (the fee rule)
├── Services/                  business logic (Equipment/User/Rental/Report)
└── Database/Singleton.cs      the provided in-memory "mock database"
```

The code is split into **four layers** that depend on each other in one direction
only: `Console (App)` → `Services` → `Repositories` → `Models`. Nothing in the
domain knows about services, and nothing in the services knows how data is stored.

---

## Design decisions (cohesion, coupling, responsibilities)

I tried to make every class answer one question, and to let classes talk to each
other through interfaces rather than concrete types. The concrete places where
that shows up:

**Single Responsibility / Cohesion.**
Renting and reporting are two different jobs, so they are two different classes:
`RentalService` only rents/returns/queries, and `ReportService` only builds the
summary text. `Program.cs` does no business logic at all — it just asks the
`Bootstrapper` to wire things up and runs the demo. Each equipment instance keeps
its own status rules (`MarkAsRented`, `MarkAsReturned`, …) in one place instead of
letting services flip a status field from the outside.

**Coupling / Dependency Inversion.**
No service creates its own dependencies with `new`. `RentalService` receives an
`IRentalRepository` and an `IPenaltyPolicy` through its constructor; the only
class allowed to pick concrete types is the `Bootstrapper`. So the business code
depends on abstractions, and the in-memory `Singleton` store could be swapped for
a real database without touching a single service. The repository interfaces are
the "clear, stable boundary" between business logic and storage.

**Open/Closed.**
The rental limit is an `abstract` member on `User`, not a `switch` over a
`UserType` enum. Adding a new kind of user (say a `Guest` with a limit of 1) means
adding one subclass — no existing method has to be edited. The same idea applies
to equipment: a new device type is a new subclass of `Equipment`.

**Interface Segregation.**
Instead of one fat `IEquipmentService` with ten methods, the contracts are split
by who needs them: `IEquipmentService`, `IUserService`, `IRentalService`,
`IReportService`. A caller depends only on the slice it actually uses.

**Liskov Substitution.**
Every `Equipment` subclass is a genuine equipment and every `User` subclass is a
genuine user — the subclasses only *add* fields and *fill in* the abstract
members, they never break what the base promises, so the services can treat them
all through the base type.

**Explicit failure handling.**
Every expected failure is its own exception type (`EquipmentNotAvailableException`,
`RentalLimitExceededException`, `ActiveRentalNotFoundException`, …) deriving from a
common `RentalException`. The console layer catches `RentalException` once and
prints a friendly message; it never has to inspect return codes.

**One place per business rule.**
The two rules most likely to change live in exactly one spot each: the **late fee**
is in `LatePenaltyPolicy` (a one-line, configurable daily rate) and the **rental
limit** is on the `User` subclasses. Neither rule is duplicated across the project.

---

## Notes

- Target framework: **.NET 8** (`net8.0`).
- Date logic is day-based and the demo/report use fixed dates, so output is
  deterministic regardless of when or where it is run.
- The provided `Singleton` is kept as the in-memory data store; the services reach
  it only through the repository interfaces.
