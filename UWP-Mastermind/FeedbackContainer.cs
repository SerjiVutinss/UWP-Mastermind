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
    public class FeedbackContainer : Grid
    {

        // 4 turns
        // for each turn, 
        public FeedbackContainer(StackPanel parent)
        {
            int ellipse_size = 25;

            this.BorderBrush = new SolidColorBrush(Colors.Black);
            this.BorderThickness = new Thickness(1);

            // grid defs
            this.ColumnDefinitions.Add(new ColumnDefinition());
            this.ColumnDefinitions.Add(new ColumnDefinition());
            this.RowDefinitions.Add(new RowDefinition());
            this.RowDefinitions.Add(new RowDefinition());
            this.Padding = new Thickness(5);

            Ellipse el;
            el = new Ellipse();
            el.SetValue(Grid.ColumnProperty, 0);
            el.SetValue(Grid.RowProperty, 0);
            el.Height = ellipse_size;
            el.Width = ellipse_size;
            el.Fill = new SolidColorBrush(Colors.Pink);
            el.Name = parent.Name + "fb1";
            this.Children.Add(el);

            el = new Ellipse();
            el.SetValue(Grid.ColumnProperty, 1);
            el.SetValue(Grid.RowProperty, 0);
            el.Height = ellipse_size;
            el.Width = ellipse_size;
            el.Fill = new SolidColorBrush(Colors.Pink);
            el.Name = parent.Name + "fb2";
            this.Children.Add(el);

            el = new Ellipse();
            el.SetValue(Grid.ColumnProperty, 0);
            el.SetValue(Grid.RowProperty, 1);
            el.Height = ellipse_size;
            el.Width = ellipse_size;
            el.Fill = new SolidColorBrush(Colors.Pink);
            el.Name = parent.Name + "fb3";
            this.Children.Add(el);

            el = new Ellipse();
            el.SetValue(Grid.ColumnProperty, 1);
            el.SetValue(Grid.RowProperty, 1);
            el.Height = ellipse_size;
            el.Width = ellipse_size;
            el.Fill = new SolidColorBrush(Colors.Pink);
            el.Name = parent.Name + "fb4";
            this.Children.Add(el);
        }
    }
}
