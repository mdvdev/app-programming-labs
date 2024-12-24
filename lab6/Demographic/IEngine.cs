namespace Demographic;

public interface IEngine
{
    void StartSimulation(int startYear, int endYear);
    void AddPerson(Person person);
    void RemovePerson(Person person);
    int CurrentYear { get; }
}