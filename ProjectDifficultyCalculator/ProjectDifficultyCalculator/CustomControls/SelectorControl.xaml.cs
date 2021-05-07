using System.Windows;
using System.Windows.Controls;

namespace ProjectDifficultyCalculator.CustomControls
{
    /// <summary>
    /// Interaction logic for SelectorControl.xaml
    /// </summary>
    public partial class SelectorControl : UserControl
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ControlTooltip { get; set; }
        private int _value { get; set; }

        public SelectorControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _value = (int)e.NewValue;
        }

        public int GetValue()
        {
            return _value;
        }
    }
}
