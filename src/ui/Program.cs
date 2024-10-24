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
    private Button _exitButton;

    private Panel _radioButtons;
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

    [STAThread]
    static public void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Program());
    }

    public Program()
    {
        Size = new Size(500, 700);
        this.FormClosing += new FormClosingEventHandler(Program_FormClosing);

        SetUpRadioBoxPanel();
        SetUpSimulationPausingButton();
        SetUpSimulationStartingButton();
        SetUpParametersLabel();
        SetUpExitButton();

        SetUpAdjustmentComponents(
            ref _lethalitySlider,
            ref _lethalityLabel,
            3,
            1,
            10,
            30,
            _lethalitySlider_Scroll
        );
        SetUpAdjustmentComponents(
            ref _diseaseDurationSlider,
            ref _diseaseDurationLabel,
            5,
            2,
            60,
            100,
            _diseaseDurationSlider_Scroll
        );
        SetUpAdjustmentComponents(
            ref _communicabilitySlider,
            ref _communicabilityLabel,
            7,
            1,
            100,
            135,
            _communicabilitySlider_Scroll
        );
        SetUpAdjustmentComponents(
            ref _populationSlider,
            ref _populationLabel,
            9,
            0,
            300,
            150,
            _populationSlider_Scroll
        );
    }

    private delegate void ScrollMethod(object sender, EventArgs e);

    private void SetUpExitButton()
    {
        _exitButton = new Button();
        _exitButton.Location = new Point(
            (this.ClientSize.Width - _exitButton.Width) / 2,
            _simulationPausingButton.Bottom + 10
        );
        _exitButton.Text = "Выход";
        _exitButton.Click += new EventHandler(ExitButton_Click);
        Controls.Add(_exitButton);
    }

    private delegate void RadioButtonClickMethod(object sender, EventArgs e);

    private void SetUpRadioBoxPanel()
    {
        _radioButtons = new Panel();
        _radioButtons.BorderStyle = BorderStyle.FixedSingle;
        _radioButtons.Width = 500;

        Label label = new Label();
        label.Font = new Font(label.Font.FontFamily, 11, label.Font.Style);
        label.Width = 300;
        label.Height = 30;
        label.Location = new Point(5, 10);
        label.Text = "Выберите сценарий:";

        _radioButtons.Controls.Add(label);

        SetUpRadioBox(
            ref _singleCommunitySimulationButton,
            "Одиночное сообщество",
            0,
            _singleCommunitySimulationButton_Click
        );
        SetUpRadioBox(
            ref _shoppingCommunitySimulationButton,
            "Точка интереса",
            2,
            _shoppingCommunitySimulationButton_Click
        );
        SetUpRadioBox(
            ref _multiCommunitySimulationButton,
            "Мульти сообщество",
            1,
            _multiCommunitySimulationButton_Click
        );

        Controls.Add(_radioButtons);
    }

    private void SetUpRadioBox(
        ref RadioButton radioButton,
        string label,
        ushort order,
        RadioButtonClickMethod clickEvent
    )
    {
        radioButton = new RadioButton();

        radioButton.AutoCheck = true;
        radioButton.Text = label;
        radioButton.Height = 50;
        radioButton.Width = 150;
        radioButton.Location = new Point(50 + order * 150, 50);

        if (order == 0u)
            radioButton.Checked = true;

        radioButton.Click += new System.EventHandler(clickEvent);

        _radioButtons.Controls.Add(radioButton);
    }

    private void SetUpAdjustmentComponents(
        ref TrackBar slider,
        ref Label textLabel,
        ushort order,
        int sliderMinValue,
        int sliderMaxValue,
        int textLabelWidth,
        ScrollMethod sliderScrollMethod
    )
    {
        int elementHeight = 50;
        int posY = order * elementHeight;

        slider = new TrackBar();
        textLabel = new Label();

        // Настраиваем Label
        textLabel.Height = elementHeight;
        textLabel.Width = textLabelWidth + 75;
        textLabel.Location = new Point(200, posY + elementHeight);

        // Настраиваем TrackBar
        slider.Minimum = sliderMinValue;
        slider.Maximum = sliderMaxValue;
        slider.Value = (sliderMaxValue + sliderMinValue) / 2;
        slider.Height = elementHeight;
        slider.Width = 400;
        slider.Location = new Point(50, posY);
        slider.TickFrequency = (sliderMaxValue - sliderMinValue) / 20;

        slider.Scroll += new System.EventHandler(sliderScrollMethod);

        Controls.Add(slider);
        Controls.Add(textLabel);

        sliderScrollMethod(null, null);
    }


    private void SetUpParametersLabel()
    {
        Label label = new Label();
        label.Font = new Font(label.Font.FontFamily, 11, label.Font.Style);
        label.Width = 400;
        label.Height = 20;
        label.Location = new Point(5, 110);
        label.Text = "Настройте параметры симуляции:";

        Controls.Add(label);
    }


    private void SetUpSimulationStartingButton()
    {
        _simulationStartingButton = new Button();
        _simulationStartingButton.Location = new Point(150, 550);
        _simulationStartingButton.Text = "Запуск";

        _simulationStartingButton.Click += new EventHandler(Button_Click);

        Controls.Add(_simulationStartingButton);
    }


    private void SetUpSimulationPausingButton()
    {
        _simulationPausingButton = new Button();
        _simulationPausingButton.Location = new Point(250, 550);
        _simulationPausingButton.Text = "Пауза";

        _simulationPausingButton.Click += new EventHandler(PauseSimulation);

        Controls.Add(_simulationPausingButton);
    }


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

    private void _populationSlider_Scroll(object sender, EventArgs e)
    {
        _populationLabel.Text = "Популяция: " + _populationSlider.Value.ToString() + " человек";
    }


    private void _lethalitySlider_Scroll(object sender, EventArgs e)
    {
        _lethalityLabel.Text = "Летальность: " + _lethalitySlider.Value.ToString() + "%";
    }

    private void _diseaseDurationSlider_Scroll(object sender, EventArgs e)
    {
        _diseaseDurationLabel.Text =
            "Длительность: " + _diseaseDurationSlider.Value.ToString() + " дней";
    }


    private void _communicabilitySlider_Scroll(object sender, EventArgs e)
    {
        _communicabilityLabel.Text =
            "Коммуникабельность: " + (float)_communicabilitySlider.Value / 4 + "%";
    }


    private void _singleCommunitySimulationButton_Click(object sender, EventArgs e)
    {
        if (_simulationThread == null)
            _simulation = new SingleCommunitySimulation((uint)_populationSlider.Value);
    }


    private void _multiCommunitySimulationButton_Click(object sender, EventArgs e)
    {
        if (_simulationThread == null)
            _simulation = new MultigroupCommunitySimulation(4, (uint)_populationSlider.Value);
    }


    private void _shoppingCommunitySimulationButton_Click(object sender, EventArgs e)
    {
        Microsoft.Xna.Framework.Point centerPoint = new Microsoft.Xna.Framework.Point(
            Simulation.s_SimulationWidth / 2,
            Simulation.s_SimulationWidth / 2
        );
        _simulation = new ShoppingCommunitySimulation(centerPoint, (uint)_populationSlider.Value);
    }


    private void ExitButton_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

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
