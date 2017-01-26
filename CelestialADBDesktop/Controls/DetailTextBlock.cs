using System.Windows;
using System.Windows.Controls;

namespace Harris.CelestialADB.Desktop.Controls
{
    public class DetailTextBlock : TextBlock
    {
        static DetailTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DetailTextBlock), new FrameworkPropertyMetadata(typeof(DetailTextBlock)));
        }
    }
}
