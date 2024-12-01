namespace Demographic;

public class Person
{
    private readonly Config _config;
    private readonly Dictionary<(int MinAge, int MaxAge), (double Male, double Female)> _deathRules;
    
    public int BirthYear { get; }
    public int? DeathYear { get; private set; }
    public bool IsAlive => DeathYear == null;
    public string Gender { get; }

    public event EventHandler ChildBirth;

    public Person(int birthYear, string gender, Config config,
        Dictionary<(int MinAge, int MaxAge), (double Male, double Female)> deathRules)
    {
        BirthYear = birthYear;
        Gender = gender;
        _config = config;
        _deathRules = deathRules;
    }

    public void OnYearTick(object sender, EventArgs e)
    {
        if (!IsAlive) return;

        var engine = (IEngine)sender;
        var currentYear = engine.CurrentYear;
        var age = currentYear - BirthYear;

        if (ShouldDie(age))
        {
            DeathYear = currentYear;
            engine.RemovePerson(this);
            return;
        }

        if (Gender == "Female" && age >= _config.ReproductiveAgeFemaleMin &&
            age <= _config.ReproductiveAgeFemaleMax && ShouldGiveBirth())
        {
            OnChildBirth();
        }
    }

    private void OnChildBirth() => ChildBirth?.Invoke(this, EventArgs.Empty);

    private bool ShouldGiveBirth() => ProbabilityCalculator.IsEventHappened(_config.BirthProbability);

    private bool ShouldDie(int age) => ProbabilityCalculator.IsEventHappened(GetDeathProbability(age, Gender));

    private double GetDeathProbability(int age, string gender)
    {
        // TODO: What about bin search?
        foreach (var rule in _deathRules)
        {
            var (minAge, maxAge) = rule.Key;
            if (age >= minAge && age <= maxAge)
            {
                return gender == "Male" ? rule.Value.Male : rule.Value.Female;
            }
        }

        throw new ArgumentException($"No death probability defined for age {age}.");
    }
}