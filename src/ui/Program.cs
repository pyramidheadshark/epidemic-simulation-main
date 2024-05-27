using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using EpidemicSimulation;

public class Program : Form
{
    private Thread _simulationThread;
    private ISimulation _simulation;
    private Button _simulationStartingButton;
    private Button _simulationPausingButton;

    private GroupBox _radioButtons;
    private RadioButton _singleCommunitySimulationButton = new RadioButton();
    private RadioButton _multiCommunitySimulationButton = new RadioButton();
    private RadioButton _shoppingCommunitySimulationButton = new RadioButton();

    private TrackBar _populationSlider;
    private TrackBar _lethalitySlider;
    private TrackBar _diseaseDurationSlider;
    private TrackBar _communicabilitySlider;

    private Label _populationLabel;
    private Label _lethalityLabel;
    private Label _diseaseDurationLabel;
    private Label _communicabilityLabel;

    static public void Main()
    {
        Application.Run( new Program() );
    }

    /**
        Конструктор настраивает позиции всех компонентов пользовательского интерфейса, 
        таких как кнопки, радиокнопки, слайдеры и размер главного окна.
    */

    public Program()
    {
        Size = new Size(500, 800);
        this.FormClosing += new FormClosingEventHandler(Program_FormClosing);

        SetUpRadioBoxPanel();
        SetUpSimulationPausingButton();
        SetUpSimulationStartingButton();
        SetUpParametersLabel();

        SetUpAdjustmentComponents(ref _lethalitySlider, ref _lethalityLabel, 3, 1, 100, 100, _lethalitySlider_Scroll);
        SetUpAdjustmentComponents(ref _diseaseDurationSlider, ref _diseaseDurationLabel, 5, 2, 180, 100, _diseaseDurationSlider_Scroll);
        SetUpAdjustmentComponents(ref _communicabilitySlider, ref _communicabilityLabel, 7, 1, 100, 135, _communicabilitySlider_Scroll);
        SetUpAdjustmentComponents(ref _populationSlider, ref _populationLabel, 9, 0, 500, 150, _populationSlider_Scroll);
    }

    private delegate void ScrollMethod(object sender, EventArgs e);

    private delegate void RadioButtonClickMethod(object sender, EventArgs e);

    /**
        Настраивает панель нескольких радиокнопок,
        которая позволяет пользователю выбрать желаемую сценарий симуляции:
        сценарий для одного общества, сценарий для мультигруппового общества
        или сценарий для сценария магазина.
    */

    private void SetUpRadioBoxPanel()
    {
        _radioButtons = new GroupBox();
        _radioButtons.Width = 500;

        Label label = new Label();
        label.Width = 150;
        label.Height = 50;
        label.Location = new Point(5, 10);
        label.Text = "Выберите сценарий:";


        SetUpRadioBox(ref _singleCommunitySimulationButton, "Одиночное сообщ.", 0, _singleCommunitySimulationButton_Click);
        SetUpRadioBox(ref _shoppingCommunitySimulationButton, "Точка интереса", 1, _shoppingCommunitySimulationButton_Click);
        SetUpRadioBox(ref _multiCommunitySimulationButton, "Мульти сообщ.", 2, _multiCommunitySimulationButton_Click);

        _radioButtons.Controls.Add(label);
        Controls.Add(_radioButtons);
    }

    /**
        Настраивает одну радиокнопку, давая ей подходящую метку, размер, ширину и позицию, а также давая ей подходящее действие - изменение сцены на указанную.
    
        @param radioButton Ссылка на объект радиокнопки, который будет настроен
        @param label Текстовая метка для радиокнопки
        @param order Целое число, определяющее порядок среди радиокнопок
        @param clickEvent Экземпляр класса RadioButtonClickMethod, представляющий действие, которое произойдет после выбора этой радиокнопки
    */

    private void SetUpRadioBox(
        ref RadioButton radioButton,
        string label,
        ushort order,
        RadioButtonClickMethod clickEvent)
    {
        radioButton = new RadioButton();
        radioButton.AutoCheck = true;
        radioButton.Text = label;
        radioButton.Height = 50;
        radioButton.Width = 100;
        radioButton.Location = new Point( (Simulation.s_SimulationWidth/3) - (order*radioButton.Width), 25);

        if (order == 0u)
            radioButton.Checked = true;

        radioButton.Click += new System.EventHandler(clickEvent);

        _radioButtons.Controls.Add(radioButton);
    }

    /**
        Настраивает пару слайдера и текстовой метки, отображающей текущее значение слайдера.
    
        @param slider Ссылка на слайдер
        @param text Текстовая метка для слайдера
        @param order Целое число, определяющее порядок среди пар слайдеров и их текстовых меток
        @param sliderMinValue Минимальное значение, которое может принимать слайдер
        @param sliderMaxValue Максимальное значение, которое может принимать слайдер
        @param textLabelWidth Ширина текстовой метки слайдера в пикселях
        @param sliderScrollMethod Метод, который будет вызываться после использования слайдера
    */ 

    private void SetUpAdjustmentComponents(
        ref TrackBar slider,
        ref Label textLabel,
        ushort order,
        int sliderMinValue,
        int sliderMaxValue,
        int textLabelWidth,
        ScrollMethod sliderScrollMethod)
    {
        int elementHeight = 50;
        int posY = order * elementHeight;

        slider = new TrackBar();
        textLabel = new Label();

        textLabel.Height = elementHeight;
        textLabel.Width = textLabelWidth;
        textLabel.Location = new Point(200, posY + elementHeight);

        slider.Minimum = sliderMinValue;
        slider.Maximum = sliderMaxValue;
        slider.Value = (sliderMaxValue + sliderMinValue) / 2;
        slider.Height = elementHeight;
        slider.Width = 400;
        slider.Location = new Point(50, posY);

        slider.Scroll += new System.EventHandler(sliderScrollMethod);

        Controls.Add(slider);
        Controls.Add(textLabel);

        sliderScrollMethod(null, null);
    }

