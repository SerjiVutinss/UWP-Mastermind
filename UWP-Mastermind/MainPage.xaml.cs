﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.UI.Xaml.Media.Imaging;
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
        #region Globals and Constants

        // set the game state to the base setting
        public static int current_turn = 1;
        public static int current_peg = 1;

        // the list of colours used in the control panel
        public static List<SolidColorBrush> _colorList = new List<SolidColorBrush>();

        /// <summary>
        /// //
        //
        /// </summary>
        /// 
        // PegContainer display and layout values
        //static SolidColorBrush PEG_CONTAINER_COLOUR = new SolidColorBrush(Colors.Black);
        public static double PEG_MARGIN_SIZE = PEG_SIZE / 25;
        public static double PEG_CONTAINER_BORDER = PEG_SIZE / 50;
        public static double PEG_CONTAINER_PADDING = PEG_SIZE / 10;


        // CONSTANTS

        // Number of pegs per turn, per feedback container and 
        // in the solution container.
        // TODO: This may be variable at a later stage, feedback container
        // is toughest to implement since it is a non-linear grid
        public static readonly int NUM_PEGS = 4;
        // the number of turns per game
        // TODO: again, this may be variable later
        public static readonly int NUM_TURNS = 10;

        // PEGS - layout and display values
        public static readonly SolidColorBrush PEG_CONTAINER_COLOUR = new SolidColorBrush(Colors.Black);
        // - the size of each peg in the game
        public static readonly double PEG_SIZE = 50;
        // the colour of each peg location before a move has been made
        public static readonly SolidColorBrush PEG_COLOUR = new SolidColorBrush(Colors.DarkOrange);

        // ImageBrushes assigned in SetImageBrushes()
        // main background image brush
        public static ImageBrush MAIN_BG;
        // secondary background image brush (solution or turn container)
        public static ImageBrush SECONDARY_BG; public static ImageBrush BORDER_BG;



        // END CONSTANTS
        #endregion Globals and Constants


        public MainPage()
        {
            this.InitializeComponent();
            // create the colour list
            _colorList = CreateColourList();
            // assign the background image brushes
            SetImageBrushes();

            StartGame();
        }

        // bunch of methods to start the game
        private void StartGame()
        {
            // build the board and pegs
            BuildTheBoard();
            // add the marker to show the next peg which will be populated
            AddNextMovemarker(current_turn, current_peg);

            // generate a random solution
            SetSolution();
        }

        private void BuildTheBoard()
        {
            // set some colours on the boardGrid
            Border brdr = new Border();
            //brdr.Background = new SolidColorBrush(Colors.RosyBrown);
            brdr.SetValue(Grid.ColumnProperty, 1);
            brdr.SetValue(Grid.RowProperty, 0);
            brdr.SetValue(Grid.RowSpanProperty, 2);

            this.boardGrid.Background = MAIN_BG;

            // add stackpanel to grid 1, 0 - colspan 2 - name it "spTurns"
            StackPanel spTurns = new StackPanel();
            spTurns.SetValue(Grid.ColumnProperty, 0);
            spTurns.SetValue(Grid.RowProperty, 1);
            spTurns.SetValue(Grid.ColumnSpanProperty, 1);

            // create the colution peg container
            // TODO: don't add the solution yet as it may need to be loaded from AppData
            // so create the solution in method SetSolution()
            PegContainer solution = new PegContainer(spTurns, 4);
            solution.Name = "solution";
            solution.Background = SECONDARY_BG;
            solution.SetValue(Grid.ColumnProperty, 0);
            solution.SetValue(Grid.RowProperty, 0);
            solution.HorizontalAlignment = HorizontalAlignment.Right;

            solution.Padding = new Thickness(
                75,
                MainPage.PEG_CONTAINER_PADDING,
                MainPage.PEG_CONTAINER_PADDING,
                MainPage.PEG_CONTAINER_PADDING);

            solution.BorderBrush = BORDER_BG;
            solution.BorderThickness = new Thickness(2);


            this.boardGrid.Children.Add(solution);

            // turn1 is at the bottom of the stack panel, so 
            // reverse the loop
            for (int i = NUM_TURNS; i >= 1; i--)
            {
                // turns begin at 10, end at 1
                spTurns.Children.Add(new TurnContainer(i));
            }

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

        // generate a new solution and add it to the PegContainer solution object
        private void SetSolution()
        {
            // 
            // for each number of pegs per turn,
            // add a random colour to the peg container named "solution"
            // name each peg solutionPeg + i

            // better to create a random object here and call .Next() multiple times
            Random rand = new Random();
            Ellipse solutionPeg;
            for (int i = 1; i <= NUM_PEGS; i++)
            {
                solutionPeg = new Ellipse();
                solutionPeg.Name = "solutionPeg" + i;
                // generate a random number between 0 and the length of 
                // _colourList, uppperbound is not included in random
                int _randColourIndex = rand.Next(0, _colorList.Count);
                solutionPeg.Fill = _colorList.ElementAt(_randColourIndex);
                // TODO: should set constants here, for all pegs and containers created
                solutionPeg.Height = 50;
                solutionPeg.Width = 50;
                PegContainer solutionContainer = FindName("solution") as PegContainer;
                // set the grid location to (i - 1) since grid is zero-based
                solutionPeg.SetValue(Grid.ColumnProperty, i - 1);
                // add it to the PegContainer
                solutionContainer.Children.Add(solutionPeg);
            }
        }

        // increment the move - i.e. peg# and/or turn# and 
        // add a move marker to it
        public void NextMove()
        {
            // check if it is the last peg in the turn
            if (MainPage.current_peg == MainPage.NUM_PEGS)
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
            this.AddNextMovemarker(MainPage.current_turn, MainPage.current_peg);
        }

        // at the end of a turn, check the turn pegs against the solution pegs
        public void CalculateTurnEnd()
        {
            // TODO: implement
        }

        // create a static list of colours
        private List<SolidColorBrush> CreateColourList()
        {
            List<SolidColorBrush> colorList = new List<SolidColorBrush>();

            SolidColorBrush c1 = new SolidColorBrush(Colors.DarkBlue);
            SolidColorBrush c2 = new SolidColorBrush(Colors.Green);
            SolidColorBrush c3 = new SolidColorBrush(Colors.Red);
            SolidColorBrush c4 = new SolidColorBrush(Colors.Yellow);
            SolidColorBrush c5 = new SolidColorBrush(Colors.Purple);
            SolidColorBrush c6 = new SolidColorBrush(Colors.HotPink);
            SolidColorBrush c7 = new SolidColorBrush(Colors.OrangeRed);

            colorList.Add(c1);
            colorList.Add(c2);
            colorList.Add(c3);
            colorList.Add(c4);
            colorList.Add(c5);
            colorList.Add(c6);
            colorList.Add(c7);

            return colorList;
        }

        private void SetImageBrushes()
        {
            // vertical wood grain
            MAIN_BG = new ImageBrush
            {
                ImageSource = new BitmapImage(
                    new Uri(this.BaseUri, @"Assets\Images\wood_bg_1_rot90.jpg")
                    ),
                Stretch = Stretch.UniformToFill
            };
            // horizontal wood grain
            SECONDARY_BG = new ImageBrush
            {
                ImageSource = new BitmapImage(
                    new Uri(this.BaseUri, @"Assets\Images\wood_bg_1.jpg")
                    ),
                Stretch = Stretch.UniformToFill
            }; // horizontal wood grain
            BORDER_BG = new ImageBrush
            {
                ImageSource = new BitmapImage(
                    new Uri(this.BaseUri, @"Assets\Images\Dark-Wood-Background.jpg")
                    ),
                Stretch = Stretch.UniformToFill
            };
        }
    }
}
