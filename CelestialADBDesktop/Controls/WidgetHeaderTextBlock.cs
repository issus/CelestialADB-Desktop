using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Harris.CelestialADB.Desktop.Controls
{
    public class WidgetHeaderTextBlock : TextBlock
    {
        static WidgetHeaderTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WidgetHeaderTextBlock), new FrameworkPropertyMetadata(typeof(WidgetHeaderTextBlock)));
        }
    
    }
}
