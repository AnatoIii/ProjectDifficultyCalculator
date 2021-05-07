using ProjectDifficultyCalculator.CustomControls;
using ProjectDifficultyCalculator.Logic.COCOMO;
using System;
using System.Collections.Generic;
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

namespace ProjectDifficultyCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<SelectorControl> CostDriverControls { get; set; }
        private List<SelectorControl> ScaleFactorsControls { get; set; }
        private CocomoCalculator CocomoCalculator { get; set; }
        private CocomoProperties CocomoProperties => CocomoCalculator.Properties;

        public MainWindow()
        {
            CostDriverControls = new List<SelectorControl>();
            ScaleFactorsControls = new List<SelectorControl>();
            InitializeComponent();
            DataContext = this;
            var properties = CocomoDefaultPropertiesFactory.Create();
            CocomoCalculator = new CocomoCalculator(properties);

            for (var i = 0; i < properties.ScaleFactors.Length; i++)
            {
                var costDriver = properties.ScaleFactors[i];
                var selectorControl = new SelectorControl()
                {
                    Id = costDriver.ShortName,
                    ControlTooltip = costDriver.FullName,
                    Title = costDriver.ShortName
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
                //_ = i % 3 switch
                //{
                //    0 => panel1.Children.Add(selectorControl),
                //    1 => panel2.Children.Add(selectorControl),
                //    _ => panel3.Children.Add(selectorControl)
                //};

                ScaleFactorsControls.Add(selectorControl);
            }

            for (var i = 0; i < properties.CostDrivers.Length; i++)
            {
                var costDriver = properties.CostDrivers[i];
                var selectorControl = new SelectorControl()
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
                //_ = i % 3 switch
                //{
                //    0 => panel1.Children.Add(selectorControl),
                //    1 => panel2.Children.Add(selectorControl),
                //    _ => panel3.Children.Add(selectorControl)
                //};

                CostDriverControls.Add(selectorControl);
            }

            //var selectorControl = new SelectorControl()
            //{
            //    ControlTooltip = "Test value",
            //    Title = "Test title"
            //};
            //this.panel2.Children.Add(selectorControl);
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
    }
}
