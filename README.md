# ToDo

## Mehmet Akif

### Foundation
- Initial console view and project folder structure

### UI & Navigation
- **NavigateMenu\<T\>()** — arrow-key driven menu that works with any IMyCollection, highlights the selected item in color
- **GetAtIndex\<T\>()** — retrieves an item from any IMyCollection by index
- **Prompt()** — displays a prompt string and reads a line of input from the user
- **Run()** — the main entry point and event loop for the console UI
- Kanban board — always-visible task board on the main screen
- Refactored ConsoleTaskView to depend on IMyCollection instead of .NET built-in collections

### Data Structures
- **FindBy\<K\>() on MyLinkedList** — traverses the list and returns the first item matching a given key using a custom comparer
- **BSFind() on MyArray** — binary search implementation on the internal array
- **_isSorted flag on MyArray** — tracks sort state so Find() automatically uses binary search when possible instead of a linear scan
- **AddSorted() on MyArray** — inserts an item in sorted position using binary search to find the correct index
- **IsSorted() on MyArray** — checks whether the array is currently in sorted order

### Features
- **DeleteUser()** — finds a user by name and removes them from the in-memory collection
- **Priority field on TaskItem** — integer value (1, 2, 3) representing task priority
- **PriorityText on TaskItem** — computed property that maps the priority integer to "Low", "Medium", or "High"
- **SetTaskPriority()** — finds a task by ID and updates its priority, validates the 1–3 range

### Fixes & Maintenance
- Fixed 0-sized array bug and added safe exit from NavigateMenu
- Fixed broken JSON formatting and removed stale temp content
- Fixed .csproj and moved task.json to the correct location
- Updated README with prerequisites and build steps

## Prerequisites

* [.NET SDK](https://dotnet.microsoft.com/en-us/download)

---


## Build and Compile

Restore dependencies:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build
```
