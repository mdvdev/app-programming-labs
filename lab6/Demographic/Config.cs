using System.Text.Json;
using System.Text.Json.Serialization;

namespace Demographic;

public class Config
{
    [JsonPropertyName("BirthProbability")]
    public double BirthProbability { get; }
    
    [JsonPropertyName("ChildGenderProbability")]
    public double ChildGenderProbability { get; }
    
    [JsonPropertyName("ReproductiveAgeFemaleMin")]
    public int ReproductiveAgeFemaleMin { get; }
    
    [JsonPropertyName("ReproductiveAgeFemaleMax")]
    public int ReproductiveAgeFemaleMax { get; }
    
    [JsonPropertyName("SimulationStep")]
    public int SimulationStep { get; }
    
    [JsonConstructor]
    public Config(double birthProbability, double childGenderProbability, int reproductiveAgeFemaleMin,
        int reproductiveAgeFemaleMax, int simulationStep)
    {
        BirthProbability = birthProbability;
        ChildGenderProbability = childGenderProbability;
        ReproductiveAgeFemaleMin = reproductiveAgeFemaleMin;
        ReproductiveAgeFemaleMax = reproductiveAgeFemaleMax;
        SimulationStep = simulationStep;
    }

    public static Config Load(string configFilePath)
    {
        var json = File.ReadAllText(configFilePath);
        var config = JsonSerializer.Deserialize<Config>(json);
        
        if (config == null)
        {
            throw new FileLoadException("The config file could not be loaded.");
        }

        return config;
    }
}