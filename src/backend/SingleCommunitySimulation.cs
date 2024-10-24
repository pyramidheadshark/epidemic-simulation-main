using EpidemicSimulation;
using System.Collections.Generic;

namespace EpidemicSimulation
{
     /**
        Класс сценария общества - 
        обрабатывающий высокоуровневые события симуляции,
        такие как пауза, завершение, запуск и предоставление данных,
        а также предоставляет упрощенный конструктор

    */
    class SingleCommunitySimulation : Simulation, ISimulation
    {
        public bool IsRunning { get; private set; } = false;
        /**
            Конструктор создающий экземпляр класса Simulation, принимая в качестве
            параметра размер популяции.

        @param population количество людей для симуляции
        .*/
        public SingleCommunitySimulation(uint population = 20, uint infected = 2):
            base(population, infected)
        {

        }
         /**
            Запускает симуляцию.
        */
        public void Start()
        {
            IsRunning = true;
            Run();
        }
        /**
            Закрывает симуляцию.
        */
        public void Close()
        {
            Exit();
        }


        /**
            Возвращает количество умерших, здоровых и вылечившихся.
        */

        public Dictionary<string, int> GetSimulationData()
        {
            return GenerateOutputLists();
        }
    }
}
