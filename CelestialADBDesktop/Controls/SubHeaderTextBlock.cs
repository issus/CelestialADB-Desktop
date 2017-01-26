using System.Windows;

namespace Harris.CelestialADB.Desktop.Controls
{
    public class SubHeaderTextBlock : TextBlock
    {
        static SubHeaderTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SubHeaderTextBlock), new FrameworkPropertyMetadata(typeof(SubHeaderTextBlock)));
        }
    }
}
