namespace Demographic.FileOperations;

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public static class FileOperations
{
    public static List<Person> LoadPopulation(string filePath, Config config, int currentYear,
        Dictionary<(int MinAge, int MaxAge), (double Male, double Female)> deathRules)
    {
        var lines = File.ReadLines(filePath).Skip(1); // Skip CSV header
        var population = new List<Person>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(',');
            if (parts.Length < 2) throw new FormatException($"Invalid line format: {line}");

            if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var age) ||
                !float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var count))
            {
                throw new FormatException($"Invalid data in line: {line}");
            }

            population.AddRange(CreatePersons(age, (int)count, currentYear, config, deathRules));
        }

        return population;
    }

    public static Dictionary<(int MinAge, int MaxAge), (double Male, double Female)> LoadDeathRules(string filePath)
    {
        var lines = File.ReadLines(filePath).Skip(1);
        var deathRules = new Dictionary<(int MinAge, int MaxAge), (double Male, double Female)>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(',');
            if (parts.Length != 4)
            {
                throw new FormatException(
                    $"Invalid line format: {line}. Expected 4 values (MinAge, MaxAge, MaleProb, FemaleProb).");
            }

            if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var minAge) ||
                !int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var maxAge) ||
                !double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var maleProb) ||
                !double.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var femaleProb))
            {
                throw new FormatException(
                    $"Invalid data in line: {line}. Ensure ages are integers and probabilities are decimals.");
            }

            if (maleProb < 0 || maleProb > 1 || femaleProb < 0 || femaleProb > 1)
            {
                throw new ArgumentOutOfRangeException(
                    $"Probabilities must be between 0 and 1. Invalid values in line: {line}.");
            }

            if (deathRules.ContainsKey((minAge, maxAge)))
            {
                throw new ArgumentException(
                    $"Duplicate age range found: ({minAge}, {maxAge}). Each range must be unique.");
            }

            deathRules.Add((minAge, maxAge), (maleProb, femaleProb));
        }

        return deathRules;
    }

    private static IEnumerable<Person> CreatePersons(int age, int count, int currentYear, Config config,
        Dictionary<(int MinAge, int MaxAge), (double Male, double Female)> deathRules)
    {
        var persons = new List<Person>();

        for (var i = 0; i < count; i++)
        {
            var gender = i % 2 == 0 ? "Male" : "Female";
            persons.Add(new Person(currentYear - age, gender, config, deathRules));
        }

        return persons;
    }
}