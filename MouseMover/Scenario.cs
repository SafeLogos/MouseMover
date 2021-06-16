using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSON = Newtonsoft.Json.JsonConvert;

namespace MouseMover
{
    public class Scenario
    {
        private const string _fileName = "scenarios.json";
        public string Name { get; set; }
        public List<PointWithTime> Points { get; set; }

        public bool Action() => new Mouse().MoveByTimePoints(Points.ToArray());

        public static List<Scenario> GetAllScenarios()
        {
            if (!System.IO.File.Exists(_fileName))
                return new List<Scenario>();

            string json = System.IO.File.ReadAllText(_fileName);
            return JSON.DeserializeObject<List<Scenario>>(json);
        }

        public static bool ScenarioExists(string name) => 
            GetAllScenarios().Any(s => s.Name.ToLower() == name.ToLower());

        public static List<Scenario> AddNewScenario(Scenario scenario)
        {
            if (ScenarioExists(scenario.Name))
                throw new Exception("Такой сценарий уже существует");

            List<Scenario> scenarios = GetAllScenarios();
            scenarios.Add(scenario);
            string json = JSON.SerializeObject(scenarios);
            System.IO.File.WriteAllText(_fileName, json);
            return scenarios;
        }

        public static List<Scenario> DeleteScenario(Scenario scenario)
        {
            if (!ScenarioExists(scenario.Name))
                throw new Exception("Сценарий не найден");

            List<Scenario> scenarios = GetAllScenarios();
            scenarios.Remove(scenarios.FirstOrDefault(s => s.Name == scenario.Name));

            string json = JSON.SerializeObject(scenarios);
            System.IO.File.WriteAllText(_fileName, json);
            return scenarios;
        }

    }
}
