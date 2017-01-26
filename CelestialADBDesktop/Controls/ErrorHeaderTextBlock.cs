using System.Windows;

namespace Harris.CelestialADB.Desktop.Controls
{
    public class ErrorHeaderTextBlock : TextBlock
    {
        static ErrorHeaderTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ErrorHeaderTextBlock), new FrameworkPropertyMetadata(typeof(ErrorHeaderTextBlock)));
        }
    }
}
