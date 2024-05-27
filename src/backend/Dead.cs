using Microsoft.Xna.Framework;

namespace EpidemicSimulation {

    /**
        Класс, представляющий человека, который умер из-за вируса
    */


    class Dead: Person
    {
        /**
            Конструктор, вызываемый при создании экземпляра класса умершего человека.

            @param simulationRect Прямоугольник, определяющий область, в которой этот человек может двигаться
            @param startPosition Позиция, где человек находится в начале симуляции или после добавления в симуляцию
            @param MovementVector Вектор, определяющий движение этого человека
            @param immunity Численное представление иммунитета этого человека
            @param repulsionRate Скорость, с которой человек отталкивается от других. Она снижает риск заражения
        */

        public Dead(Rectangle SimulationRect, Point startPosition, Vector2 MovementVector, float? immunity = null, int? repulsionRate = null)
            : base(SimulationRect, startPosition, MovementVector, 0, repulsionRate)
        {

        }
        /**
            Переопределяет метод базового класса UpdateSelf(), чтобы предотвратить действия умершего человека
        */
        public override void UpdateSelf()
        {
            
        }

        /**
            Возвращает тип человека в виде label
        */

        public override string Type()
        {
            return "Dead";
        }
    }
}
