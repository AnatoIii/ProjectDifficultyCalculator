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
using ProjectDifficultyCalculator.Serializers;

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

        private string _result;
        public string Result
        {
            get
            {
                if (string.IsNullOrEmpty(_result))
                {
                    return "No calcualtion";
                } 
                return _result;
            }
            set
            {
                var doubleValue = double.Parse(value);
                if (doubleValue > 0)
                {
                    _result = string.Format("{0:N} person-months", doubleValue);
                }
                else
                {
                    _result = "Incorrect values";
                }
                OnPropertyChanged("Result");
            }
        }

        public CurrentStep _currentStep { get; set; }

        public MainWindow()
        {
            var jsonSerializer = new JsonFileSerializer<(uint, double, int[], int[], Dictionary<string, uint[][]>, Dictionary<string, uint[][]>)>();
            (uint sloc, double brak, int[] scaleFactors, int[] costDrivers, Dictionary<string, uint[][]> slocLangsFp, Dictionary<string, uint[][]> brakLangsFp) = jsonSerializer.Load("test.json");

            InitializeComponent();
            DataContext = this;

            CocomoCalculator = new CocomoCalculator(CocomoDefaultPropertiesFactory.Create());
            UfpCalculator = new UfpCalculator(UfpDefaultPropertiesFactory.Create());
            CostDriverControls = new List<SelectorControl>();
            ScaleFactorsControls = new List<SelectorControl>();
            _languageSizes = new List<LanguageSize>
            {
                new LanguageSize(UfpProperties.LanguagesSlocPerFpDict.Keys.First(), 0),
                new LanguageSize("Total", 0)
            };
            SizesDataGrid.ItemsSource = _languageSizes;

            InitScaleFactors(scaleFactors);
            InitCostDrivers(costDrivers);
            InitSizePage(sloc, brak, slocLangsFp, brakLangsFp);
            InitResult(sloc, brak);
        }

        #region Initialization
        private void InitScaleFactors(int[] scaleFactors)
        {
            int[] values = null;
            if (scaleFactors != null && scaleFactors.Length == CocomoProperties.ScaleFactors.Length)
            {
                values = scaleFactors;
            }

            for (var i = 0; i < CocomoProperties.ScaleFactors.Length; i++)
            {
                var scaleFactor = CocomoProperties.ScaleFactors[i];
                var selectorControl = new SelectorControl(scaleFactor.Coefficients, values?[i])
                {
                    Id = scaleFactor.ShortName,
                    ControlTooltip = scaleFactor.FullName,
                    Title = scaleFactor.ShortName
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
        }

        private void InitCostDrivers(int[] costDrivers)
        {
            int[] values = null;
            if (costDrivers != null && costDrivers.Length == CocomoProperties.CostDrivers.Length)
            {
                values = costDrivers;
            }

            for (var i = 0; i < CocomoProperties.CostDrivers.Length; i++)
            {
                var costDriver = CocomoProperties.CostDrivers[i];
                var selectorControl = new SelectorControl(costDriver.Coefficients, values?[i])
                {
                    Id = costDriver.ShortName,
                    ControlTooltip = costDriver.FullName,
                    Title = costDriver.ShortName
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
        }

        private void InitSizePage(uint sloc, double brak, Dictionary<string, uint[][]> slocLangsFp, Dictionary<string, uint[][]> brakLangsFp)
        {
            if (sloc != default)
            {
                slocTextBox.Text = sloc.ToString();
            }

            if (brak != default)
            {
                brakTextBox.Text = brak.ToString();
            }

            // It's better to check if slocLangsFp.Keys are similar to brakLangsFp.Keys
            if (slocLangsFp != null && slocLangsFp.Keys.Count() > 0 && brakLangsFp.Keys != null && brakLangsFp.Keys.Count() > 0) 
            {
                _slocLangsFp = slocLangsFp;
                _brakLangsFp = brakLangsFp;
                UpdateTable();
            }
        }

        private void InitResult(uint sloc, double brak)
        {
            if (sloc == 0 || brak > 100 || brak < 0)
            {
                return;
            }

            CalculateResult(sloc, brak, false);
        }
        #endregion

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

        #region MenuButtons
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
        #endregion

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (!uint.TryParse(slocTextBox.Text, out var sloc) || sloc == 0)
            {
                ShowError("Invalid SLOC value!");
                return;
            }

            if (!double.TryParse(brakTextBox.Text, out var brak) || brak < 0 || brak > 100)
            {
                ShowError("Invalid BRAK value!" + Environment.NewLine + "BRAK must be >= 0 and <= 100.");
                return;
            }

            CalculateResult(sloc, brak);
        }

        private void CalculateResult(uint sloc, double brak, bool withSave = true)
        {
            var scaleFactorsNumbers = ScaleFactorsControls.Select(c => c.GetValue());
            var scaleFactors = scaleFactorsNumbers
                .Zip(CocomoProperties.ScaleFactors)
                .Select(pair => pair.Second.Coefficients[pair.First])
                .ToArray();

            var costDriversNumbers = CostDriverControls.Select(c => c.GetValue());
            var costDrivers = costDriversNumbers
                .Zip(CocomoProperties.CostDrivers)
                .Select(pair => pair.Second.Coefficients[pair.First])
                .ToArray();

            if (withSave)
            {
                var jsonSerializer = new JsonFileSerializer<(uint, double, int[], int[], Dictionary<string, uint[][]>, Dictionary<string, uint[][]>)>();
                jsonSerializer.Save("test.json", (sloc, brak, scaleFactorsNumbers.ToArray(), costDriversNumbers.ToArray(), _slocLangsFp, _brakLangsFp));
            }

            Result = CocomoCalculator.CalculateComplexityPM(sloc, brak, scaleFactors, costDrivers).ToString();
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
