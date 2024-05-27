using System;
using System.Collections.Generic;
using System.IO;

namespace EpidemicSimulation
{

    /**
        Класс, сохраняющий логи в файл в папке "log"
    */

    class StatisticsPrinter
    {
        private const string OUTPUT_FILENAME_PATH = "log/statistics.txt";
        private ISimulation _simulation;

        /**
            Конструктор принимает экземпляр ISimulation и
            присваивает его ссылку в закрытую переменную класса.

           @param simulation указывает экземпляр ISimulation, из которого будут извлекаться данные
        */

        public StatisticsPrinter(ISimulation simulation)
        {
            _simulation = simulation;
        }

        /**
            Создает новый файл, если его не существует, в противном случае ничего не создает или перезаписывает.
            Сохраняет информацию, включая дату, время, выбранный коэффициент летальности, длительность заболевания,
            коэффициент передачи, общее количество людей и окончательное количество зараженных, здоровых, вылеченных и умерших в файл.
            Закрывает этот файл.
        */

        public void Print()
        {
            FileStream fileStream = new FileStream(OUTPUT_FILENAME_PATH, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fileStream);
            var data = _simulation.GetSimulationData();

            writer.WriteLine(DateTime.Now.ToString("hh:mm:ss dd-MM-yyyy"));

            writer.WriteLine("Lethality: ~" +  (Disease.Lethality * 1000f) + "%");
            writer.WriteLine("Disease duration: " + ((Disease.Duration - 1500f) / 10f) + " days");
            writer.WriteLine("Communicability: ~" + Disease.Communicability * 16 + "%");

            writer.WriteLine("Population: " + (data["Susceptible"] + data["Infectious"] + data["Recovered"] + data["Dead"]));

            writer.WriteLine("Susceptible: " + data["Susceptible"]);
            writer.WriteLine("Infectious: " + data["Infectious"]);
            writer.WriteLine("Recovered: " + data["Recovered"]);
            writer.WriteLine("Dead: " + data["Dead"]);
            writer.WriteLine();

            writer.Close();
        }
    }
}
