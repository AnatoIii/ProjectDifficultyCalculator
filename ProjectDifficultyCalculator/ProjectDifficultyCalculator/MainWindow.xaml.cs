using ProjectDifficultyCalculator.CustomControls;
using ProjectDifficultyCalculator.Logic.COCOMO;
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

namespace ProjectDifficultyCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<SelectorControl> costDriverControls { get; set; }
        private List<SelectorControl> scaleFactorsControls { get; set; }
        private int _linesAmount = 0;

        public MainWindow()
        {
            costDriverControls = new List<SelectorControl>();
            scaleFactorsControls = new List<SelectorControl>();
            InitializeComponent();
            DataContext = this;
            var values = CocomoDefaultPropertiesFactory.Create();

            int j = 0;
            foreach (var costDriver in values.ScaleFactors)
            {
                var selectorControl = new SelectorControl()
                {
                    Id = costDriver.ShortName,
                    ControlTooltip = costDriver.FullName,
                    Title = costDriver.ShortName
                };

                if (j % 3 == 0)
                {
                    scaleFactorsPanel1.Children.Add(selectorControl);
                }
                else if (j % 3 == 1)
                {
                    scaleFactorsPanel2.Children.Add(selectorControl);
                }
                else if (j % 3 == 2)
                {
                    scaleFactorsPanel3.Children.Add(selectorControl);
                }
                //_ = i % 3 switch
                //{
                //    0 => panel1.Children.Add(selectorControl),
                //    1 => panel2.Children.Add(selectorControl),
                //    _ => panel3.Children.Add(selectorControl)
                //};

                scaleFactorsControls.Add(selectorControl);
                j++;
            }

            int i = 0;
            foreach (var costDriver in values.CostDrivers)
            {
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

                costDriverControls.Add(selectorControl);
                i++;
            }
            //var selectorControl = new SelectorControl()
            //{
            //    ControlTooltip = "Test value",
            //    Title = "Test title"
            //};
            //this.panel2.Children.Add(selectorControl);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Calculate result here
            var results = costDriverControls.Select(el => (el.Id, el.GetValue())).ToArray();
        }

        private void ButtonFP_Click(object sender, RoutedEventArgs e)
        {
            // In reality something should happen here, but I'm not sure what
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int value))
            {
                _linesAmount = value;
            }
            else
            {
                textBox1.Text = _linesAmount.ToString();
            }
        }
    }
}
