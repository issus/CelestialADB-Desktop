using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Harris.CelestialADB.Desktop.Controls
{
    public class ToggleLinkButton : ToggleButton
    {
        static ToggleLinkButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleLinkButton), new FrameworkPropertyMetadata(typeof(ToggleLinkButton)));
        }
    }
}
