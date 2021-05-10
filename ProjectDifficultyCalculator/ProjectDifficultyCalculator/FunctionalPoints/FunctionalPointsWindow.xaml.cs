using ProjectDifficultyCalculator.LanguageControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
        private List<TextBox> _textBoxes;
        public List<string> _availableLanguages;
        public List<string> _allLanguages;
        private string _currentLanguage;
        public string CurrentLanguage
        {
            get { return _currentLanguage; }
            set
            {
                LLanguage.Content = value;
                OnPropertyChanged("CurrentLanguage");
                _currentLanguage = value;
            }
        }

        private Dictionary<string, double[]> _languagesComplexity;

        public Dictionary<string, double[]> GetLanguages()
        {
            return _languagesComplexity;
        }

        public FunctionalPointsWindow(Dictionary<string, double[]> languagesComplexity = null)
        {
            InitializeComponent();

            _allLanguages = new List<string> { "C#", "C++", "Java", "SQL", "Perl", "HTML" };
            _availableLanguages = new List<string> { "C#", "C++", "Java", "SQL", "Perl", "HTML" };

            _languagesComplexity = new Dictionary<string, double[]>();

            _textBoxes = new List<TextBox>()
            {
                TBILFs_Low, TBILFs_Average, TBILFs_High,
                EIFs_Low, EIFs_Average, EIFs_High,
                Els_Low, Els_Average, Els_High,
                EOs_Low, EOs_Average, EOs_High,
                EQs_Low, EQs_Average, EQs_High
            };

            CBlanguage.ItemsSource = _availableLanguages;

            if (languagesComplexity != null)
            {
                _languagesComplexity = languagesComplexity;
                _availableLanguages = _allLanguages.Where(el => !languagesComplexity.ContainsKey(el)).ToList();

                foreach(var language in _languagesComplexity.Keys)
                {
                    AddNewLanguage(language);
                }
                SelectLanguage(languagesComplexity.Keys.First());
            }
        }

        private RoutedEventHandler SetLanguage(string language)
        {
            if (!_languagesComplexity.ContainsKey(language))
            {
                _languagesComplexity.Add(language, new double[15]);
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
                for (int i = 0; i < 15; i++)
                {
                    _textBoxes[i].Text = "0";
                }
                return;
            }

            var values = _languagesComplexity[language];
            for (int i = 0; i < 15; i++)
            {
                _textBoxes[i].Text = values[i].ToString();
            }
        }

        private RoutedEventHandler DropLanguage(string language)
        {
            void BSendLanguage_Click(object sender, RoutedEventArgs e)
            {
                _availableLanguages.Add(language);

                var newLanguage = _allLanguages.Where(el => !_availableLanguages.Contains(el)).FirstOrDefault() ?? "";

                if (newLanguage == "")
                {
                    MessageBox.Show("Add at least one more language to delete current one");
                    _availableLanguages.Remove(language);
                    return;
                }

                _languagesComplexity.Remove(language);

                if (_languagesComplexity.ContainsKey(newLanguage))
                {
                    SelectLanguage(newLanguage);
                }
                else
                {
                    SelectLanguage(null);
                }

                UIElement childToDelete = null;
                foreach (var child in SPLanguages.Children)
                {
                    if (((LanguageControl)child).LanguageName == language)
                    {
                        childToDelete = (UIElement)child;
                        continue;
                    }
                }
                SPLanguages.Children.Remove(childToDelete);
            }

            return BSendLanguage_Click;
        }


        private void AddNewLanguage(string language)
        {
            var style = FindResource("MenuStyles") as Style;

            var button = new LanguageControl(language, SetLanguage(language), DropLanguage(language), style);
            SPLanguages.Children.Add(button);
            SelectLanguage(language);
        }

        private void TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_languagesComplexity.TryGetValue(_currentLanguage, out double[] value))
            {
                var senderTB = (TextBox)sender;
                var index = _textBoxes.Select((el, index) => (el, index)).First(el => el.el.Equals(senderTB)).index;
                value[index] = double.Parse(senderTB.Text); // We will get errors here
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
            this.Hide();
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
