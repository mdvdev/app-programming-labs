namespace Demographic;

public class Engine : IEngine
{
    public event EventHandler YearTick;

    private readonly Config _config;
    private readonly List<Person> _population;
    private readonly string _outputFile;
    private Dictionary<(int MinAge, int MaxAge), (double Male, double Female)> _deathRules;
    private const string _csvHeader =
        "Year,Total,Male_0_18,Male_19_45,Male_46_65,Male_66_100,Female_0_18,Female_19_45,Female_46_65,Female_66_100\n";
    
    public int CurrentYear { get; private set; }

    public Engine(List<Person> population, Config config,
        Dictionary<(int MinAge, int MaxAge), (double Male, double Female)> deathRules, string outputFile)
    {
        _population = population;
        _config = config;
        _deathRules = deathRules;
        _outputFile = outputFile;

        if (File.Exists(_outputFile)) File.Delete(_outputFile);
        File.WriteAllText(_outputFile, _csvHeader);

        foreach (var person in _population)
        {
            SubscribeToChildBirth(person);
            SubscribePersonToYearTick(person);
        }
    }

    public void StartSimulation(int startYear, int endYear)
    {
        for (CurrentYear = startYear; CurrentYear <= endYear; CurrentYear += _config.SimulationStep)
        {
            WritePopulationData();
            OnYearTick();
        }
    }

    public void AddPerson(Person person)
    {
        SubscribeToChildBirth(person);
        SubscribePersonToYearTick(person);
        _population.Add(person);
    }

    public void RemovePerson(Person person)
    {
        UnsubscribePersonFromYearTick(person);   
        UnsubscribeFromChildBirth(person);
    }

    private void SubscribePersonToYearTick(Person person) => YearTick += person.OnYearTick;
    
    private void UnsubscribePersonFromYearTick(Person person) => YearTick -= person.OnYearTick;

    private void SubscribeToChildBirth(Person person) => person.ChildBirth += OnChildBirth;
    
    private void UnsubscribeFromChildBirth(Person person) => person.ChildBirth -= OnChildBirth;
    
    private void OnYearTick() => YearTick?.Invoke(this, EventArgs.Empty);

    private void OnChildBirth(object sender, EventArgs e)
    {
        var gender = ProbabilityCalculator.IsEventHappened(_config.ChildGenderProbability) ? "Female" : "Male";
        var child = new Person(CurrentYear, gender, _config, _deathRules);

        AddPerson(child);
    }

    private void WritePopulationData()
    {
        int total = _population.Count(p => p.IsAlive);

        int male_0_18 = _population.Count(p => p.IsAlive && p.Gender == "Male" && CurrentYear - p.BirthYear <= 18);
        int male_19_45 = _population.Count(p => p.IsAlive && p.Gender == "Male" && CurrentYear - p.BirthYear is >= 19 and <= 45);
        int male_46_65 = _population.Count(p => p.IsAlive && p.Gender == "Male" && CurrentYear - p.BirthYear is >= 46 and <= 65);
        int male_66_100 = _population.Count(p => p.IsAlive && p.Gender == "Male" && CurrentYear - p.BirthYear >= 66);

        int female_0_18 = _population.Count(p => p.IsAlive && p.Gender == "Female" && CurrentYear - p.BirthYear <= 18);
        int female_19_45 = _population.Count(p => p.IsAlive && p.Gender == "Female" && CurrentYear - p.BirthYear is >= 19 and <= 45);
        int female_46_65 = _population.Count(p => p.IsAlive && p.Gender == "Female" && CurrentYear - p.BirthYear is >= 46 and <= 65);
        int female_66_100 = _population.Count(p => p.IsAlive && p.Gender == "Female" && CurrentYear - p.BirthYear >= 66);

        string line = $"{CurrentYear},{total},{male_0_18},{male_19_45},{male_46_65},{male_66_100},{female_0_18},{female_19_45},{female_46_65},{female_66_100}\n";
        File.AppendAllText(_outputFile, line);
    }
}
