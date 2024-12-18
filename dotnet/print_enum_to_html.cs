using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        // Example list of data
        var data = new List<Person>
        {
            new Person { Id = 1, Name = "Alice", Age = 30 },
            new Person { Id = 2, Name = "Bob", Age = 25 },
            new Person { Id = 3, Name = "Charlie", Age = 35 }
        };

        // Export the list as an HTML table
        string htmlTable = ToHtmlTable(data);

        // Print the generated HTML
        Console.WriteLine(htmlTable);
    }

    // Generic method to convert a list to an HTML table
    public static string ToHtmlTable<T>(IEnumerable<T> data)
    {
        if (data == null || !data.Any())
        {
            return "<table><tr><td>No data available</td></tr></table>";
        }

        var properties = typeof(T).GetProperties();

        // Generate the HTML table header
        var headerRow = string.Join("", properties.Select(p => $"<th>{p.Name}</th>"));

        // Generate the HTML table rows
        var rows = data.Select(item =>
        {
            var cells = string.Join("", properties.Select(p => $"<td>{p.GetValue(item)}</td>"));
            return $"<tr>{cells}</tr>";
        });

        // Combine header and rows into a complete table
        return $"<table border='1'>\n<thead><tr>{headerRow}</tr></thead>\n<tbody>\n{string.Join("\n", rows)}\n</tbody>\n</table>";
    }
}

// Example data class
public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}