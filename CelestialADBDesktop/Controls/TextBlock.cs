using System.Windows;
using System.Windows.Controls;

namespace Harris.CelestialADB.Desktop.Controls
{
    public class TextBlock : System.Windows.Controls.TextBlock
    {
        static TextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(typeof(TextBlock)));
        }
    }
}
