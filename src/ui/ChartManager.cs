using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Charting;
using EpidemicSimulation;

/**
    Класс управляющий классом Graph, предоставляя базовую настройку и позволяя легко обновлять отображаемые статистики
 */

namespace EpidemicSimulation
{
    class ChartManager
    {
        private Graph _graph;
        private Simulation _simulation;
        private GraphicsDevice _graphicsDevice;
        private Vector2 _position;
        private Point _size;

        private List<float> _susceptibleTimeSeries = new List<float>();
        private List<float> _infectedTimeSeries = new List<float>();
        private List<float> _recoveredTimeSeries = new List<float>();
        private List<float> _deadTimeSeries = new List<float>();

        /**
            Конструктор устанавливает позицию графика внутри окна, размер графика в пикселях,
            экземпляр Simulation для получения данных для построения и объект графической карты.

            @param position Позиция графика внутри окна
            @param size Размер графика в пикселях
            @param simulation Экземпляр Simulation для получения данных
            @param graphicsDevice Объект графической карты
        */

        public ChartManager(
            Vector2 position,
            Point size,
            Simulation simulation,
            GraphicsDevice graphicsDevice)
        {
            _position = position;
            _size = size;
            _simulation = simulation;
            _graphicsDevice = graphicsDevice;
        }

        /**
            Обновляет статистику, извлекая из предоставленного экземпляра Simulation
        */

        public void Update()
        {
            UpdateInfectedPopulation();
            UpdateSusceptiblePopulation();
            UpdateRecoveredPopulation();
            UpdateDeadPopulation();
        }

        /**
            Рисует все данные: зараженных в красном, здоровых в синем,
            вылеченных в зеленом и мертвых в сером.
        */

        public void Draw()
        {
            _graph.Draw(_infectedTimeSeries, Color.Red);
            _graph.Draw(_susceptibleTimeSeries, Color.Blue);
            _graph.Draw(_recoveredTimeSeries, Color.Green);
            _graph.Draw(_deadTimeSeries, Color.Gray);
        }

        /**
            Инициализирует экземпляр Graph и настраивает его
        */

        public void LoadContent()
        {
            _graph = new Graph(_graphicsDevice, _size);
            _graph.Position = _position;
            _graph.Size = _size;
            _graph.MaxValue = 50;
            _graph.Type = Charting.Graph.GraphType.Line;
        }

        /**
            Обновляет количество зараженных
        */

        private void UpdateInfectedPopulation()
        {
            int currentNum = _simulation.GenerateOutputLists()["Infectious"];
            _infectedTimeSeries.Add( (float) currentNum );
        }

        /**
            Обновляет количество здоровых
        */

        private void UpdateSusceptiblePopulation()
        {
            int currentNum = _simulation.GenerateOutputLists()["Susceptible"];
            _susceptibleTimeSeries.Add( (float) currentNum );
        }

        /**
            Обновляет количество вылеченных
        */

        private void UpdateRecoveredPopulation()
        {
            int currentNum = _simulation.GenerateOutputLists()["Recovered"];
            _recoveredTimeSeries.Add( (float) currentNum );
        }

        /**
            Обновляет количество мертвых
        */

        private void UpdateDeadPopulation()
        {
            int currentNum = _simulation.GenerateOutputLists()["Dead"];
            _deadTimeSeries.Add( (float) currentNum );
        }
    }

}
