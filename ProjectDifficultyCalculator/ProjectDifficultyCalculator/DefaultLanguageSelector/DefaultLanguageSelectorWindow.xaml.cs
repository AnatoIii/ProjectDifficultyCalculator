using System;
using System.Collections.Generic;
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

namespace ProjectDifficultyCalculator.DefaultLanguageSelector
{
    /// <summary>
    /// Interaction logic for DefaultLanguageSelectorWindow.xaml
    /// </summary>
    public partial class DefaultLanguageSelectorWindow : Window
    {
        public string SelectedLanguage { get; set; }

        public DefaultLanguageSelectorWindow()
        {
            InitializeComponent();

            CBLanguages.ItemsSource = new List<string> { "C#", "C++", "Java", "SQL", "Perl", "HTML" };
            CBLanguages.SelectedIndex = 0;
        }

        private void BAccept_Click(object sender, RoutedEventArgs e)
        {
            SelectedLanguage = CBLanguages.SelectedItem.ToString();
            Hide();
        }
    }
}
