using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    /**
    Класс, представляющий человека, который вылечился от вируса
    */

    class Recovered: Person
    {
        /**
            Конструктор, создающий конкретный экземпляр вылечившегося человека.

            @param simulationRect Прямоугольник, определяющий область, в которой этот человек может двигаться
            @param startPosition Позиция, где человек находится в начале симуляции или после добавления в симуляцию
            @param MovementVector Вектор, определяющий движение этого человека
            @param immunity Численное представление иммунитета этого человека
            @param repulsionRate Скорость, с которой человек отталкивается от других. Она снижает риск заражения
        */

         public Recovered(Rectangle SimulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(SimulationRect, startPosition, MovementVector, immunity, repulsionRate)
        {

        }

        /**
            Возвращает тип человека в виде label
        */

        public override string Type()
        {
            return "Recovered";
        }

    }
}
