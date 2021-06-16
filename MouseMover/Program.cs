using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MouseMover
{
    class Program
    {
        private const ConsoleColor GrayColor = ConsoleColor.Gray;
        private const ConsoleColor GreenColor = ConsoleColor.Green;
        private const ConsoleColor YellowColor = ConsoleColor.Yellow;
        private const ConsoleColor RedColor = ConsoleColor.Red;

        static int Main(string[] args)
        {
            Console.ForegroundColor = GrayColor;
            string directoryPath = ".\\Scenarios";
            if (!System.IO.Directory.Exists(directoryPath))
                System.IO.Directory.CreateDirectory(directoryPath);

            if (args.Length <= 0)
            {
                Console.WriteLine("Напишите какую инструцкцию вы хотите.");
                Console.Write("Введите ");
                Console.ForegroundColor = YellowColor;
                Console.Write("--repeat ");
                Console.ForegroundColor = GrayColor;
                Console.Write("для повтора, и ");
                Console.ForegroundColor = RedColor;
                Console.Write("--delete ");
                Console.ForegroundColor = GrayColor;
                Console.WriteLine("для удаления");


                Console.WriteLine("Record - Запись движения мыши");


                List<Scenario> scenarios = Scenario.GetAllScenarios();
                Console.ForegroundColor = YellowColor;

                foreach (var item in scenarios)
                    Console.WriteLine(item.Name);

                Console.ForegroundColor = GrayColor;

                if (scenarios.Count <= 0)
                    Console.WriteLine("Доступные сценарии отсутствуют. Доступна только запись");

                Console.Write("Команда: ");
                string command = Console.ReadLine();
                if (command.ToLower() == "record")
                    return Record();

                else if (command.ToLower() == "exit")
                    return 0;

                else
                    return Choose(command);

            }

            else if (args[0].ToLower() == "--record")
                return Record();

            else
                return Choose(args.Length <= 1 ? args[0] : args[0] + " " + args[1]);
        }

        public static int CloseProgram(string text)
        {
            Console.WriteLine(text);

#if DEBUG
            Console.WriteLine("Enter Для завершения");
            Console.ReadLine();
#endif

#if !DEBUG
            System.Threading.Thread.Sleep(2000);
#endif
            return 0;
        }

        public static int Record()
        {
            Console.Write("Введите название сценария: ");
            string scenarionName = Console.ReadLine();
            if (string.IsNullOrEmpty(scenarionName))
                return CloseProgram("Название пусто");

            if (Scenario.ScenarioExists(scenarionName))
                return CloseProgram("Сценарий существует");

            Console.ForegroundColor = GreenColor;
            Console.WriteLine("Запись начата!");
            Console.ForegroundColor = GrayColor;

            List<PointWithTime> points = new Mouse().Record();

            Console.ForegroundColor = GreenColor;
            Console.WriteLine("Запись окончена!");
            Console.ForegroundColor = GrayColor;

            if (points.Count() <= 1)
                return CloseProgram("Слишком короткий сценарий");

            Scenario scenario = new Scenario() { Name = scenarionName, Points = points };
            Scenario.AddNewScenario(scenario);
            return CloseProgram("Сценарий добавлен");
        }

        public static int Choose(string command)
        {

            if (string.IsNullOrEmpty(command))
                return CloseProgram("Передана пустая строка");
            if (command.StartsWith("--"))
                command = command.Remove(0, 2);


            string[] args = command.Split(' ');
            string scenarioName = args[0];



            List<Scenario> scenarios = Scenario.GetAllScenarios();
            Scenario scenario = scenarios.FirstOrDefault(s => s.Name.ToLower() == scenarioName);
            if (scenario == null)
                return CloseProgram("Сценарий не найден");

            if (args.Length > 1)
            {
                if (args[1].ToLower() == "--repeat")
                {
#if !DEBUG
            Window.HideCurrentWindow();
#endif
                    while (scenario.Action()) { }
                }

                else if (args[1].ToLower() == "--delete")
                {
                    Scenario.DeleteScenario(scenario);
                    return CloseProgram("Сценарий удален");
                }
            }

            else
            {
#if !DEBUG
            Window.HideCurrentWindow();
#endif
                scenario.Action();
            }

            return CloseProgram("Работа завершена");
        }
    }
}
