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
    // this class will control the majority of the game logic
    // its constructor takes the current_turn and current_peg as 
    // parameters, which are used to set the starting turn
    public class ControlPanel : StackPanel
    {
        MainPage _mainPage;

        public ControlPanel(MainPage mainPage, int current_turn, int current_peg)
        {
            this._mainPage = mainPage;

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
        }

        // event handler which fires when a colour is tapped
        // because of the game logic, the majority of the time, 
        // this is a move, except for the last peg in the last turn, 
        // which is (game over?)
        private void El_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Ellipse tapped;
            tapped = (Ellipse)sender;

            // get the current game state values from the MainPage
            int currentTurn = MainPage.current_turn;
            int currentPeg = MainPage.current_peg;
            // retrieve the peg which matches these values
            Ellipse elCurrentTurnPeg = GetPeg(currentTurn, currentPeg);
            // set the peg colour to the tapped colour
            elCurrentTurnPeg.Fill = tapped.Fill;
            elCurrentTurnPeg.Opacity = 100;

            // go to the next move
            NextMove();
        }

        // get a peg with name turnXpegY
        private Ellipse GetPeg(int numTurn, int numPeg)
        {
            // try to find the peg
            string pegName = "turn" + numTurn + "peg" + numPeg;
            Ellipse peg = FindName(pegName) as Ellipse;

            return peg;
        }

        // increment the move - i.e. peg# and/or turn# and 
        // add a move marker to it
        private void NextMove()
        {
            // check if it is the last peg in the turn
            if (MainPage.current_peg == MainPage.numPegsPerTurn)
            {
                // if so, reset the peg# to 1 
                MainPage.current_peg = 1;
                // and increment the turn#
                MainPage.current_turn += 1;
            }
            else
            {
                MainPage.current_peg += 1;
            }

            // add a move marker to the next move
            this._mainPage.AddNextMovemarker(MainPage.current_turn, MainPage.current_peg);
        }
    }
}
