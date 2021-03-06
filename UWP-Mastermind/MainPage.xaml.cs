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

        // CONSTANTS

        // Number of pegs per turn, per feedback container and 
        // in the solution container.
        // TODO: This may be variable at a later stage, feedback container
        // is toughest to implement since it is a non-linear grid
        public static readonly int NUM_PEGS_PER_TURN = 4;
        // the number of turns per game
        // TODO: again, this may be variable later
        public static readonly int NUM_TURNS = 10;

        // PEGS - layout and display values
        public static readonly SolidColorBrush PEG_CONTAINER_COLOUR = new SolidColorBrush(Colors.Black);
        // - the size of each peg in the game
        public static readonly double PEG_LOCATION_SIZE = 50;
        // the ratio of PEG_SIZE to PEG_LOCATION
        public static readonly double PEG_SIZE = PEG_LOCATION_SIZE * 0.8;

        // feedback peg size
        public static readonly double FEEDBACK_PEG_LOCATION_SIZE = PEG_LOCATION_SIZE / 2;
        public static readonly double FEEDBACK_PEG_SIZE = PEG_SIZE / 2;

        // the colour of each peg location before a move has been made
        public static readonly SolidColorBrush PEG_COLOUR = new SolidColorBrush(Colors.DarkOrange);

        // ImageBrushes assigned in SetImageBrushes()
        // main background image brush
        public static ImageBrush MAIN_BG;
        // secondary background image brush (solution or turn container)
        public static ImageBrush SECONDARY_BG;
        public static ImageBrush BORDER_BG;



        // END CONSTANTS
        #endregion Globals and 

        // PegContainer display and layout values
        //static SolidColorBrush PEG_CONTAINER_COLOUR = new SolidColorBrush(Colors.Black);
        public static double PEG_MARGIN_SIZE = PEG_LOCATION_SIZE / 5;
        public static double PEG_CONTAINER_BORDER = PEG_LOCATION_SIZE / 50;
        public static double PEG_CONTAINER_PADDING = PEG_LOCATION_SIZE / 10;

        public static double TURN_CONTAINER_PADDING = PEG_CONTAINER_PADDING / 10;

        public static double BORDER_THICKNESS = 2;

        public static double TURN_CONTAINER_WIDTH = 0;

        // used to store the solution
        PegContainer solution;
        // max number of black pegs added, if equal to NUM_PEGS_PER_TURN, you win!
        int BLACK_PEGS_ADDED = 0;
        // a list of the solution Ellipses, used to compare after each turn
        List<Ellipse> solutionList = new List<Ellipse>();

        public MainPage()
        {
            this.InitializeComponent();

            // create the colour list - only needs to be done once
            _colorList = CreateColourList();
            // assign the background image brushes
            SetImageBrushes();

            // and set some properties on the rootGrid
            PlaneProjection pp = new PlaneProjection();
            pp.RotationX = -10;
            this.rootGrid.Projection = pp;
            this.rootGrid.Background = MAIN_BG;
            this.rootGrid.BorderBrush = BORDER_BG;
            this.rootGrid.BorderThickness = new Thickness(20);

            // setup the button backgrounds
            btnNewGame.Background = SECONDARY_BG;
            btnNewGame.BorderBrush = BORDER_BG;
            btnExit.Background = SECONDARY_BG;
            btnExit.BorderBrush = BORDER_BG;

            // create a new game
            CreateNewGame();
        }

        // bunch of methods to start the game
        private void CreateNewGame()
        {
            // reset the global turn and peg numbers
            current_turn = 1;
            current_peg = 1;

            // build the board and pegs
            BuildTheBoard();
            // add the marker to show the next peg which will be populated
            AddNextMovemarker(current_turn, current_peg);

            // generate a random solution
            SetSolution();
            // hide the solution
            this.solution.Visibility = Visibility.Collapsed;
        }

        private void BuildTheBoard()
        {
            // clear the board
            this.boardGrid.Children.Clear();

            // add stackpanel to grid 1, 0 - colspan 2 - name it "spTurns"
            StackPanel spTurns = new StackPanel();
            spTurns.SetValue(Grid.ColumnProperty, 0);
            spTurns.SetValue(Grid.RowProperty, 1);

            // add a button for debugging - check the solution
            Button btn = new Button();
            btn.Content = "Reveal";
            this.boardGrid.Children.Add(btn);
            // add its handler
            btn.Tapped += btn_Tapped;

            // create the solution peg container
            // TODO: don't add the solution yet as it may need to be loaded from AppData
            // so create the solution in method SetSolution()
            this.solution = new PegContainer(spTurns, 4);
            this.solution.Name = "solution";
            this.solution.Background = SECONDARY_BG;
            this.solution.SetValue(Grid.ColumnProperty, 0);
            this.solution.SetValue(Grid.RowProperty, 0);
            this.solution.HorizontalAlignment = HorizontalAlignment.Right;
            // TODO: calculate total size of a turncontainer 
            //  - (and then) set a left padding here
            this.solution.Padding = new Thickness(
                60,
                MainPage.PEG_CONTAINER_PADDING,
                MainPage.PEG_CONTAINER_PADDING,
                MainPage.PEG_CONTAINER_PADDING);

            this.solution.BorderBrush = BORDER_BG;
            this.solution.BorderThickness = new Thickness(BORDER_THICKNESS);
            this.boardGrid.Children.Add(this.solution);

            // for each turn (numTurns), add a turn container to spTurns - decrementing loop
            // add each turn container to the stackpanel
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
            // add this to a stackpanel
            StackPanel spColourPaletteContainer = new StackPanel();
            ColourPalette cpColourPalette = new ColourPalette(this, current_turn, current_peg);
            cpColourPalette.Name = "colourPallette";
            cpColourPalette.Margin = new Thickness(5);
            spColourPaletteContainer.SetValue(Grid.ColumnProperty, 1);
            spColourPaletteContainer.SetValue(Grid.RowProperty, 1);
            spColourPaletteContainer.Children.Add(cpColourPalette);
            this.boardGrid.Children.Add(spColourPaletteContainer);
        }

        // handler for the 'reveal' button
        private void btn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.solution.Visibility == Visibility.Collapsed)
            {
                this.solution.Visibility = Visibility.Visible;
            }
            else
            {
                this.solution.Visibility = Visibility.Collapsed;
            }
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


            // add an ellipse object in row 1, column numPeg -1 of the turn container
            Ellipse nextMove = new Ellipse();
            nextMove.Name = "nextMoveMarker";
            nextMove.Height = 10;
            nextMove.Width = 10;
            nextMove.Fill = new SolidColorBrush(Colors.White);

            nextMove.SetValue(Grid.RowProperty, 0);
            nextMove.SetValue(Grid.ColumnProperty, numPeg - 1);

            turnContainer.Children.Add(nextMove);

        }

        // generate a new solution and add it to the PegContainer solution object
        private void SetSolution()
        {

            // first, reset the solutionList
            this.solutionList = new List<Ellipse>();
            // TODO: do this a better way
            //this.solution.Width = TURN_CONTAINER_WIDTH;
            // for each number of pegs per turn,
            // add a random colour to the peg container named "solution"
            // name each peg solutionPeg + i
            Ellipse solutionPeg;
            // better to create a random object here and call .Next() multiple times
            Random rand = new Random();
            for (int i = 1; i <= NUM_PEGS_PER_TURN; i++)
            {
                // add the peg location
                PegWrapper pegLocationWrapper = new PegWrapper(solution.Name, i, BORDER_BG, PEG_LOCATION_SIZE);
                this.solution.Children.Add(pegLocationWrapper.Peg);

                // add the actual peg with colour
                solutionPeg = new Ellipse();
                // generate a random number between 0 and the length of 
                // _colourList, uppperbound is not included in random
                int _randColourIndex = rand.Next(0, _colorList.Count);
                // create a new pegWrapper with the correct arguments
                PegWrapper pegWrapper = new PegWrapper(
                    "solutionPeg",
                    i,
                    _colorList.ElementAt(_randColourIndex),
                    PEG_SIZE);
                solutionPeg = pegWrapper.Peg;
                // add it to the PegContainer
                this.solution.Children.Add(solutionPeg);
                // also add it to the solutionList
                this.solutionList.Add(solutionPeg);
            }
        }

        // increment the move - i.e. peg# and/or turn# and 
        // add a move marker to it
        public void NextMove()
        {
            // check if it is the last peg in the turn
            if (MainPage.current_peg == MainPage.NUM_PEGS_PER_TURN)
            {
                // calculate the feedback for end of turn
                CalculateTurnEnd();
                // if so, reset the peg# to 1 
                MainPage.current_peg = 1;
                // and increment the turn#
                MainPage.current_turn += 1;
                // turn is over, calculate the end of turn
            }
            else
            {
                MainPage.current_peg += 1;
            }

            // check if it was the last turn
            if (MainPage.current_turn > NUM_TURNS)
            {
                // TODO: game over
            }
            else
            {
                // add a move marker to the next move
                this.AddNextMovemarker(MainPage.current_turn, MainPage.current_peg);
            }

        }

        // called from NextMove() if a new turn has been detected
        // at the end of a turn, check the turn pegs against the solution pegs
        public void CalculateTurnEnd()
        {
            // going to create two lists and compare
            List<Ellipse> turnPegs = new List<Ellipse>();
            // find the correct pegContainer: named turnXpegs
            PegContainer pegContainer = FindName("turn" + current_turn + "pegs") as PegContainer;
            Debug.WriteLine(pegContainer.Name);

            // get each colour from the current turn,
            // compare these colours against the solution colours and indexes
            // simply find any Ellipses that have a name starting with
            // turnXpeg where X is the turn number
            string queryString = "turn" + current_turn + "peg";
            Debug.WriteLine(queryString);
            foreach (var item in pegContainer.Children)
            {
                if (item.GetType() == typeof(Ellipse))
                {
                    Ellipse peg = (Ellipse)item;
                    if (peg.Name.StartsWith(queryString))
                    {
                        //Debug.WriteLine);
                        turnPegs.Add(new Ellipse
                        {
                            Fill = peg.Fill
                        });
                    }
                }
            }
            if (turnPegs.Count == solutionList.Count)
            {
                // list is the correct size
                Debug.WriteLine("Checking turn " + current_turn + " values!");
                CompareLists(turnPegs);
            }
            else
            {
                // error
                Debug.WriteLine("turn " + current_turn + " error!");
            }


            // if it is the last turn
            // TODO: Game Over?
            if (BLACK_PEGS_ADDED == NUM_PEGS_PER_TURN)
            {
                // You Won
                this.solution.Visibility = Visibility.Visible;
            }
            else if (current_turn == NUM_TURNS)
            {
                this.solution.Visibility = Visibility.Visible;
                // Game Over - did not win
                // Show the solution
            }
        }

        // this method is called after each turn. it takes a parameter of a 
        // list of ellipses which have fills the same as the ones in the 
        // actual turn container, so as not to change the colours of the 
        // pegs on the board
        private void CompareLists(List<Ellipse> turnPegs)
        {
            // these will be used to ensure that a particular 
            // peg is not checked again once matched
            Brush solutionPegCheckedColor = new SolidColorBrush(Colors.Black);
            Brush turnPegCheckedColor = new SolidColorBrush(Colors.White);

            // make a copy of the solution list so as not to 
            // affect the colours in the solution itself
            List<Ellipse> solutionListCopy = new List<Ellipse>();
            foreach (Ellipse peg in solutionList)
            {
                solutionListCopy.Add(new Ellipse
                {
                    Fill = peg.Fill
                });
            }

            // use this to keep track of which location to add the next peg
            // to the feedback container
            int pegToAdd = 1;
            BLACK_PEGS_ADDED = 0;

            // first check for elements which match both position and colour
            foreach (Ellipse turnPeg in turnPegs)
            {
                // check whether the element has the same colour as
                // the corresponding solutionPeg

                // get the list index of the peg
                int i = turnPegs.IndexOf(turnPeg);
                // get the corresponding element from solutionList
                Ellipse solutionPeg = solutionListCopy.ElementAt(i);
                // compare the Fill properties
                if (turnPeg.Fill == solutionPeg.Fill)
                {
                    // elements are the same colour and position:
                    //  add a dark marker to the feedback container and
                    AddFeedBackMarker(Colors.DarkBlue, pegToAdd);
                    // increment the BLACK_PEGS_ADDED this turn
                    BLACK_PEGS_ADDED++;
                    // then set the turnPeg and solutionPeg Fill to a 
                    // color which can't be matched again
                    solutionPeg.Fill = solutionPegCheckedColor;
                    turnPeg.Fill = turnPegCheckedColor;
                    // and increment the pegsAdded so that the next peg 
                    // is placed in the correct location
                    pegToAdd++;
                }
            }
            List<Brush> addedColoursList = new List<Brush>();
            // for each turnPeg
            foreach (Ellipse turnPeg in turnPegs)
            {
                // and for each solution peg
                foreach (Ellipse solutionPeg in solutionListCopy)
                {
                    // check if the colours match
                    if (turnPeg.Fill == solutionPeg.Fill)
                    {
                        // if they do, add a white marker
                        AddFeedBackMarker(Colors.White, pegToAdd);
                        // set the colours of these pegs to colours not used in 
                        // the game so they won't be checked again this turn
                        solutionPeg.Fill = solutionPegCheckedColor;
                        turnPeg.Fill = turnPegCheckedColor;
                        // and increment the feedback peg location
                        pegToAdd++;
                    }
                }
            }
        }

        // feedback containers are named turnXfeedback
        private void AddFeedBackMarker(Color colour, int pegNumber)
        {
            Debug.WriteLine("Adding feedbackpeg!!");
            // set a string to search for
            string qryString = "turn" + current_turn + "feedbackpeg" + pegNumber;
            // add a peg to the feedback container
            Ellipse fbPegLocation = FindName(qryString) as Ellipse;
            // now add a new Peg to the same parent and grid positions

            // feedback pegs will (should) be called turnXfeedbackpegYpegY
            PegWrapper pegWrapper = new PegWrapper(
                qryString + "peg",
                pegNumber,
                new SolidColorBrush(colour),
                FEEDBACK_PEG_SIZE
                );

            // TODO: maybe add a method for figuring this out. it could also 
            // be used for building the feedback containers initialy
            if (pegNumber == 1)
            {
                pegWrapper.Peg.SetValue(Grid.ColumnProperty, 0);
                pegWrapper.Peg.SetValue(Grid.RowProperty, 0);
            }
            else if (pegNumber == 2)
            {
                pegWrapper.Peg.SetValue(Grid.ColumnProperty, 1);
                pegWrapper.Peg.SetValue(Grid.RowProperty, 0);
            }
            else if (pegNumber == 3)
            {
                pegWrapper.Peg.SetValue(Grid.ColumnProperty, 0);
                pegWrapper.Peg.SetValue(Grid.RowProperty, 1);
            }
            else if (pegNumber == 4)
            {
                pegWrapper.Peg.SetValue(Grid.ColumnProperty, 1);
                pegWrapper.Peg.SetValue(Grid.RowProperty, 1);
            }

            FeedbackContainer fbContainer = FindName("turn" + current_turn + "feedback") as FeedbackContainer;
            Debug.WriteLine(fbContainer.Name);
            fbContainer.Children.Add(pegWrapper.Peg);
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
            SolidColorBrush c6 = new SolidColorBrush(Colors.Magenta);
            SolidColorBrush c7 = new SolidColorBrush(Colors.Cyan);

            colorList.Add(c1);
            colorList.Add(c2);
            colorList.Add(c3);
            colorList.Add(c4);
            colorList.Add(c5);
            colorList.Add(c6);
            colorList.Add(c7);

            return colorList;
        }

        // assign the static image brushes to images
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
            }; // darker wood grain
            BORDER_BG = new ImageBrush
            {
                ImageSource = new BitmapImage(
                    new Uri(this.BaseUri, @"Assets\Images\Dark-Wood-Background.jpg")
                    ),
                Stretch = Stretch.UniformToFill
            };
        }

        private void btnNewGame_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CreateNewGame();
        }


        private void SaveGameData()
        {
            Ellipse el;
            int ellipseCount = 0;
            for (int i = 1; i <= NUM_TURNS; i++)
            {
                for (int j = 1; i <= NUM_PEGS_PER_TURN; j++)
                {
                    // each turn peg will be called turn[i]peg[j]
                    el = FindName("turn" + i + "peg" + j) as Ellipse;
                    ellipseCount++;
                }
            }
            Debug.WriteLine(ellipseCount);

        }

        private void btnSaveGame_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SaveGameData();
        }
    }
}
