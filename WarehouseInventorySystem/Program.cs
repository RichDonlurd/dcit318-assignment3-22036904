using System;
using System.Collections.Generic;

// =====================================================
// QUESTION 3: WAREHOUSE INVENTORY MANAGEMENT SYSTEM
// =====================================================

// =====================================================
// a. Marker Interface for Inventory Items
// =====================================================

public interface IInventoryItem
{
    int Id { get; }
    string Name { get; }
    int Quantity { get; set; }
}

// =====================================================
// b. ElectronicItem
// =====================================================

public class ElectronicItem : IInventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string Brand { get; set; }
    public int WarrantyMonths { get; set; }

    public ElectronicItem(
        int id,
        string name,
        int quantity,
        string brand,
        int warrantyMonths)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Brand = brand;
        WarrantyMonths = warrantyMonths;
    }
}

// =====================================================
// c. GroceryItem
// =====================================================

public class GroceryItem : IInventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }

    public GroceryItem(
        int id,
        string name,
        int quantity,
        DateTime expiryDate)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        ExpiryDate = expiryDate;
    }
}

// =====================================================
// e. CUSTOM EXCEPTIONS
// =====================================================

public class DuplicateItemException : Exception
{
    public DuplicateItemException(string message)
        : base(message)
    {
    }
}

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message)
        : base(message)
    {
    }
}

public class InvalidQuantityException : Exception
{
    public InvalidQuantityException(string message)
        : base(message)
    {
    }
}

// =====================================================
// d. GENERIC INVENTORY REPOSITORY
// =====================================================

public class InventoryRepository<T>
    where T : IInventoryItem
{
    private Dictionary<int, T> _items;

    public InventoryRepository()
    {
        _items = new Dictionary<int, T>();
    }

    // Add item
    public void AddItem(T item)
    {
        if (_items.ContainsKey(item.Id))
        {
            throw new DuplicateItemException(
                $"Item with ID {item.Id} already exists."
            );
        }

        _items.Add(item.Id, item);
    }

    // Get item by ID
    public T GetItemById(int id)
    {
        if (!_items.ContainsKey(id))
        {
            throw new ItemNotFoundException(
                $"Item with ID {id} was not found."
            );
        }

        return _items[id];
    }

    // Remove item
    public void RemoveItem(int id)
    {
        if (!_items.ContainsKey(id))
        {
            throw new ItemNotFoundException(
                $"Cannot remove item. Item with ID {id} was not found."
            );
        }

        _items.Remove(id);
    }

    // Get all items
    public List<T> GetAllItems()
    {
        return new List<T>(_items.Values);
    }

    // Update quantity
    public void UpdateQuantity(int id, int newQuantity)
    {
        if (newQuantity < 0)
        {
            throw new InvalidQuantityException(
                "Quantity cannot be negative."
            );
        }

        T item = GetItemById(id);

        item.Quantity = newQuantity;
    }
}

// =====================================================
// f. WAREHOUSE MANAGER
// =====================================================

public class WareHouseManager
{
    private InventoryRepository<ElectronicItem> _electronics;
    private InventoryRepository<GroceryItem> _groceries;

    public WareHouseManager()
    {
        _electronics =
            new InventoryRepository<ElectronicItem>();

        _groceries =
            new InventoryRepository<GroceryItem>();
    }

    // SeedData()
    public void SeedData()
    {
        // Electronic items
        _electronics.AddItem(
            new ElectronicItem(
                101,
                "Laptop",
                10,
                "Dell",
                24
            )
        );

        _electronics.AddItem(
            new ElectronicItem(
                102,
                "Smartphone",
                15,
                "Samsung",
                12
            )
        );

        _electronics.AddItem(
            new ElectronicItem(
                103,
                "Television",
                5,
                "LG",
                24
            )
        );

        // Grocery items
        _groceries.AddItem(
            new GroceryItem(
                201,
                "Rice",
                50,
                new DateTime(2027, 6, 30)
            )
        );

        _groceries.AddItem(
            new GroceryItem(
                202,
                "Milk",
                30,
                new DateTime(2026, 12, 15)
            )
        );

        _groceries.AddItem(
            new GroceryItem(
                203,
                "Bread",
                25,
                new DateTime(2026, 9, 15)
            )
        );
    }

    // Generic method for printing items
    public void PrintAllItems<T>(
        InventoryRepository<T> repo)
        where T : IInventoryItem
    {
        foreach (T item in repo.GetAllItems())
        {
            Console.WriteLine(
                $"ID: {item.Id} | " +
                $"Name: {item.Name} | " +
                $"Quantity: {item.Quantity}"
            );
        }
    }

