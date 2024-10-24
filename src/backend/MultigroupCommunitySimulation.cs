using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EpidemicSimulation
{
    /**
        Класс сценария мультигруппового общества - 
        обрабатывающий высокоуровневые события симуляции,
        такие как пауза, завершение, запуск и предоставление данных,
        а также предоставляет упрощенный конструктор

    */
    class MultigroupCommunitySimulation : Simulation, ISimulation
    {
        private List<Point> CentralPoints = new List<Point>();
        private List<Rectangle> Obsticles = new List<Rectangle>();
        private int PointCooldown = 400;

        public bool IsRunning { get; private set; } = false;

        /**
        Конструктор устанавливает параметры популяции и зараженных.
        Вызывает базовый класс Simulation. Устанавливает особые границы обществ, вероятность посещения и генерирует препятствия
        @param population количество людей для симуляции
        @param infected количество зараженных для симуляции
        */
        public MultigroupCommunitySimulation(uint population, uint infected) :base(population, infected) {
            CentralPoints.Add(new Point(250,250));
            CentralPoints.Add(new Point(750,250));
            CentralPoints.Add(new Point(250,750));
            CentralPoints.Add(new Point(750,750));
            VisitingProbability = 0.0001f;

            Obsticles.Add(new Rectangle(SimulationRect.Location.X+SimulationRect.Width/2-4, 0, 8, SimulationRect.Height));
            Obsticles.Add(new Rectangle(0, SimulationRect.Location.Y+SimulationRect.Height/2-4 , SimulationRect.Width, 8));
            Person.Obsticles = Obsticles;
            foreach (Person person in this._people)
            if (Obsticles[0].Contains(person.Rect.Location) || Obsticles[1].Contains(person.Rect.Location)) person.GoToPoint(new Point(s_randomizer.Next(100,900), s_randomizer.Next(100,900)));
        }
        /**
        Обновляет базовый метод Update, чтобы поддерживать промежуток между рисованием новой точки посещения для каждого человека.
        @param GameTime объект, наследуемый от базового класса Game, который регулирует скорость вызова этого метода
        */
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (PointCooldown < 0) {CenterPoint = this.CentralPoints[Simulation.s_randomizer.Next(0,3)]; PointCooldown = 300; }
            else PointCooldown -=1;
        }
        /**
        Отображает каждый отображаемый (drawable) объект в базовой симуляции и добавляет препятствия
        @param GameTime объект, наследуемый от базового класса Game, который регулирует скорость вызова этого метода
        */
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _spriteBatch.Begin();
            _spriteBatch.Draw(Wall, Obsticles[0], Color.White);
            _spriteBatch.Draw(Wall, Obsticles[1], Color.White);
            _spriteBatch.End();
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
        public void Close() { Exit(); }

        /**
            Возвращает количество умерших, здоровых и вылечившихся.
        */


        public Dictionary<string, int> GetSimulationData()
        {
            return GenerateOutputLists();
        }
     }
    }
