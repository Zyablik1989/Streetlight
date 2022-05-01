using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Observer.Views
{
    /// <summary>
    /// Interaction logic for SegmentedDigit.xaml
    /// </summary>
    public partial class SegmentedDigit : UserControl
    {
        public SegmentedDigit()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty NumberForDigitProperty = 
            DependencyProperty.Register("NumberForDigit", typeof(byte?), typeof(SegmentedDigit), 
                new FrameworkPropertyMetadata((byte)0, new PropertyChangedCallback(OnNumberChanged)));
        public byte? NumberForDigit
        {
            get { return (byte)GetValue(NumberForDigitProperty); }
            set { SetValue(NumberForDigitProperty, value); 
                }
        }

        private static void OnNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RenderSegmentsBasedOnNumber((SegmentedDigit)d, (byte?)e.NewValue);
        }

        private static void RenderSegmentsBasedOnNumber(SegmentedDigit d, byte? number)
        {
            if (number == null)
            {
                number = 0;
            }
            var textScheme = Convert.ToString((byte)number, toBase: 2).PadLeft(8, '0').ToCharArray();

            d.Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                d.R0.Visibility = textScheme[0] == '1' ? Visibility.Visible : Visibility.Hidden;
                d.R1.Visibility = textScheme[1] == '1' ? Visibility.Visible : Visibility.Hidden;
                d.R2.Visibility = textScheme[2] == '1' ? Visibility.Visible : Visibility.Hidden;
                d.R3.Visibility = textScheme[3] == '1' ? Visibility.Visible : Visibility.Hidden;
                d.R4.Visibility = textScheme[4] == '1' ? Visibility.Visible : Visibility.Hidden;
                d.R5.Visibility = textScheme[5] == '1' ? Visibility.Visible : Visibility.Hidden;
                d.R6.Visibility = textScheme[6] == '1' ? Visibility.Visible : Visibility.Hidden;
            }));


        }

    }
}
