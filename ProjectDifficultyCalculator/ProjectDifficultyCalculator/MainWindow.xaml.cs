using ProjectDifficultyCalculator.CustomControls;
using ProjectDifficultyCalculator.DefaultLanguageSelector;
using ProjectDifficultyCalculator.FunctionalPoints;
using ProjectDifficultyCalculator.Logic.COCOMO;
using ProjectDifficultyCalculator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjectDifficultyCalculator.Logic.UFP;

namespace ProjectDifficultyCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<SelectorControl> CostDriverControls;
        private List<SelectorControl> ScaleFactorsControls;
        private List<LanguageSize> _languageSizes;
        private CocomoCalculator CocomoCalculator;
        private UfpCalculator UfpCalculator;
        private CocomoProperties CocomoProperties => CocomoCalculator.Properties;
        private UfpProperties UfpProperties => UfpCalculator.Properties;

        public CurrentStep _currentStep { get; set; }

        public MainWindow()
        {
            CostDriverControls = new List<SelectorControl>();
            ScaleFactorsControls = new List<SelectorControl>();
            InitializeComponent();
            DataContext = this;
            CocomoCalculator = new CocomoCalculator(CocomoDefaultPropertiesFactory.Create());
            UfpCalculator = new UfpCalculator(UfpDefaultPropertiesFactory.Create());

            for (var i = 0; i < CocomoProperties.ScaleFactors.Length; i++)
            {
                var costDriver = CocomoProperties.ScaleFactors[i];
                var selectorControl = new SelectorControl()
                {
                    Id = costDriver.ShortName,
                    ControlTooltip = costDriver.FullName,
                    Title = costDriver.ShortName,
                    Values = costDriver.Coefficients
                };

                if (i % 3 == 0)
                {
                    scaleFactorsPanel1.Children.Add(selectorControl);
                }
                else if (i % 3 == 1)
                {
                    scaleFactorsPanel2.Children.Add(selectorControl);
                }
                else if (i % 3 == 2)
                {
                    scaleFactorsPanel3.Children.Add(selectorControl);
                }

                ScaleFactorsControls.Add(selectorControl);
            }

            for (var i = 0; i < CocomoProperties.CostDrivers.Length; i++)
            {
                var costDriver = CocomoProperties.CostDrivers[i];
                var selectorControl = new SelectorControl()
                {
                    Id = costDriver.ShortName,
                    ControlTooltip = costDriver.FullName,
                    Title = costDriver.ShortName,
                    Values = costDriver.Coefficients
                };

                if (i % 3 == 0)
                {
                    costDriverPanel1.Children.Add(selectorControl);
                }
                else if (i % 3 == 1)
                {
                    costDriverPanel2.Children.Add(selectorControl);
                }
                else if (i % 3 == 2)
                {
                    costDriverPanel3.Children.Add(selectorControl);
                }

                CostDriverControls.Add(selectorControl);
            }

            _languageSizes = new List<LanguageSize>();
            _languageSizes.Add(new LanguageSize("C#", 120));
            _languageSizes.Add(new LanguageSize("Total", 1020));

            SizesDataGrid.ItemsSource = _languageSizes;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!uint.TryParse(slocTextBox.Text, out var sloc))
            {
                ShowError("Invalid SLOC value!");
                return;
            }

            if (!double.TryParse(brakTextBox.Text, out var brak) || brak < 0 || brak > 100)
            {
                ShowError("Invalid BRAK value!" + Environment.NewLine + "BRAK must be >= 0 and <= 100.");
                return;
            }

            var scaleFactors = ScaleFactorsControls.Select(c => c.GetValue())
                .Zip(CocomoProperties.ScaleFactors)
                .Select(pair => pair.Second.Coefficients[pair.First])
                .ToArray();

            var costDrivers = CostDriverControls.Select(c => c.GetValue())
                .Zip(CocomoProperties.CostDrivers)
                .Select(pair => pair.Second.Coefficients[pair.First])
                .ToArray();

            var result = CocomoCalculator.CalculateComplexityPM(sloc, brak, scaleFactors, costDrivers);
            MessageBox.Show(this, "Result: " + result + " person-months.", "Difficulty", MessageBoxButton.OK);
        }

        private void ButtonFP_Click(object sender, RoutedEventArgs e)
        {
            // In reality something should happen here, but I'm not sure what
            // TODO
        }

        private void TextBox_IntegerValidation(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_DoubleValidation(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9.,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BSizeCalculation_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentStep != CurrentStep.SizeCalculation)
            {
                CurrentStep = CurrentStep.SizeCalculation;
            }
        }
        private void BScaleFactorsCalculation_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentStep != CurrentStep.ScaleFactorsCalculation)
            {
                CurrentStep = CurrentStep.ScaleFactorsCalculation;
            }
        }
        private void BCostDriverCalculation_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentStep != CurrentStep.CostDriverCalculation)
            {
                CurrentStep = CurrentStep.CostDriverCalculation;
            }
        }
        private void BResults_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentStep != CurrentStep.Results)
            {
                CurrentStep = CurrentStep.Results;
            }
        }


        public CurrentStep CurrentStep
        {
            get { return _currentStep; }
            private set
            {
                _currentStep = value;
                OnPropertyChanged("CurrentStep");
            }
        }


        private Dictionary<string, double[]> _slocLangsFp;
        private Dictionary<string, double[]> _brakLangsFp;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_slocLangsFp == null)
            {
                var dlsWindow = new DefaultLanguageSelectorWindow(UfpProperties.LanguagesSlocPerFpDict.Keys);
                dlsWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                dlsWindow.ShowDialog();

                var selectedLanguage = dlsWindow.SelectedLanguage;
                dlsWindow.Close();
                if (selectedLanguage == null)
                {
                    return;
                }

                _slocLangsFp = new Dictionary<string, double[]>();
                _slocLangsFp.Add(selectedLanguage, new double[15]);
            }

            var fpWindow = new FunctionalPointsWindow(UfpProperties.LanguagesSlocPerFpDict.Keys, _slocLangsFp);
            fpWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Hide();
            fpWindow.ShowDialog();
            Show();

            if (!fpWindow.Hidden)
            {
                fpWindow.Close();
                return;
            }
            var languages = fpWindow.GetLanguages();
            _slocLangsFp = languages;
            _languageSizes.Clear();
            foreach (var language in languages)
            {
                _languageSizes.Add(new LanguageSize(language.Key, 100));
            }
            _languageSizes.Add(new LanguageSize("Total", _languageSizes.Select(el => el.LinesAmount).Sum()));
            SizesDataGrid.Items.Refresh();
            fpWindow.Close();
        }
    }

    public enum CurrentStep
    {
        SizeCalculation,
        ScaleFactorsCalculation,
        CostDriverCalculation,
        Results
    }
}
