using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public ColourPalette()
        {

            // layout and colours
            this.Orientation = Orientation.Vertical;
            this.Padding = new Thickness(5);
            this.Background = new SolidColorBrush(Colors.DarkBlue);
            this.BorderBrush = new SolidColorBrush(Colors.Black);
            this.BorderThickness = new Thickness(1);

            Ellipse el;
            int ellipse_size = 50;
            foreach (SolidColorBrush c in MainPage._colorList)
            {
                el = new Ellipse();
                // TODO: check
                el.Name = "color" + (MainPage._colorList.IndexOf(c) + 1);
                el.Fill = c;
                el.Height = ellipse_size;
                el.Width = ellipse_size;
                el.Margin = new Thickness(10);
                el.Tapped += El_Tapped; ;
                this.Children.Add(el);
            }

            //Rectangle rect = new Rectangle();
            //rect.Fill = new SolidColorBrush(Colors.Black);
            //rect.Height = ellipse_size;
            //rect.Width = ellipse_size;
            //rect.Name = "rect";
            //this.Children.Add(rect);
        }

        private void El_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Ellipse tapped;
            tapped = (Ellipse)sender;

            // // Diaply the colour in the rect
            //Rectangle rect = FindName("rect") as Rectangle;
            //rect.Fill = tapped.Fill;

            Ellipse elCurrentTurnPeg = GetNextPeg();

            // set the peg colour to the tapped colour
            elCurrentTurnPeg.Fill = tapped.Fill;
            elCurrentTurnPeg.Opacity = 100;

            // go to the next move
            NextMove();
            elCurrentTurnPeg = null;
            // highlight the next peg
            elCurrentTurnPeg = GetNextPeg();
            Grid grid = (Grid)elCurrentTurnPeg.Parent;

            Ellipse nextMove = new Ellipse();
            nextMove.Height = 10;
            nextMove.Width = 10;
            nextMove.Fill = new SolidColorBrush(Colors.Black);

            grid.Children.Add(nextMove);
        }

        private Ellipse GetNextPeg()
        {
            // get turn values
            int currentTurn = MainPage.current_turn;
            int currentPeg = MainPage.current_peg;

            // try to find the peg
            string pegName = "turn" + currentTurn + "peg" + currentPeg;
            Ellipse peg = FindName(pegName) as Ellipse;

            return peg;
        }

        private void NextMove()
        {
            // check if it is the last peg
            if (MainPage.current_peg == MainPage.numPegsPerTurn)
            {
                MainPage.current_peg = 1;
                MainPage.current_turn += 1;
            }
            else
            {
                MainPage.current_peg += 1;
            }
        }
    }
}