    /**
        Настраивает заголовок для параметров эпидемии.
    */

    private void SetUpParametersLabel()
    {
        Label label = new Label();
        label.Width = 250;
        label.Height = 20;
        label.Location = new Point(5, 110);
        label.Text = "Настройте параметры симуляции:";

        Controls.Add(label);
    }

    /**
        Настраивает кнопку запуска всей симуляции.
    */

    private void SetUpSimulationStartingButton()
    {
        _simulationStartingButton = new Button();
        _simulationStartingButton.Location = new Point(150, 550);
        _simulationStartingButton.Text = "Запуск";

        _simulationStartingButton.Click += new EventHandler(Button_Click);

        Controls.Add(_simulationStartingButton);
    }

    /**
        Настраивает кнопку приостановки всей симуляции.
    */

    private void SetUpSimulationPausingButton()
    {
        _simulationPausingButton = new Button();
        _simulationPausingButton.Location = new Point(250, 550);
        _simulationPausingButton.Text = "Пауза";

        _simulationPausingButton.Click += new EventHandler(PauseSimulation);

        Controls.Add(_simulationPausingButton);
    }

    /**
        Определяет действие, которое произойдет после нажатия кнопки 'Старт',
        включая настройку параметров заболевания на основе данных,
        введенных пользователем, и запуск окна симуляции в отдельном потоке,
        чтобы не блокировать пользовательский интерфейс.
    */

    private void Button_Click(object sender, EventArgs e)
    {
        Disease.s_SetUpParams(
            _lethalitySlider.Value / 1000f,
            10f * _diseaseDurationSlider.Value + 1500f,
            _communicabilitySlider.Value / (4f * 4f)
        );

        if (_simulation == null)
            this._singleCommunitySimulationButton_Click(null, null);

        _simulationThread = new Thread(_simulation.Start);
        _simulationThread.Start();
    }

    /**
        Обрабатывает приостановку и возобновление симуляции через кнопку.
    */

    private void PauseSimulation(object sender, EventArgs e)
    {
        if (_simulation != null)
        {
            if (_simulationPausingButton.Text == "Пауза")
            {
                _simulation.Pause();
                _simulationPausingButton.Text = "Пуск";
            }
            else
            {
                _simulation.Pause();
                _simulationPausingButton.Text = "Пауза";
            }

        }
    }

    /**
        Определяет действие, которое произойдет после прокрутки слайдера численности популяции.
    */

    private void _populationSlider_Scroll(object sender, EventArgs e)
    {
        _populationLabel.Text = "Популяция: " + _populationSlider.Value.ToString() + " человек";
    }

    /**
       Определяет действие, которое произойдет после прокрутки слайдера летальности заболевания.
    */

    private void _lethalitySlider_Scroll(object sender, EventArgs e)
    {
        _lethalityLabel.Text = "Летальность: " + _lethalitySlider.Value.ToString() + "%";
    }

    /**
        Определяет действие, которое произойдет после прокрутки слайдера продолжительности заболевания.
    */

    private void _diseaseDurationSlider_Scroll(object sender, EventArgs e)
    {
        _diseaseDurationLabel.Text = "Длительность: " + _diseaseDurationSlider.Value.ToString() + " дней";
    }

    /**
        Определяет действие, которое произойдет после прокрутки слайдера передачи заболевания.
    */

    private void _communicabilitySlider_Scroll(object sender, EventArgs e)
    {
        _communicabilityLabel.Text = "Коммуникабельность: " + (float) _communicabilitySlider.Value / 4 + "%";
    }

    /**
        Назначает экземпляр симуляции для общества в качестве текущей симуляции после нажатия на radio box.
    */

    private void _singleCommunitySimulationButton_Click(object sender, EventArgs e)
    {
        if (_simulationThread == null)
            _simulation = new SingleCommunitySimulation( (uint) _populationSlider.Value);
    }

    /**
        Назначает экземпляр симуляции для мультигрупового общества в качестве текущей симуляции после нажатия на radio box.
    */

    private void _multiCommunitySimulationButton_Click(object sender, EventArgs e)
    {
        if (_simulationThread == null)
            _simulation = new MultigroupCommunitySimulation( 4, (uint) _populationSlider.Value);
    }

    /**
        Назначает экземпляр симуляции для сценария магазина в качестве текущей симуляции после нажатия на radio box.
    */

    private void _shoppingCommunitySimulationButton_Click(object sender, EventArgs e)
    {
        Microsoft.Xna.Framework.Point centerPoint = new Microsoft.Xna.Framework.Point(Simulation.s_SimulationWidth/2, Simulation.s_SimulationWidth/2);
        _simulation = new ShoppingCommunitySimulation(centerPoint, (uint) _populationSlider.Value);
    }

    /**
        Обрабатывает закрытие всей программы. Метод отвечает за сохранение
        статистики симуляции в внешний файл, закрытие потока симуляции
        и закрытие пользовательского интерфейса.
    */

    private void Program_FormClosing(Object sender, FormClosingEventArgs e)
    {
        if (_simulation != null)
        {
            StatisticsPrinter printer = new StatisticsPrinter(_simulation);
            printer.Print();
        }

        if (_simulationThread != null)
            _simulationThread.Abort();

        Close();
    }
}
