using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace UWP_Mastermind
{
    public class PegContainer: StackPanel
    {
        int ellipse_size = 50;

        public PegContainer(StackPanel parent, int numPegs)
        {
            this.Orientation = Orientation.Horizontal;
            this.Padding = new Thickness(2);
            Ellipse peg;
            for(int i = 0; i < numPegs; i++)
            {
                peg = new Ellipse();
                peg.Fill = new SolidColorBrush( Colors.DarkOrange);
                peg.Margin = new Thickness(2);
                peg.Height = ellipse_size;
                peg.Width= ellipse_size;
                peg.Name = parent.Name + "peg" + i;
                this.Children.Add(peg);

            }
        }
    }
}
