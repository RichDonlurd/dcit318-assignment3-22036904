using System;
using System.Collections.Generic;
using System.IO;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int Score { get; set; }

    public Student(int id, string fullName, int score)
    {
        Id = id;
        FullName = fullName;
        Score = score;
    }

    public string GetGrade()
    {
        if (Score >= 80)
            return "A";
        else if (Score >= 70)
            return "B";
        else if (Score >= 60)
            return "C";
        else if (Score >= 50)
            return "D";
        else
            return "F";
    }
}

public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string message)
        : base(message)
    {
    }
}

public class MissingFieldException : Exception
{
    public MissingFieldException(string message)
        : base(message)
    {
    }
}

public class StudentResultProcessor
{
    public List<Student> ReadStudentsFromFile(string inputFilePath)
    {
        List<Student> students = new List<Student>();

        using (StreamReader reader = new StreamReader(inputFilePath))
        {
            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] fields = line.Split(',');

                // Check that all three required fields are present
                if (fields.Length != 3)
                {
                    throw new MissingFieldException(
                        "A student record is missing one or more required fields."
                    );
                }

                string idText = fields[0].Trim();
                string fullName = fields[1].Trim();
                string scoreText = fields[2].Trim();

                // Check for empty fields
                if (string.IsNullOrWhiteSpace(idText) ||
                    string.IsNullOrWhiteSpace(fullName) ||
                    string.IsNullOrWhiteSpace(scoreText))
                {
                    throw new MissingFieldException(
                        "A required student field is missing."
                    );
                }

                // Convert ID
                if (!int.TryParse(idText, out int id))
                {
                    throw new InvalidScoreFormatException(
                        "Student ID must be a valid integer."
                    );
                }

                // Convert Score
                if (!int.TryParse(scoreText, out int score))
                {
                    throw new InvalidScoreFormatException(
                        $"Invalid score format for student: {fullName}"
                    );
                }

                students.Add(new Student(id, fullName, score));
            }
        }

        return students;
    }

    public void WriteReportToFile(
        List<Student> students,
        string outputFilePath)
    {
        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            writer.WriteLine("STUDENT RESULT REPORT");
            writer.WriteLine("=====================");
            writer.WriteLine();

            foreach (Student student in students)
            {
                writer.WriteLine(
                    $"{student.FullName} (ID: {student.Id}): " +
                    $"Score = {student.Score}, " +
                    $"Grade = {student.GetGrade()}"
                );
            }
        }
    }
}

public class Program
{
    public static void Main()
    {
        string inputFilePath = "students.txt";
        string outputFilePath = "student_report.txt";

        StudentResultProcessor processor =
            new StudentResultProcessor();

        try
        {
            List<Student> students =
                processor.ReadStudentsFromFile(inputFilePath);

            processor.WriteReportToFile(
                students,
                outputFilePath
            );

            Console.WriteLine("Student records processed successfully.");
            Console.WriteLine(
                $"Report created: {outputFilePath}"
            );
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine(
                "ERROR: The students.txt file was not found."
            );
        }
        catch (InvalidScoreFormatException ex)
        {
            Console.WriteLine(
                $"ERROR: {ex.Message}"
            );
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine(
                $"ERROR: {ex.Message}"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"ERROR: {ex.Message}"
            );
        }
    }
}