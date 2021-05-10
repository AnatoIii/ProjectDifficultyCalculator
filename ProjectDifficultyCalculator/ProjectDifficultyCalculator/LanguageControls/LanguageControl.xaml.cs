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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectDifficultyCalculator.LanguageControls
{
    /// <summary>
    /// Interaction logic for LanguageControl.xaml
    /// </summary>
    public partial class LanguageControl : UserControl
    {
        public string LanguageName { get; set; }

        public LanguageControl(string language, RoutedEventHandler onSelect, RoutedEventHandler onDelete, Style buttonStyle = null)
        {
            LanguageName = language;
            InitializeComponent();

            BName.Content = language;
            BName.Style = buttonStyle;
            BName.Click += onSelect;
            BDelete.Click += onDelete;
        }
    }
}
