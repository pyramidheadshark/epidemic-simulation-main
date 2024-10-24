using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EpidemicSimulation
{
    /**
    Класс сценария магазина ("точки интереса") - 
    обрабатывающий высокоуровневые события симуляции,
    такие как пауза, завершение, запуск и предоставление данных,
    а также предоставляет упрощенный конструктор
     */
    class ShoppingCommunitySimulation : Simulation, ISimulation
    {
        public bool IsRunning { get; private set; } = false;
        /**
           Конструктор устанавливает популяцию и центральную точку.

           @param расположение центральной точки (может быть null)
           @param population количество людей для симуляции
        */
        public ShoppingCommunitySimulation(Point? centerPoint = null, uint population = 20, uint infected = 2):
         base(population, infected)
        {
            if (centerPoint.HasValue) this.CenterPoint = centerPoint;
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