    // Generic method for increasing stock
    public void IncreaseStock<T>(
        InventoryRepository<T> repo,
        int id,
        int quantity)
        where T : IInventoryItem
    {
        try
        {
            T item = repo.GetItemById(id);

            if (quantity < 0)
            {
                throw new InvalidQuantityException(
                    "Increase quantity cannot be negative."
                );
            }

            repo.UpdateQuantity(
                id,
                item.Quantity + quantity
            );

            Console.WriteLine(
                $"Stock increased successfully for {item.Name}. " +
                $"New quantity: {item.Quantity}"
            );
        }
        catch (ItemNotFoundException ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
        catch (InvalidQuantityException ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    // Generic method for removing an item
    public void RemoveItemById<T>(
        InventoryRepository<T> repo,
        int id)
        where T : IInventoryItem
    {
        try
        {
            T item = repo.GetItemById(id);

            repo.RemoveItem(id);

            Console.WriteLine(
                $"Item '{item.Name}' removed successfully."
            );
        }
        catch (ItemNotFoundException ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    // Demonstrate duplicate item exception
    public void TestDuplicateItem()
    {
        try
        {
            _electronics.AddItem(
                new ElectronicItem(
                    101,
                    "Another Laptop",
                    5,
                    "HP",
                    12
                )
            );
        }
        catch (DuplicateItemException ex)
        {
            Console.WriteLine(
                $"ERROR: {ex.Message}"
            );
        }
    }

    // Demonstrate invalid quantity exception
    public void TestInvalidQuantity()
    {
        try
        {
            _groceries.UpdateQuantity(201, -10);
        }
        catch (InvalidQuantityException ex)
        {
            Console.WriteLine(
                $"ERROR: {ex.Message}"
            );
        }
    }
}

// =====================================================
// MAIN APPLICATION
// =====================================================

class Program
{
    static void Main()
    {
        WareHouseManager manager =
            new WareHouseManager();

        // i. Instantiate WareHouseManager
        // ii. Seed the inventory
        manager.SeedData();

        // iii. Print all grocery items
        Console.WriteLine("====================================");
        Console.WriteLine("       GROCERY INVENTORY");
        Console.WriteLine("====================================");

        manager.PrintAllItems(
            GetGroceryRepository(manager)
        );

        // iv. Print all electronic items
        Console.WriteLine();
        Console.WriteLine("====================================");
        Console.WriteLine("      ELECTRONIC INVENTORY");
        Console.WriteLine("====================================");

        manager.PrintAllItems(
            GetElectronicRepository(manager)
        );

        // Increase stock demonstration
        Console.WriteLine();
        Console.WriteLine("====================================");
        Console.WriteLine("       INCREASE STOCK");
        Console.WriteLine("====================================");

        manager.IncreaseStock(
            GetGroceryRepository(manager),
            201,
            20
        );

        // v. Try to add duplicate item
        Console.WriteLine();
        Console.WriteLine("====================================");
        Console.WriteLine("       DUPLICATE ITEM TEST");
        Console.WriteLine("====================================");

        manager.TestDuplicateItem();

        // Try to remove a non-existent item
        Console.WriteLine();
        Console.WriteLine("====================================");
        Console.WriteLine("     NON-EXISTENT ITEM TEST");
        Console.WriteLine("====================================");

        manager.RemoveItemById(
            GetElectronicRepository(manager),
            999
        );

        // Try to update with invalid quantity
        Console.WriteLine();
        Console.WriteLine("====================================");
        Console.WriteLine("       INVALID QUANTITY TEST");
        Console.WriteLine("====================================");

        manager.TestInvalidQuantity();
    }

    // Helper methods to access repositories
    private static InventoryRepository<GroceryItem>
        GetGroceryRepository(WareHouseManager manager)
    {
        var field = typeof(WareHouseManager)
            .GetField(
                "_groceries",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance
            );

        return (InventoryRepository<GroceryItem>)
            field!.GetValue(manager)!;
    }

    private static InventoryRepository<ElectronicItem>
        GetElectronicRepository(WareHouseManager manager)
    {
        var field = typeof(WareHouseManager)
            .GetField(
                "_electronics",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance
            );

        return (InventoryRepository<ElectronicItem>)
            field!.GetValue(manager)!;
    }
}