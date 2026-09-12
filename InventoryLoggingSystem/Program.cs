using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public record InventoryItem(
    int Id,
    string Name,
    int Quantity,
    DateTime DateAdded
) : IInventoryEntity;

public interface IInventoryEntity
{
    int Id { get; }
}

public class InventoryLogger<T> where T : IInventoryEntity
{
    private List<T> _log = new List<T>();
    private string _filePath;

    public InventoryLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Add(T item)
    {
        _log.Add(item);
    }

    public List<T> GetAll()
    {
        return _log;
    }

    public void SaveToFile()
    {
        try
        {
            string json = JsonSerializer.Serialize(
                _log,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }
            );

            File.WriteAllText(_filePath, json);

            Console.WriteLine(
                $"Inventory data saved to {_filePath}"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"ERROR while saving data: {ex.Message}"
            );
        }
    }

    public void LoadFromFile()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine(
                    "No inventory file found."
                );
                return;
            }

            string json = File.ReadAllText(_filePath);

            List<T>? loadedItems =
                JsonSerializer.Deserialize<List<T>>(json);

            if (loadedItems != null)
            {
                _log = loadedItems;
            }

            Console.WriteLine(
                $"Inventory data loaded from {_filePath}"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"ERROR while loading data: {ex.Message}"
            );
        }
    }
}

public class InventoryApp
{
    private InventoryLogger<InventoryItem> _logger;

    public InventoryApp(string filePath)
    {
        _logger =
            new InventoryLogger<InventoryItem>(
                filePath
            );
    }

    public void SeedSampleData()
    {
        _logger.Add(
            new InventoryItem(
                1,
                "Laptop",
                10,
                DateTime.Now
            )
        );

        _logger.Add(
            new InventoryItem(
                2,
                "Wireless Mouse",
                25,
                DateTime.Now
            )
        );

        _logger.Add(
            new InventoryItem(
                3,
                "Keyboard",
                15,
                DateTime.Now
            )
        );

        _logger.Add(
            new InventoryItem(
                4,
                "Monitor",
                8,
                DateTime.Now
            )
        );

        Console.WriteLine(
            "Sample inventory data added."
        );
    }

    public void SaveData()
    {
        _logger.SaveToFile();
    }

    public void LoadData()
    {
        _logger.LoadFromFile();
    }

    public void PrintAllItems()
    {
        Console.WriteLine();
        Console.WriteLine("INVENTORY ITEMS");
        Console.WriteLine("================");

        foreach (InventoryItem item in _logger.GetAll())
        {
            Console.WriteLine(
                $"ID: {item.Id} | " +
                $"Name: {item.Name} | " +
                $"Quantity: {item.Quantity} | " +
                $"Date Added: {item.DateAdded}"
            );
        }
    }
}

public class Program
{
    public static void Main()
    {
        string filePath = "inventory.json";

        Console.WriteLine(
            "=== INVENTORY LOGGING SYSTEM ==="
        );
        Console.WriteLine();

        // First session
        InventoryApp app = new InventoryApp(filePath);

        app.SeedSampleData();

        Console.WriteLine();
        Console.WriteLine("Saving inventory...");
        app.SaveData();

        // Simulate a new session
        Console.WriteLine();
        Console.WriteLine("Starting a new session...");

        InventoryApp newApp =
            new InventoryApp(filePath);

        newApp.LoadData();

        newApp.PrintAllItems();

        Console.WriteLine();
        Console.WriteLine(
            "Inventory loading completed successfully."
        );
    }
}