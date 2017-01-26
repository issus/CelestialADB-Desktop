using System.Windows;
using System.Windows.Controls;

namespace Harris.CelestialADB.Desktop.Controls
{
    public class LinkButton : Button
    {
        static LinkButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkButton), new FrameworkPropertyMetadata(typeof(LinkButton)));
        }
    }
}