using ProjectDifficultyCalculator.CustomControls;
using ProjectDifficultyCalculator.DefaultLanguageSelector;
using ProjectDifficultyCalculator.FunctionalPoints;
using ProjectDifficultyCalculator.Logic.COCOMO;
using ProjectDifficultyCalculator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        private Dictionary<string, uint[][]> _slocLangsFp;
        private Dictionary<string, uint[][]> _brakLangsFp;
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

            _languageSizes = new List<LanguageSize>
            {
                new LanguageSize(UfpProperties.LanguagesSlocPerFpDict.Keys.First(), 0),
                new LanguageSize("Total", 0)
            };

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

        private uint[][] CreateEmptyArraysForFP() => UfpProperties.Weights.Select(a => new uint[a.Coefficients.Length]).ToArray();

        private void SynchronizeDictionaries(bool checkArrays)
        {
            var keysToRemove = _brakLangsFp.Keys.Where(k => !_slocLangsFp.ContainsKey(k));
            foreach (var key in keysToRemove) _brakLangsFp.Remove(key);
            foreach (var (key, slocArrs) in _slocLangsFp)
            {
                if(_brakLangsFp.TryGetValue(key, out var brakArrs))
                {
                    if(!checkArrays) continue;
                    for (var i = 0; i < slocArrs.Length; i++)
                    {
                        var slocRow = slocArrs[i];
                        var brakRow = brakArrs[i];
                        for (var j = 0; j < slocRow.Length; j++)
                        {
                            if (brakRow[j] > slocRow[j]) brakRow[j] = slocRow[j];
                        }
                    }
                }
                else
                {
                    _brakLangsFp.Add(key, CreateEmptyArraysForFP());
                }
            }
        }

        private void UpdateTable()
        {
            _languageSizes.Clear();

            var totalSum = 0u;
            var changeSum = 0u;

            var changed = _brakLangsFp.ToDictionary(p => p.Key, p => UfpCalculator.CalculateSloc(p.Key, p.Value));
            foreach (var (language, complexity) in _slocLangsFp)
            {
                var total = UfpCalculator.CalculateSloc(language, complexity);
                changed.TryGetValue(language, out var change);
                _languageSizes.Add(new LanguageSize(language, total, change));
                totalSum += total;
                changeSum += change;
            }

            _languageSizes.Add(new LanguageSize("Total", totalSum, changeSum));
            SizesDataGrid.Items.Refresh();

            if (totalSum > 0)
            {
                slocTextBox.Text = ((totalSum - 1) / 1000 + 1).ToString(); // round up
                brakTextBox.Text = (100.0 * changeSum / totalSum).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                slocTextBox.Text = "0";
                brakTextBox.Text = "0";
            }
        }

        private void SlocButtonFP_Click(object sender, RoutedEventArgs e)
        {
            if (_slocLangsFp == null)
            {
                var dlsWindow = new DefaultLanguageSelectorWindow(UfpProperties.LanguagesSlocPerFpDict.Keys);
                dlsWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                dlsWindow.ShowDialog();

                var selectedLanguage = dlsWindow.SelectedLanguage;
                dlsWindow.Close();
                if (selectedLanguage == null) return;

                _slocLangsFp = new Dictionary<string, uint[][]> { { selectedLanguage, CreateEmptyArraysForFP() } };
                _brakLangsFp = new Dictionary<string, uint[][]> { { selectedLanguage, CreateEmptyArraysForFP() } };
            }

            CalculateFP(_slocLangsFp);
            SynchronizeDictionaries(true);
        }

        private void BrakButtonFP_Click(object sender, RoutedEventArgs e)
        {
            if (_brakLangsFp == null)
            {
                MessageBox.Show(this, "Must set SLOC before BRAK.", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            CalculateFP(_brakLangsFp, _slocLangsFp);
            SynchronizeDictionaries(false);
        }

        private void CalculateFP(Dictionary<string, uint[][]> current, Dictionary<string, uint[][]> max = null)
        {

            var fpWindow = new FunctionalPointsWindow(UfpProperties.LanguagesSlocPerFpDict.Keys, current, max);
            fpWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Hide();
            fpWindow.ShowDialog();
            Show();

            if (!fpWindow.Hidden)
            {
                fpWindow.Close();
                return;
            }

            UpdateTable();
            fpWindow.Close();
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
    }

    public enum CurrentStep
    {
        SizeCalculation,
        ScaleFactorsCalculation,
        CostDriverCalculation,
        Results
    }
}
