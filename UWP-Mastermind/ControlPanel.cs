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
        // reference to the MainPage, which is used for 
        // accessing the game methods
        MainPage _mainPage;

        public ControlPanel(MainPage mainPage, int current_turn, int current_peg)
        {
            this._mainPage = mainPage;

            // layout and colours
            this.Orientation = Orientation.Vertical;
            this.Padding = new Thickness(5);
            //this.Background = MainPage.SECONDARY_BG;
            //this.BorderBrush = new SolidColorBrush(Colors.Black);
            //this.BorderThickness = new Thickness(1);

            Ellipse el;
            foreach (SolidColorBrush c in MainPage._colorList)
            {
                el = new Ellipse();
                // TODO: check
                el.Name = "color" + (MainPage._colorList.IndexOf(c) + 1);
                el.Fill = c;
                el.Height = MainPage.PEG_LOCATION_SIZE;
                el.Width = MainPage.PEG_LOCATION_SIZE;
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
            Ellipse elCurrentTurnPegLocation = GetPegLocation(currentTurn, currentPeg);
            // set the peg colour to the tapped colour
            //elCurrentTurnPegLocation.Fill = tapped.Fill;
            elCurrentTurnPegLocation.Opacity = 100;

            // add a new ellipse into the peg location
            Ellipse elMove = new Ellipse();
            elMove.Height = MainPage.PEG_SIZE;
            elMove.Width = MainPage.PEG_SIZE;
            //elMove.SetValue(Canvas.ZIndexProperty, 100);
            elMove.Fill = tapped.Fill;
            elMove.SetValue(
                Grid.ColumnProperty,
                elCurrentTurnPegLocation.GetValue(Grid.ColumnProperty)
                );
            elMove.SetValue(
                Grid.RowProperty,
                elCurrentTurnPegLocation.GetValue(Grid.RowProperty)
                );
            PegContainer pegContainer = (PegContainer)elCurrentTurnPegLocation.Parent;
            pegContainer.Children.Add(elMove);
            // go to the next move
            this._mainPage.NextMove();
        }

        // get a peg with name turnXpegY
        private Ellipse GetPegLocation(int numTurn, int numPeg)
        {
            // try to find the peg
            string pegName = "turn" + numTurn + "pegLocation" + numPeg;
            Ellipse peg = FindName(pegName) as Ellipse;

            return peg;
        }


    }
}
