using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TreeTask.Data;
using TreeTask.Services;
using TreeTask.Utils;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


var services = new ServiceCollection();

services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

services.AddScoped<ICategoryTreeService, CategoryTreeEfService>();
services.AddSingleton(new CategoryTreeSpService(configuration.GetConnectionString("DefaultConnection")));
services.AddSingleton<ConsoleTreePrinter>();

var provider = services.BuildServiceProvider();

// Ensure DB Exists
var db = provider.GetRequiredService<AppDbContext>();
DbInitializer.EnsureDbCreated(db);

// Resolve services
var efService = provider.GetRequiredService<ICategoryTreeService>();
var spService = provider.GetRequiredService<CategoryTreeSpService>();
var printer = provider.GetRequiredService<ConsoleTreePrinter>();

// Task 2: Test GuidConverter
Console.WriteLine("\n=== GuidConverter Test ===");
TestGuidConverter();

Console.WriteLine("\n=== Task 1: Performance Comparison ===\n");

// Run EF Tree
Console.WriteLine("--- Entity Framework + LINQ Approach ---");
var efStopwatch = System.Diagnostics.Stopwatch.StartNew();
var efTree = await efService.BuildTreeAsync();
efStopwatch.Stop();
printer.Print(efTree);
var efTime = efStopwatch.ElapsedMilliseconds;

// Run SP Tree
Console.WriteLine("\n--- Stored Procedure Approach ---");
var spStopwatch = System.Diagnostics.Stopwatch.StartNew();
var spTree = await spService.BuildTreeAsync();
spStopwatch.Stop();
printer.Print(spTree);
var spTime = spStopwatch.ElapsedMilliseconds;

// Performance Comparison
Console.WriteLine("\n=== Performance Comparison Summary ===");
Console.WriteLine($"Entity Framework + LINQ: {efTime}ms");
Console.WriteLine($"Stored Procedure: {spTime}ms");
if (efTime < spTime)
{
    var diff = ((double)(spTime - efTime) / spTime) * 100;
    Console.WriteLine($"\n✅ EF + LINQ is {diff:F1}% faster ({spTime - efTime}ms faster)");
}
else if (spTime < efTime)
{
    var diff = ((double)(efTime - spTime) / efTime) * 100;
    Console.WriteLine($"\n✅ Stored Procedure is {diff:F1}% faster ({efTime - spTime}ms faster)");
}
else
{
    Console.WriteLine("\n⚠️ Both approaches have similar performance");
}

// Recommendation
Console.WriteLine("\n=== Recommendation ===");
PrintRecommendation(efTime, spTime);

Console.WriteLine("\nDone...");

// GuidConverter Test Method
static void TestGuidConverter()
{
    // Test case 1: Convert a base64url string to Guid
    string base64Url1 = "AAAAAAAAAAAAAAAAAAAAAA";
    Guid guid1 = GuidConverter.Base64UrlToGuid(base64Url1);
    Console.WriteLine($"Base64Url: {base64Url1}");
    Console.WriteLine($"Converted Guid: {guid1}");
    Console.WriteLine();

    // Test case 2: Another example
    string base64Url2 = "YXNkZmFzZGZhZGZh";
    Guid guid2 = GuidConverter.Base64UrlToGuid(base64Url2);
    Console.WriteLine($"Base64Url: {base64Url2}");
    Console.WriteLine($"Converted Guid: {guid2}");
    Console.WriteLine();

    // Test case 3: With URL-safe characters (- and _)
    string base64Url3 = "YXNkZmFzZGZhZGZh-_";
    Guid guid3 = GuidConverter.Base64UrlToGuid(base64Url3);
    Console.WriteLine($"Base64Url: {base64Url3}");
    Console.WriteLine($"Converted Guid: {guid3}");
    Console.WriteLine();
}

// Recommendation Method
static void PrintRecommendation(long efTime, long spTime)
{
    Console.WriteLine("Which approach would you prefer and why?\n");
    
    if (efTime <= spTime)
    {
        Console.WriteLine("✅ PREFERRED: Entity Framework + LINQ");
        Console.WriteLine("\nReasons:");
        Console.WriteLine("1. Faster performance for small to medium datasets");
        Console.WriteLine("2. Type-safe LINQ queries with IntelliSense support");
        Console.WriteLine("3. No need to maintain separate SQL stored procedures");
        Console.WriteLine("4. Easier to test and debug with in-memory providers");
        Console.WriteLine("5. Better integration with C# codebase and refactoring tools");
        Console.WriteLine("6. AsNoTracking() optimizes read-only scenarios");
        Console.WriteLine("7. More maintainable - changes in model reflect automatically");
    }
    else
    {
        Console.WriteLine("✅ PREFERRED: Stored Procedure");
        Console.WriteLine("\nReasons:");
        Console.WriteLine("1. Better performance for large datasets (optimized SQL execution)");
        Console.WriteLine("2. Reduced network traffic - tree built at database level");
        Console.WriteLine("3. Database-level optimizations and execution plans");
        Console.WriteLine("4. Can leverage database-specific features (CTEs, recursive queries)");
        Console.WriteLine("5. Centralized business logic in database");
        Console.WriteLine("6. Better for scenarios where SQL tuning is critical");
    }
    
    Console.WriteLine("\nNote: For this specific use case, EF + LINQ offers better");
    Console.WriteLine("developer experience and maintainability, while stored procedures");
    Console.WriteLine("can provide better performance for very large category hierarchies.");
}
