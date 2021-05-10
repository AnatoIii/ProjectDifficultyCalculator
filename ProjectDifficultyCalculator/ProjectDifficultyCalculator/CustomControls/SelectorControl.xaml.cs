using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectDifficultyCalculator.CustomControls
{
    /// <summary>
    /// Interaction logic for SelectorControl.xaml
    /// </summary>
    public partial class SelectorControl : UserControl, INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ControlTooltip { get; set; }
        private int _value { get; set; }

        public double[] Values { get; set; }

        private int _sliderValue = 2;
        public int SliderValue
        {
            get { return _sliderValue; }
            set
            {
                if (double.IsNaN(Values[value]))
                {
                    _sliderValue = GetClosestNotNaN(value);
                }
                else if (value != _sliderValue)
                {
                    _sliderValue = value;
                    OnPropertyChanged("SliderValue");
                }
            }
        }

        public SelectorControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //var newValue = (int)e.NewValue;
            //if (double.IsNaN(Values[newValue]))
            //{
            //    _sliderValue = 2;
            //}
            //else
            //{
            //    _sliderValue = (int)e.NewValue;
            //}

            //OnPropertyChanged("_sliderValue");
        }

        public int GetValue()
        {
            return _value;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Values == null || Values.Count() == 0)
            {
                return;
            }

            if (double.IsNaN(Values[0]))
            {
                LVL.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#dddddd"));
            }
            if (double.IsNaN(Values[1]))
            {
                LL.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#dddddd"));
            }
            if (double.IsNaN(Values[2]))
            {
                LN.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#dddddd"));
            }
            if (double.IsNaN(Values[3]))
            {
                LH.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#dddddd"));
            }
            if (double.IsNaN(Values[4]))
            {
                LVH.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#dddddd"));
            }
            if (double.IsNaN(Values[5]))
            {
                LEH.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#dddddd"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int GetClosestNotNaN(int value)
        {
            if (value < 2)
            {
                if (!double.IsNaN(Values[value + 1]))
                {
                    return value + 1;
                } 
                else
                {
                    return GetClosestNotNaN(value + 1);
                }
            }

            if (value > 2)
            {
                if (!double.IsNaN(Values[value - 1]))
                {
                    return value - 1;
                }
                else
                {
                    return GetClosestNotNaN(value - 1);
                }
            }

            return 2;
        }
    }
}
