using Microsoft.Xna.Framework;

namespace EpidemicSimulation
{
    /**
        Класс, представляющий человека, который здоров, но может заболеть.
    */

    class Susceptible: Person
    {
        /**
            Конструктор, создающий конкретный экземпляр здорового человека.

            @param simulationRect Прямоугольник, определяющий область, в которой этот человек может двигаться
            @param startPosition Позиция, где человек находится в начале симуляции или после добавления в симуляцию
            @param MovementVector Вектор, определяющий движение этого человека
            @param immunity Численное представление иммунитета этого человека
            @param repulsionRate Скорость, с которой человек отталкивается от других. Она снижает риск заражения
        */

        public Susceptible(Rectangle simulationRect, float? immunity = null, int? repulsionRate = null)
            : base(simulationRect, immunity, repulsionRate)
        {

        }

        /**
            Конструктор, создающий конкретный экземпляр здорового человека.

            @param simulationRect Прямоугольник, определяющий область, в которой этот человек может двигаться
            @param immunity Численное представление иммунитета этого человека
            @param repulsionRate Скорость, с которой человек отталкивается от других. Она снижает риск заражения
        */

        public Susceptible(Rectangle simulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(simulationRect, startPosition, MovementVector, immunity, repulsionRate)
        {

        }

        /**
            Возвращает тип человека в виде label
        */

        public override string Type()
        {
            return "Susceptible";
        }
    }
}
