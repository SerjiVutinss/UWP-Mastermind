using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace UWP_Mastermind
{
    // cannot extend sealed class Ellipse
    public class PegLocationWrapper
    {
        // workaround
        public Ellipse pegLocation;
        public PegLocationWrapper(StackPanel parent, int pegLocationNumber)
        {
            this.pegLocation = new Ellipse();
            // build a new peg and set some values
            this.pegLocation = new Ellipse();
            this.pegLocation.Fill = MainPage.BORDER_BG;
            this.pegLocation.Margin = new Thickness(MainPage.PEG_MARGIN_SIZE);
            this.pegLocation.Height = MainPage.PEG_LOCATION_SIZE;
            this.pegLocation.Width = MainPage.PEG_LOCATION_SIZE;
            // give it a name which is based on the parent.Name and peg#(i)
            this.pegLocation.Name = parent.Name + "pegLocation" + pegLocationNumber;
            // as turn peg numbers are not zero-based and 
            // column values are, decrement the peg number by 1
            // and place in that column
            this.pegLocation.SetValue(Grid.RowProperty, 0);
            this.pegLocation.SetValue(Grid.ColumnProperty, pegLocationNumber -1);
        }
    }
}
