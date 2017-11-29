using System.Windows;
using Harris.CelestialADB.Desktop.ViewModel;
using System;
using System.Linq;

namespace Harris.CelestialADB.Desktop
{
    enum UIScale
    {
        Small,
        Medium,
        Regular
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            scale = UIScale.Regular;
        }


        private UIScale scale;

        #region ScaleValue Dependency Property
        public static readonly DependencyProperty ScaleValueProperty =
            DependencyProperty.Register("ScaleValue", typeof(double), typeof(MainWindow),
                new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), new CoerceValueCallback(OnCoerceScaleValue)));

        private static object OnCoerceScaleValue(DependencyObject o, object value)
        {
            MainWindow mainWindow = o as MainWindow;
            if (mainWindow != null)
                return mainWindow.OnCoerceScaleValue((double)value);
            else
                return value;
        }

        private static void OnScaleValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MainWindow mainWindow = o as MainWindow;
            if (mainWindow != null)
                mainWindow.OnScaleValueChanged((double)e.OldValue, (double)e.NewValue);
        }

        protected virtual double OnCoerceScaleValue(double value)
        {
            if (double.IsNaN(value))
                return 1.0f;

            value = Math.Max(0.1, value);
            return value;
        }

        protected virtual void OnScaleValueChanged(double oldValue, double newValue)
        {

        }

        public double ScaleValue
        {
            get
            {
                return (double)GetValue(ScaleValueProperty);
            }
            set
            {
                SetValue(ScaleValueProperty, value);
            }
        }
        #endregion

        private void MainGrid_SizeChanged(object sender, EventArgs e)
        {
            CalculateScale();
        }

        private void CalculateScale()
        {
            double yScale = ActualHeight / 900f;
            double xScale = ActualWidth / 1200f;
            double value = Math.Min(xScale, yScale);

            if (value > 1)
                value = 1;

            ScaleValue = Math.Round((double)OnCoerceScaleValue(dbMainWindow, value), 2);

            if (ScaleValue < 0.82 && scale != UIScale.Small)
            {
                scale = UIScale.Small;

                var dictionary = Application.Current.Resources.MergedDictionaries
                    .Where(d => d.Source.OriginalString == "Styles/ThemeFonts.Medium.xaml" || d.Source.OriginalString == "Styles/ThemeFonts.Regular.xaml")
                    .FirstOrDefault();

                if (dictionary != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(dictionary);

                    Application.Current.Resources.MergedDictionaries.Add(
                        new ResourceDictionary
                        {
                            Source = new Uri("Styles/ThemeFonts.Small.xaml", UriKind.Relative)
                        }
                        );
                }
            }
            else if (ScaleValue >= 0.82 && ScaleValue < 0.9 && scale != UIScale.Medium)
            {
                scale = UIScale.Medium;

                var dictionary = Application.Current.Resources.MergedDictionaries
                    .Where(d => d.Source.OriginalString == "Styles/ThemeFonts.Small.xaml" || d.Source.OriginalString == "Styles/ThemeFonts.Regular.xaml")
                    .FirstOrDefault();

                if (dictionary != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(dictionary);

                    Application.Current.Resources.MergedDictionaries.Add(
                        new ResourceDictionary
                        {
                            Source = new Uri("Styles/ThemeFonts.Medium.xaml", UriKind.Relative)
                        }
                        );
                }
            }

            else if (ScaleValue >= 0.9 && scale != UIScale.Regular)
            {
                scale = UIScale.Regular;

                var dictionary = Application.Current.Resources.MergedDictionaries
                        .Where(d => d.Source.OriginalString == "Styles/ThemeFonts.Small.xaml" || d.Source.OriginalString == "Styles/ThemeFonts.Medium.xaml")
                        .FirstOrDefault();

                if (dictionary != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(dictionary);

                    Application.Current.Resources.MergedDictionaries.Add(
                        new ResourceDictionary
                        {
                            Source = new Uri("Styles/ThemeFonts.Regular.xaml", UriKind.Relative)
                        }
                        );
                }
            }
        }
    }
}