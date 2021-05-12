using ProjectDifficultyCalculator.LanguageControls;
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
using System.Windows.Shapes;

namespace ProjectDifficultyCalculator.FunctionalPoints
{
    /// <summary>
    /// Interaction logic for FunctionalPointsWindow.xaml
    /// </summary>
    public partial class FunctionalPointsWindow : Window, INotifyPropertyChanged
    {
        private readonly TextBox[][] _textBoxes;
        public List<string> _availableLanguages;
        public IEnumerable<string> _allLanguages;
        private string _currentLanguage;
        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                LLanguage.Content = value;
                OnPropertyChanged("CurrentLanguage");
                _currentLanguage = value;
            }
        }

        public Dictionary<string, uint[][]> LanguagesComplexities { get; }
        private Dictionary<string, uint[][]> _maxComplexities;

        public FunctionalPointsWindow(IEnumerable<string> allLanguages, Dictionary<string, uint[][]> languagesComplexities, Dictionary<string, uint[][]> maxComplexities = null)
        {
            InitializeComponent();

            _allLanguages = allLanguages;
            _maxComplexities = maxComplexities;

            _textBoxes = new []
            {
                new[] { TBILFs_Low, TBILFs_Average, TBILFs_High },
                new[] { EIFs_Low, EIFs_Average, EIFs_High },
                new[] { Els_Low, Els_Average, Els_High },
                new[] { EOs_Low, EOs_Average, EOs_High },
                new[] { EQs_Low, EQs_Average, EQs_High }
            };

            if (languagesComplexities != null)
            {
                LanguagesComplexities = languagesComplexities;
                _availableLanguages = _allLanguages.Where(el => !languagesComplexities.ContainsKey(el)).ToList();

                foreach(var language in LanguagesComplexities.Keys)
                {
                    AddNewLanguage(language, true);
                }
                SelectLanguage(languagesComplexities.Keys.First());
            }
            else
            {
                LanguagesComplexities = new Dictionary<string, uint[][]>();
                _availableLanguages = new List<string>(_allLanguages);
            }

            CBlanguage.ItemsSource = _allLanguages;
        }

        private RoutedEventHandler SetLanguage(string language)
        {
            if (!LanguagesComplexities.ContainsKey(language))
            {
                var newArrs = _textBoxes.Select(a => new uint[a.Length]).ToArray();
                LanguagesComplexities.Add(language, newArrs);
            }

            void BSendLanguage_Click(object sender, RoutedEventArgs e)
            {
                SelectLanguage(((Button)sender).Content.ToString());
            }

            return BSendLanguage_Click;
        }

        private void SelectLanguage(string language)
        {
            CurrentLanguage = language ?? "";

            if (language == null)
            {
                foreach (var textBoxesArr in _textBoxes)
                {
                    foreach (var textBox in textBoxesArr)
                    {
                        textBox.Text = "0";
                    }
                }
                return;
            }

            var values = LanguagesComplexities[language];
            for (var i = 0; i < _textBoxes.Length; i++)
            {
                var textBoxesArr = _textBoxes[i];
                var valuesArr = values[i];
                for (var j = 0; j < textBoxesArr.Length; j++)
                {
                    textBoxesArr[j].Text = valuesArr[j].ToString();
                }
            }
        }

        private RoutedEventHandler DropLanguage(string language)
        {
            void BSendLanguage_Click(object sender, RoutedEventArgs e)
            {
                _availableLanguages.Add(language);

                var newLanguage = _allLanguages.FirstOrDefault(el => !_availableLanguages.Contains(el));

                if (string.IsNullOrWhiteSpace(newLanguage))
                {
                    MessageBox.Show("Add at least one more language to delete current one");
                    _availableLanguages.Remove(language);
                    return;
                }

                LanguagesComplexities.Remove(language);

                SelectLanguage(LanguagesComplexities.ContainsKey(newLanguage) ? newLanguage : null);

                UIElement childToDelete = SPLanguages.Children.Cast<LanguageControl>()
                    .FirstOrDefault(c => c.LanguageName == language);
                SPLanguages.Children.Remove(childToDelete);
            }

            return BSendLanguage_Click;
        }

        private void AddNewLanguage(string language, bool skipChecking = false)
        {
            if (!skipChecking && _maxComplexities != null && _maxComplexities.ContainsKey(language))
            {
                MessageBox.Show(this, "Languages are in readonly mode now.", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var style = FindResource("MenuStyles") as Style;

            var button = new LanguageControl(language, SetLanguage(language), DropLanguage(language), style);
            SPLanguages.Children.Add(button);
            SelectLanguage(language);
        }

        private void TextBox_Validation(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private static void GetIndexesOf<T>(T[][] arrs, T val, out int i, out int j)
        {
            for (i = 0; i < arrs.Length; i++)
            {
                var arr = arrs[i];
                for (j = 0; j < arr.Length; j++)
                {
                    if (arr[j].Equals(val)) return;
                }
            }

            i = -1;
            j = -1;
        }

        private void TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LanguagesComplexities.TryGetValue(_currentLanguage, out var value))
            {
                var textBox = (TextBox)sender;
                GetIndexesOf(_textBoxes, textBox, out var i, out var j);

                var max = _maxComplexities != null && _maxComplexities.TryGetValue(CurrentLanguage, out var maxes)
                    ? maxes[i][j] : uint.MaxValue;

                if (uint.TryParse(textBox.Text, out value[i][j]) && value[i][j] <= max) return;
                textBox.Text = max.ToString();
            }
            else
            {
                MessageBox.Show("Something wrong!");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Hidden { get; private set; }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hidden = true;
            Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (CBlanguage.SelectedIndex == -1)
            {
                MessageBox.Show("Select language first!");
                return;
            }

            var language = CBlanguage.SelectedItem.ToString();
            if (_availableLanguages.Contains(language))
            {
                AddNewLanguage(language);
                _availableLanguages.Remove(language);
            }
        }
    }
}
