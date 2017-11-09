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
    public class ColourPalette : StackPanel
    {
        List<SolidColorBrush> colorList;

        public ColourPalette()
        {
            this.colorList = createColourList();
            this.Orientation = Orientation.Vertical;

            this.BorderBrush = new SolidColorBrush(Colors.Black);
            this.BorderThickness = new Thickness(1);

            this.Background = new SolidColorBrush(Colors.DarkBlue);

            this.Padding = new Thickness(5);


            Ellipse el;
            int ellipse_size = 50;
            foreach (SolidColorBrush c in colorList)
            {
                el = new Ellipse();
                // TODO: check
                el.Name = "color" + (colorList.IndexOf(c) + 1);
                el.Fill = c;
                el.Height = ellipse_size;
                el.Width = ellipse_size;
                el.Margin = new Thickness(10);
                el.Tapped += El_Tapped; ;
                this.Children.Add(el);
            }

            Rectangle rect = new Rectangle();
            rect.Fill = new SolidColorBrush(Colors.Black);
            rect.Height = ellipse_size;
            rect.Width = ellipse_size;
            rect.Name = "rect";
            this.Children.Add(rect);
        }

        private void El_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Ellipse tapped;
            tapped = (Ellipse)sender;


            Rectangle rect = FindName("rect") as Rectangle;
            rect.Fill = tapped.Fill;
        }

        private List<SolidColorBrush> createColourList()
        {
            List<SolidColorBrush> colorList = new List<SolidColorBrush>();

            SolidColorBrush c1 = new SolidColorBrush(Colors.White);
            SolidColorBrush c2 = new SolidColorBrush(Colors.Green);
            SolidColorBrush c3 = new SolidColorBrush(Colors.Red);
            SolidColorBrush c4 = new SolidColorBrush(Colors.Yellow);
            SolidColorBrush c5 = new SolidColorBrush(Colors.Purple);
            SolidColorBrush c6 = new SolidColorBrush(Colors.Gold);
            SolidColorBrush c7 = new SolidColorBrush(Colors.Silver);

            colorList.Add(c1);
            colorList.Add(c2);
            colorList.Add(c3);
            colorList.Add(c4);
            colorList.Add(c5);
            colorList.Add(c6);
            colorList.Add(c7);

            return colorList;
        }
    }
}
