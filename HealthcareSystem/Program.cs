using System;
using System.Collections.Generic;
using System.Linq;

// =====================================================
// QUESTION 2: HEALTHCARE SYSTEM
// =====================================================

// a. Generic Repository
public class Repository<T>
{
    private List<T> items;

    public Repository()
    {
        items = new List<T>();
    }

    public void Add(T item)
    {
        items.Add(item);
    }

    public List<T> GetAll()
    {
        return items;
    }

    public T? GetById(Func<T, bool> predicate)
    {
        return items.FirstOrDefault(predicate);
    }

    public bool Remove(Func<T, bool> predicate)
    {
        T? item = items.FirstOrDefault(predicate);

        if (item == null)
        {
            return false;
        }

        items.Remove(item);
        return true;
    }
}

// =====================================================
// b. Patient Class
// =====================================================

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }

    public Patient(int id, string name, int age, string gender)
    {
        Id = id;
        Name = name;
        Age = age;
        Gender = gender;
    }
}

// =====================================================
// c. Prescription Class
// =====================================================

public class Prescription
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string MedicationName { get; set; }
    public DateTime DateIssued { get; set; }

    public Prescription(
        int id,
        int patientId,
        string medicationName,
        DateTime dateIssued)
    {
        Id = id;
        PatientId = patientId;
        MedicationName = medicationName;
        DateIssued = dateIssued;
    }
}

// =====================================================
// g. HealthSystemApp
// =====================================================

public class HealthSystemApp
{
    private Repository<Patient> _patientRepo;
    private Repository<Prescription> _prescriptionRepo;

    private Dictionary<int, List<Prescription>> _prescriptionMap;

    public HealthSystemApp()
    {
        _patientRepo = new Repository<Patient>();
        _prescriptionRepo = new Repository<Prescription>();

        _prescriptionMap =
            new Dictionary<int, List<Prescription>>();
    }

    // SeedData()
    public void SeedData()
    {
        // Add patients
        _patientRepo.Add(
            new Patient(1, "Alice Smith", 30, "Female")
        );

        _patientRepo.Add(
            new Patient(2, "John Mensah", 45, "Male")
        );

        _patientRepo.Add(
            new Patient(3, "Michael Brown", 25, "Male")
        );

        // Add prescriptions
        _prescriptionRepo.Add(
            new Prescription(
                101,
                1,
                "Paracetamol",
                DateTime.Now
            )
        );

        _prescriptionRepo.Add(
            new Prescription(
                102,
                1,
                "Amoxicillin",
                DateTime.Now
            )
        );

        _prescriptionRepo.Add(
            new Prescription(
                103,
                2,
                "Ibuprofen",
                DateTime.Now
            )
        );

        _prescriptionRepo.Add(
            new Prescription(
                104,
                2,
                "Vitamin C",
                DateTime.Now
            )
        );

        _prescriptionRepo.Add(
            new Prescription(
                105,
                3,
                "Cough Syrup",
                DateTime.Now
            )
        );
    }

    // BuildPrescriptionMap()
    public void BuildPrescriptionMap()
    {
        foreach (Prescription prescription
                 in _prescriptionRepo.GetAll())
        {
            if (!_prescriptionMap.ContainsKey(prescription.PatientId))
            {
                _prescriptionMap[prescription.PatientId] =
                    new List<Prescription>();
            }

            _prescriptionMap[prescription.PatientId]
                .Add(prescription);
        }
    }

    // Get prescriptions for a specific patient
    public List<Prescription> GetPrescriptionsByPatientId(
        int patientId)
    {
        if (_prescriptionMap.ContainsKey(patientId))
        {
            return _prescriptionMap[patientId];
        }

        return new List<Prescription>();
    }

    // Print all patients
    public void PrintAllPatients()
    {
        Console.WriteLine();
        Console.WriteLine("ALL PATIENTS");
        Console.WriteLine("------------");

        foreach (Patient patient in _patientRepo.GetAll())
        {
            Console.WriteLine(
                $"ID: {patient.Id}, " +
                $"Name: {patient.Name}, " +
                $"Age: {patient.Age}, " +
                $"Gender: {patient.Gender}"
            );
        }
    }

    // Print prescriptions for a specific patient
    public void PrintPrescriptionsForPatient(int patientId)
    {
        Patient? patient =
            _patientRepo.GetById(p => p.Id == patientId);

        if (patient == null)
        {
            Console.WriteLine(
                $"Patient with ID {patientId} was not found."
            );

            return;
        }

        Console.WriteLine();
        Console.WriteLine(
            $"PRESCRIPTIONS FOR {patient.Name}"
        );
        Console.WriteLine("-------------------------------");

        List<Prescription> prescriptions =
            GetPrescriptionsByPatientId(patientId);

        if (prescriptions.Count == 0)
        {
            Console.WriteLine("No prescriptions found.");
            return;
        }

        foreach (Prescription prescription in prescriptions)
        {
            Console.WriteLine(
                $"Prescription ID: {prescription.Id}"
            );

            Console.WriteLine(
                $"Medication: {prescription.MedicationName}"
            );

            Console.WriteLine(
                $"Date Issued: {prescription.DateIssued:dd/MM/yyyy}"
            );

            Console.WriteLine();
        }
    }
}

// =====================================================
// Main Application
// =====================================================

class Program
{
    static void Main()
    {
        HealthSystemApp app = new HealthSystemApp();

        // i. Instantiate HealthSystemApp
        // ii. Call SeedData()
        app.SeedData();

        // iii. Build the prescription map
        app.BuildPrescriptionMap();

        // iv. Print all patients
        app.PrintAllPatients();

        // v. Display prescriptions for Patient ID 1
        app.PrintPrescriptionsForPatient(1);
    }
}