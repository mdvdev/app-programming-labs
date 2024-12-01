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
        try
        {
            var json = File.ReadAllText(configFilePath);
            var config = DeserializeStrict<Config>(json);

            return config;
        }
        catch (Exception e)
        {
            throw new FileLoadException("The config file could not be loaded.", e);
        }
    }

    private static T DeserializeStrict<T>(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            throw new ArgumentException("JSON cannot be null or empty.");
        }
        
        var result = JsonSerializer.Deserialize<T>(json);
        if (result == null)
        {
            throw new FileLoadException("The config file could not be loaded.");
        }

        return result;
    }
}