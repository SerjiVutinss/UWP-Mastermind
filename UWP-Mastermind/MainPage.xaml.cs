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

            ColourPalette cp = new ColourPalette();
            cp.Name = "colourPallette";
            cp.SetValue(Grid.ColumnProperty, 1);
            cp.SetValue(Grid.RowProperty, 1);
            this.boardGrid.Children.Add(cp);

            // for each turn (numTurns), add a turn container to spTurns - decrementing loop
            // add each turn container to the stackpanel
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
