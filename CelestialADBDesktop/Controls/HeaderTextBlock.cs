using System.Windows;
using System.Windows.Controls;

namespace Harris.CelestialADB.Desktop.Controls
{
    public class HeaderTextBlock : TextBlock
    {
        static HeaderTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderTextBlock), new FrameworkPropertyMetadata(typeof(HeaderTextBlock)));
        }
    }
}