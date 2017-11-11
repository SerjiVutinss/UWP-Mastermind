using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UWP_Mastermind
{
    public class TurnContainer : StackPanel
    {
        // needs a FeedbackContainer and a PegContainer
        int turnNumber;
        int pegContainerSize = 4;

        FeedbackContainer feedBackContainer;
        PegContainer pegContainer;

        public TurnContainer(int turnNumber)
        {
            this.turnNumber = turnNumber;
            this.Padding = new Thickness(MainPage.PEG_CONTAINER_PADDING);

            this.BorderBrush = MainPage.BORDER_BG;
            this.BorderThickness = new Thickness(2);

            this.Background = MainPage.SECONDARY_BG;

            this.Orientation = Orientation.Horizontal;
            this.HorizontalAlignment = HorizontalAlignment.Right;

            this.Name = "turn" + turnNumber;

            this.feedBackContainer = new FeedbackContainer(this);
            this.Children.Add(feedBackContainer);

            this.pegContainer = new PegContainer(this, pegContainerSize);
            this.Children.Add(this.pegContainer);
            
        }

    }
}
