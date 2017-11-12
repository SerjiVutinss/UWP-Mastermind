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
    // class is ised to construct pegLocations and pegs
    public class PegWrapper
    {
        // the wrapped peg element with getter
        public Ellipse Peg { get; }

        public PegWrapper(
            String namePrefix,
            int pegNumber,
            Brush colour,
            double pegSize
            )
        {
            // build a new peg and set some values
            this.Peg = new Ellipse();
            this.Peg.Fill = colour;
            //this.Peg.Margin = new Thickness(MainPage.PEG_MARGIN_SIZE);
            this.Peg.Height = pegSize;
            this.Peg.Width = pegSize;
            // give it a name which is based on the parent.Name and peg#(i)
            this.Peg.Name = namePrefix + pegNumber;
            // as turn peg numbers are not zero-based and 
            // column values are, decrement the peg number by 1
            // and place in that column
            this.Peg.SetValue(Grid.RowProperty, 0);
            this.Peg.SetValue(Grid.ColumnProperty, pegNumber - 1);
        }
    }
}
