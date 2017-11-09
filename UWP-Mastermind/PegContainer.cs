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
    public class PegContainer: Grid
    {
        int ellipse_size = 50;

        public PegContainer(StackPanel parent, int numPegs)
        {
            this.BorderBrush = new SolidColorBrush(Colors.Black);
            this.BorderThickness = new Thickness(1);
            //this.Orientation = Orientation.Horizontal;
            this.Padding = new Thickness(5);
            Ellipse peg;
            for(int i = 1; i <= numPegs; i++)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition());

                peg = new Ellipse();
                peg.Fill = new SolidColorBrush( Colors.DarkOrange);
                peg.Margin = new Thickness(2);
                peg.Height = ellipse_size;
                peg.Width= ellipse_size;
                peg.Name = parent.Name + "peg" + i;

                peg.SetValue(Grid.ColumnProperty, i - 1);
                this.Children.Add(peg);

            }
        }
    }
}
