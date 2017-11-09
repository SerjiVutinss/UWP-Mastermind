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
        public MainPage()
        {
            this.InitializeComponent();

            BuildTheBoard();
        }

        private void BuildTheBoard()
        {
            // create a grid
            // add the solution to grid 0,1


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

            for(int i = 0; i < 10; i++)
            {
                spTurns.Children.Add(new TurnContainer(12));
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
    }
}
