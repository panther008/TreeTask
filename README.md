# TreeTask

A .NET console application that implements two tasks: category tree performance comparison and memory allocation-free base64 URL to GUID converter.

## Task List

### Task 1: Category Tree Performance Comparison

**Requirement:**
Having tree-based, at least 4 level category table, using a console application, write code that compares performance of two solutions:

1. **Using Entity Framework (EF) and LINQ** - Build a category tree and display it in the console
2. **Using a T-SQL stored procedure** - Build the category tree and also display it in the console

**Which approach would you prefer and why?**

#### Implementation Details

- **Tree Structure**: Category table with at least 4 levels of hierarchy
  - Example: Electronics → Computers → Laptops → Gaming Laptops (4 levels)
  - Example: Electronics → Phones → Smartphones → Android → Flagships (5 levels)
  - Example: Clothing → Men's → Shirts → Casual Shirts (4 levels)

- **EF + LINQ Approach** (`CategoryTreeEfService`)
  - Uses Entity Framework Core with `AsNoTracking()` for read-only optimization
  - Builds tree structure in-memory using LINQ `ToLookup`
  - Recursive tree building algorithm

- **Stored Procedure Approach** (`CategoryTreeSpService`)
  - Uses T-SQL stored procedure `GetCategoryTree`
  - Retrieves flat category list from database
  - Builds tree structure in application code using LINQ

- **Performance Measurement**
  - Both approaches use `Stopwatch` for timing
  - Displays execution time in milliseconds
  - Shows performance comparison summary with percentage difference
  - Provides recommendation based on actual results

### Task 2: Memory Allocation Free Base64 URL to GUID Converter

**Requirement:**
Write memory allocation free base64 URL string to GUID converter.

#### Implementation Details

- **Class**: `GuidConverter` in `TreeTask.Utils` namespace
- **Method**: `Base64UrlToGuid(ReadOnlySpan<char> base64Url)`
- **Memory Allocation Free Features**:
  - Uses `ReadOnlySpan<char>` for zero-copy input (no string allocation)
  - Uses `stackalloc char[24]` for base64 conversion (stack allocation, no heap)
  - Uses `stackalloc byte[16]` for GUID bytes (stack allocation, no heap)
  - No garbage collection pressure
  - Handles URL-safe base64 characters (`-` and `_`)

## Project Structure

```
TreeTask/
├── Data/
│   ├── AppDbContext.cs          # EF Core DbContext configuration
│   └── DbInitializer.cs         # Database initialization and seed data
├── Models/
│   └── Category.cs              # Category entity model
├── Services/
│   ├── CategoryTreeEfService.cs # EF + LINQ tree building service
│   ├── CategoryTreeSpService.cs # Stored procedure tree building service
│   ├── ConsoleTreePrinter.cs   # Tree display utility
│   └── ICategoryTreeService.cs # Service interface
├── Utils/
│   └── GuidConverter.cs        # Memory allocation-free base64 URL to GUID converter
├── Program.cs                   # Main application entry point
├── CreateStoredProcedure.sql    # SQL script to create GetCategoryTree stored procedure
└── appsettings.json            # Database connection string configuration
```

## Prerequisites

- .NET 8.0 SDK or later
- SQL Server (local or remote)
- SQL Server Management Studio (SSMS) or sqlcmd for running SQL scripts

## Setup Instructions

### 1. Configure Database Connection

Update `appsettings.json` with your SQL Server connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=CategoryDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 2. Create Stored Procedure

Run the SQL script to create the `GetCategoryTree` stored procedure:

**Using SSMS:**
1. Open SQL Server Management Studio
2. Connect to your SQL Server
3. Open `CreateStoredProcedure.sql`
4. Execute the script

**Using sqlcmd:**
```bash
sqlcmd -S YOUR_SERVER -d CategoryDB -i CreateStoredProcedure.sql
```

### 3. Build and Run

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

## Output

The application will display:

1. **GuidConverter Test**
   - Test cases demonstrating base64 URL to GUID conversion
   - Shows input base64 URL strings and converted GUIDs

2. **Task 1: Performance Comparison**
   - Entity Framework + LINQ tree build time and tree structure
   - Stored Procedure tree build time and tree structure
   - Performance comparison summary showing:
     - Execution times for both approaches
     - Percentage difference
     - Faster approach identification
   - Recommendation section explaining:
     - Which approach is preferred
     - Detailed reasons for the preference
     - Performance considerations

## Example Output

```
=== GuidConverter Test ===
Base64Url: AAAAAAAAAAAAAAAAAAAAAA
Converted Guid: 00000000-0000-0000-0000-000000000000

=== Task 1: Performance Comparison ===

--- Entity Framework + LINQ Approach ---
- Electronics
  - Computers
    - Laptops
      - Gaming Laptops
      - Ultrabooks
    - Desktops
  - Phones
    - Smartphones
      - Android
        - Flagships
        - Budget
      - iOS

--- Stored Procedure Approach ---
[Same tree structure]

=== Performance Comparison Summary ===
Entity Framework + LINQ: 45ms
Stored Procedure: 52ms

✅ EF + LINQ is 13.5% faster (7ms faster)

=== Recommendation ===
✅ PREFERRED: Entity Framework + LINQ

Reasons:
1. Faster performance for small to medium datasets
2. Type-safe LINQ queries with IntelliSense support
3. No need to maintain separate SQL stored procedures
...
```

## Features

### Category Tree Implementation

- **Database Model**: Self-referential category table with `Id`, `Name`, `ParentId`
- **Tree Building**: Efficient recursive algorithm using `ToLookup` for O(n) complexity
- **Display**: Hierarchical console output with proper indentation
- **Seed Data**: Automatically creates sample data with 4+ levels on first run

### GuidConverter Implementation

- **Zero Allocations**: All operations use stack-allocated memory
- **URL-Safe Support**: Handles base64 URL encoding (`-` and `_` characters)
- **Efficient**: Uses `Span<char>` and `Span<byte>` for high-performance operations
- **Safe**: Proper padding handling for base64 strings

## Technical Details

### Entity Framework Configuration

- `AsNoTracking()` used for read-only queries (faster performance)
- `Children` property ignored in database mapping (in-memory only)
- Explicit key and property configuration in `OnModelCreating`

### Stored Procedure

- Returns flat list of all categories
- Tree structure built in application code (consistent with EF approach)
- Can be enhanced with recursive CTE for SQL-level tree building if needed

## Recommendations

The application automatically analyzes performance and provides recommendations based on actual execution times. Generally:

- **EF + LINQ**: Preferred for maintainability, type safety, and developer experience
- **Stored Procedures**: Better for very large datasets or when SQL optimization is critical

## Notes

- Database and seed data are created automatically on first run
- The stored procedure must be created manually using the provided SQL script
- Performance results may vary based on database size, network latency, and hardware

## License

This project is for demonstration purposes.
