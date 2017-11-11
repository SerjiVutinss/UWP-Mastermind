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
    // PegContainer class which is used to house the pegs.
    // This custom element can be used to either hold the pegs for 
    // each turn or to hold the pegs for the solution.
    // Each PegContainer contains a numPegs(int) number of pegs and 
    // is constructed using a reference to its parent element, 
    // which is used to name the container for easy retrieval within 
    // the game logic
    // TODO: maybe better to simply pass the parent.Name string?

    // extends the Grid element
    public class PegContainer : Grid
    {

        // peg display values
        // set a margin based on the peg size

        // constructor takes a reference to the parent element and
        // the number of pegs (and columns) to be inserted in each container
        public PegContainer(StackPanel parent, int numPegs)
        {
            // give the turn a name

            // set some visual properties for the container
            //this.BorderBrush = MainPage.PEG_CONTAINER_COLOUR;
            //this.BorderThickness = new Thickness(PEG_CONTAINER_BORDER);
            this.Padding = new Thickness(MainPage.PEG_CONTAINER_PADDING);
            this.Margin = new Thickness(MainPage.PEG_CONTAINER_PADDING);
            this.Name = parent.Name + "pegs";

            // add a new row which, in the case of turns,
            // will be used to indicate the current turn and peg
            this.RowDefinitions.Add(new RowDefinition());

            // add an ellipse which will represent each peg
            for (int i = 1; i <= numPegs; i++)
            {
                // each peg will be placed in its own column,
                // so add a new ColumnDefinition to the PegContainer
                this.ColumnDefinitions.Add(new ColumnDefinition());
                // build a new peg using PegWrapperClass
                PegWrapper pegLocationWrapper = new PegWrapper(
                    parent.Name + "loc",
                    i,
                    MainPage.BORDER_BG,
                    MainPage.PEG_LOCATION_SIZE);

                // add the wrapped element Peg to the PegContainer (this) 
                // which extends Grid
                this.Children.Add(pegLocationWrapper.Peg);
            }
        }
    }
}
