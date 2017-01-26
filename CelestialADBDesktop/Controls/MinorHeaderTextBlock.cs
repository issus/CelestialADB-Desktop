using System.Windows;

namespace Harris.CelestialADB.Desktop.Controls
{
    public class MinorHeaderTextBlock : TextBlock
    {
        static MinorHeaderTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MinorHeaderTextBlock), new FrameworkPropertyMetadata(typeof(MinorHeaderTextBlock)));
        }
    }
}
