using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace UWP_Mastermind
{
    public class BrushWrapper
    {
        public string brushName;
        public Brush brush;

        public BrushWrapper(
            string brushName,
            Brush brush
            )
        {
            this.brushName = brushName;
            this.brush = brush;
        }
    }
}
