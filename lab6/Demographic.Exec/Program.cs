namespace Demographic.Exec;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 6)
        {
            Console.WriteLine("Invalid number of arguments.");
            Environment.Exit(1);
        }
        
        var initialAgeFile = args[0];
        var deathRulesFile = args[1];
        var configFilePath = args[2];
        var startYear = int.Parse(args[3]);
        var endYear = int.Parse(args[4]);
        var outputFile = args[5];

        var config = Config.Load(configFilePath);
        
        var deathRules =
            FileOperations.FileOperations.LoadDeathRules(deathRulesFile);
        
        var initialPopulation =
            FileOperations.FileOperations.LoadPopulation(initialAgeFile, config, startYear, deathRules);

        IEngine engine = new Engine(initialPopulation, config, deathRules, outputFile);
        
        engine.StartSimulation(startYear, endYear);

        Console.WriteLine("Simulation completed. Results saved to " + outputFile);
    }
}