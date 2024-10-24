namespace EpidemicSimulation
{
    public class Disease
    {
        /**
        Класс, представляющий экземпляр болезни, содержит все параметры, используемые для моделирования его поведения.
        */
        public static float Lethality = 0.1f; //  <0.0001 - 0.1>

        public static float Duration = 2000f;// <1500 - 2500>

        public static float Communicability = 0.03f; // <0.001 - 0.1>

        public static float RequiredFieldIntersetion = 0.3f; // <0.05 - 0.5>

        /**
        Метод, отвечающий за установку выбранных параметров из UI.
        @param lethality коэффициент смертности (0 - 1)
        @param duration количество вызовов метода Update() симуляции, необходимых для изменения state (состояния)
        @param communicability коэффициент вероятности передачи заболевания (0 - 1)
        @param requiredFieldIntersetion минимальное значение пересечения внешнего поля (0 - 1)
        */
        public static void s_SetUpParams(
            float? lethality = null,
            float? duration = null,
            float? communicability = null,
            float? requiredFieldIntersetion = null)
        {
            Disease.Lethality = lethality*0.1f ?? Disease.Lethality;
            Disease.Duration = duration*0.5f ?? Disease.Duration;
            Disease.Communicability = communicability*0.05f ?? Disease.Communicability;
            Disease.RequiredFieldIntersetion = requiredFieldIntersetion ?? Disease.RequiredFieldIntersetion;
        }
    }
}
