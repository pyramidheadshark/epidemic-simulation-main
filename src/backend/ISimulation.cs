using System.Collections.Generic;

namespace EpidemicSimulation
{

    /**
        Интерфейс, представляющий абстракцию сценария эпидемической ситуации (abstraction of epidemic scenario)
    */

    public interface ISimulation
    {
        void Start();
        void Close();
        void Pause();
        Dictionary<string, int> GetSimulationData();
    }
}
