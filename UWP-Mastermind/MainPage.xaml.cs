using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_Mastermind
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static int numPegsPerTurn = 4;
        public static int current_turn = 1;
        public static int current_peg = 1;

        public static List<SolidColorBrush> _colorList = new List<SolidColorBrush>();

        public MainPage()
        {
            this.InitializeComponent();
            _colorList = CreateColourList();

            StartGame();
        }

        private void StartGame()
        {
            BuildTheBoard();
            AddNextMovemarker(current_turn, current_peg);


            SetSolution();
        }

        private void BuildTheBoard()
        {
            // add stackpanel to grid 1, 0 - colspan 2 - name it "spTurns"
            StackPanel spTurns = new StackPanel();
            spTurns.SetValue(Grid.ColumnProperty, 0);
            spTurns.SetValue(Grid.RowProperty, 1);
            spTurns.SetValue(Grid.ColumnSpanProperty, 1);

            PegContainer solution = new PegContainer(spTurns, 4);
            solution.SetValue(Grid.ColumnProperty, 0);
            solution.SetValue(Grid.RowProperty, 0);
            solution.HorizontalAlignment = HorizontalAlignment.Right;


            this.boardGrid.Children.Add(solution);

            for (int i = 10; i >= 1; i--)
            {
                spTurns.Children.Add(new TurnContainer(i));
            }

            this.boardGrid.Background = new SolidColorBrush(Colors.Blue);

            this.boardGrid.Children.Add(spTurns);

            // build and add the control panel element, passing the 
            // current turn and peg values
            ControlPanel cp = new ControlPanel(this, current_turn, current_peg);
            cp.Name = "colourPallette";
            cp.SetValue(Grid.ColumnProperty, 1);
            cp.SetValue(Grid.RowProperty, 1);
            this.boardGrid.Children.Add(cp);

            // for each turn (numTurns), add a turn container to spTurns - decrementing loop
            // add each turn container to the stackpanel
        }
        // move marker will be added to row 1 in the turn container
        // and column (peg_number)
        public void AddNextMovemarker(int numTurn, int numPeg)
        {
            // remove any existing move markers
            try
            {
                // if the ellipse can be found
                Ellipse oldMoveMarker = FindName("nextMoveMarker") as Ellipse;
                // remove it from its parent...
                ((PegContainer)oldMoveMarker.Parent).Children.Remove(oldMoveMarker);
            }
            catch
            {
                // not found, no problem?
            }
            // ...and add the new marker
            // find the correct turn container (PegContainer)
            PegContainer turnContainer = FindName("turn" + numTurn + "pegs") as PegContainer;


            // add a triangle object in row 1, column numPeg -1 of the turn container
            Ellipse nextMove = new Ellipse();
            nextMove.Name = "nextMoveMarker";
            nextMove.Height = 10;
            nextMove.Width = 10;
            nextMove.Fill = new SolidColorBrush(Colors.Black);

            nextMove.SetValue(Grid.RowProperty, 1);
            nextMove.SetValue(Grid.ColumnProperty, numPeg - 1);

            turnContainer.Children.Add(nextMove);

        }

        private void SetSolution()
        {

        }

        private List<SolidColorBrush> CreateColourList()
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
