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
using Cover.Backend.BL;
using Cover.Backend.Entities;

namespace Cover.POS.Controls
{
    public partial class TimePickerUC : UserControl
    {
        #region "Properties"

        const string amText = "am";
        const string pmText = "pm";
 
        static SolidColorBrush brBlue = new SolidColorBrush(Colors.LightBlue);
        static SolidColorBrush brWhite = new SolidColorBrush(Colors.White);
 
        DateTime _lastKeyDown;

        #endregion

        #region "Constructor"

        public TimePickerUC()
        {
            InitializeComponent();
 
            _lastKeyDown = DateTime.Now;
        }

        #endregion

        #region "TimePicker_Events

        public TimeSpan Value
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

      
        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(TimeSpan), typeof(TimePickerUC),
        new UIPropertyMetadata(DateTime.Now.TimeOfDay, new PropertyChangedCallback(txt_ValueChanged)));
 
        private static void txt_ValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimePickerUC control = obj as TimePickerUC;
 
            TimeSpan newTime = ((TimeSpan)e.NewValue);
 
            int timehours = newTime.Hours;
            int hours = timehours % 12;
            hours = (hours > 0) ? hours : 12;
 
            control._hours = newTime.Hours;
            control.Hours = hours;
            control.Minutes = ((TimeSpan)e.NewValue).Minutes;
            control.DayHalf = ((timehours - 12) >= 0) ? pmText : amText;
 
        }
 
        private int _hours;
        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }
        public static readonly DependencyProperty HoursProperty =
        DependencyProperty.Register("Hours", typeof(int), typeof(TimePickerUC),
        new UIPropertyMetadata(0));
 
        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }
        public static readonly DependencyProperty MinutesProperty =
        DependencyProperty.Register("Minutes", typeof(int), typeof(TimePickerUC),
        new UIPropertyMetadata(0));
 
 
        public string DayHalf
        {
            get { return (string)GetValue(DayHalfProperty); }
            set { SetValue(DayHalfProperty, value); }
        }
        public static readonly DependencyProperty DayHalfProperty =
        DependencyProperty.Register("DayHalf", typeof(string), typeof(TimePickerUC),
        new UIPropertyMetadata(amText));
 
        private void Down(object sender, KeyEventArgs args)
        {
            bool updateValue = false;
 
            if (args.Key == Key.Up || args.Key == Key.Down)
            {
                switch (((Grid)sender).Name)
                {
                    case "min":
                        if (args.Key == Key.Up)
                            if (this.Minutes + 1 > 59)
                            {
                                this.Minutes = 0;
                                goto case "hour";
                            }
                            else
                            {
                                this.Minutes++;
                            }
                        if (args.Key == Key.Down)
                            if (this.Minutes - 1 < 0)
                            {
                                this.Minutes = 59;
                                goto case "hour";
                            }
                            else
                            {
                                this.Minutes--;
                            }
                        break;
 
                    case "hour":
                        if (args.Key == Key.Up)
                            this._hours = (_hours + 1 > 23) ? 0 : _hours + 1;
                        if (args.Key == Key.Down)
                            this._hours = (_hours - 1 < 0) ? 23 : _hours - 1;
                        break;
 
                    case "half":
                        this.DayHalf = (this.DayHalf == amText) ? pmText : amText;
 
                        int timeHours = this.Hours;
                        timeHours = (timeHours == 12) ? 0 : timeHours;
                        timeHours += (this.DayHalf == amText) ? 0 : 12;
 
                        _hours = timeHours;
                        break;
                }
 
                updateValue = true;
 
                args.Handled = true;
            }
            else if ((args.Key >= Key.D0 && args.Key <= Key.D9) || (args.Key >= Key.NumPad0 && args.Key <= Key.NumPad9))
            {
                int keyValue = (int)args.Key;
                int number = 0;
 
                number = keyValue - ((args.Key >= Key.D0 && args.Key <= Key.D9) ?
                                        (int)Key.D0 :
                                        (int)Key.NumPad0
                                    );
 
                bool attemptAdd = (DateTime.Now - _lastKeyDown).TotalSeconds < 1.5;
 
                switch (((Grid)sender).Name)
                {
                    case "min":
                        if (attemptAdd)
                        {
                            number += this.Minutes * 10;
 
                            if (number < 0 || number >= 60)
                            {
                                number -= this.Minutes * 10;
                            }
                        }
 
                        this.Minutes = number;
                        break;
 
                    case "hour":
                        if (attemptAdd)
                        {
                            number += this.Hours * 10;
 
                            if (number < 0 || number >= 13)
                            {
                                number -= this.Hours * 10;
                            }
                        }
 
                        number = (number == 12) ? 0 : number;
                        number += (this.DayHalf == amText) ? 0 : 12;
 
                        _hours = number;
                        break;
 
                    default:
                        break;
                }
 
                updateValue = true;
 
                args.Handled = true;
            }
            else if (args.Key == Key.A || args.Key == Key.P)
            {
                if (((Grid)sender).Name == "half")
                {
                    this.DayHalf = (args.Key == Key.A) ? amText : pmText;
 
                    updateValue = true; 
                }
            }
 
            if (updateValue)
            {
                this.Value = new TimeSpan(_hours, this.Minutes, 0);
            }
 
            _lastKeyDown = DateTime.Now;
        }
 
        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            var grd = sender as Grid;
 
            grd.Background = brBlue;
        }
 
        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            var grd = sender as Grid;
 
            grd.Background = brWhite;
        }
 
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var grd = sender as Grid;
 
            grd.Focus();
        }

        private void txt_Drop_1(object sender, DragEventArgs e)
        {

        }

      
       
    }
        #endregion

        #region "IValueConverter Members"

    public class MinuteSecondToStringConverter : IValueConverter
    {
        
 
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value is int)
                {
                    return ((int)value).ToString("00");
                }
            }
 
            return string.Empty;
        }
 
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value is string)
                {
                    int number;
                    if (int.TryParse(value as string, out number))
                    {
                        return number;
                    }
                }
            }
 
            return 0;
        }
 
        #endregion
    }
}
