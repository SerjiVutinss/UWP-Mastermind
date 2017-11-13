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
    public class ColourPalette : Grid
    {
        // reference to the MainPage, which is used for 
        // accessing the game methods
        MainPage _mainPage;

        public ColourPalette(MainPage mainPage, int current_turn, int current_peg)
        {
            this._mainPage = mainPage;

            // layout and colours
            //this.Orientation = Orientation.Vertical;
            this.Padding = new Thickness(5);
            //this.Background = MainPage.SECONDARY_BG;
            //this.BorderBrush = new SolidColorBrush(Colors.Black);
            //this.BorderThickness = new Thickness(1);

            Ellipse el;
            foreach (SolidColorBrush c in MainPage._colorList)
            {
                this.RowDefinitions.Add(new RowDefinition());

                int i = MainPage._colorList.IndexOf(c);

                PegWrapper pegLocationWrapper = new PegWrapper(
                    "colorLoc" + (i + 1),
                    MainPage._colorList.IndexOf(c) + 1,
                    MainPage.BORDER_BG,
                    MainPage.PEG_LOCATION_SIZE
                    );
                pegLocationWrapper.Peg.SetValue(Grid.RowProperty, i);
                pegLocationWrapper.Peg.Margin = new Thickness(10);

                PegWrapper pegWrapper = new PegWrapper(
                    "color" + MainPage._colorList.IndexOf(c) + 1,
                    MainPage._colorList.IndexOf(c) + 1,
                    c,
                    MainPage.PEG_SIZE
                    );
                pegWrapper.Peg.SetValue(Grid.RowProperty, i);
                pegWrapper.Peg.Margin = new Thickness(10);

                pegWrapper.Peg.Tapped += El_Tapped;

                //el = new Ellipse();
                //// TODO: check
                //el.Name = "color" + (MainPage._colorList.IndexOf(c) + 1);
                //el.Fill = c;
                //el.Height = MainPage.PEG_LOCATION_SIZE;
                //el.Width = MainPage.PEG_LOCATION_SIZE;
                //el.Margin = new Thickness(10);
                //el.Tapped += El_Tapped; ;
                this.Children.Add(pegLocationWrapper.Peg);
                this.Children.Add(pegWrapper.Peg);
            }
        }

        // event handler which fires when a colour is tapped
        // because of the game logic, the majority of the time, 
        // this is a move, except for the last peg in the last turn, 
        // which is (game over?)
        private void El_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Ellipse tapped = (Ellipse)sender;
            // get the current game state values from the MainPage
            // retrieve the pegLocation which matches these values
            Ellipse elCurrentTurnPegLocation = GetPegLocation(
                MainPage.current_turn, MainPage.current_peg);
            // set the peg colour to the tapped colour
            //elCurrentTurnPegLocation.Fill = tapped.Fill;
            elCurrentTurnPegLocation.Opacity = 100;

            // !!!refactoring to use the PegWrapper class!!!
            // first find the parent element that the new 
            // peg should be added to 
            PegContainer pegContainer = (PegContainer)elCurrentTurnPegLocation.Parent;

            // the peg should be named turnXpegY, so add the prefix 
            PegWrapper pegWrapper = new PegWrapper(
                "turn" + MainPage.current_turn + "peg",
                MainPage.current_peg,
                tapped.Fill,
                MainPage.PEG_SIZE
                );
            pegContainer.Children.Add(pegWrapper.Peg);
            Debug.WriteLine(pegWrapper.Peg.Name);

            //// add a new ellipse into the peg location
            //Ellipse elMove = new Ellipse();
            //elMove.Height = 21;
            //elMove.Width = MainPage.PEG_SIZE;
            ////elMove.SetValue(Canvas.ZIndexProperty, 100);
            //elMove.Fill = tapped.Fill;
            //elMove.SetValue(
            //    Grid.ColumnProperty,
            //    elCurrentTurnPegLocation.GetValue(Grid.ColumnProperty)
            //    );
            //elMove.SetValue(
            //    Grid.RowProperty,
            //    elCurrentTurnPegLocation.GetValue(Grid.RowProperty)
            //    );
            //PegContainer pegContainer = (PegContainer)elCurrentTurnPegLocation.Parent;
            //pegContainer.Children.Add(elMove);
            // go to the next move
            this._mainPage.NextMove();
        }

        // get a peg with name turnXpegY
        private Ellipse GetPegLocation(int numTurn, int numPeg)
        {
            // try to find the peg
            string pegName = "turn" + numTurn + "loc" + numPeg;
            Ellipse peg = FindName(pegName) as Ellipse;

            return peg;
        }


    }
}
