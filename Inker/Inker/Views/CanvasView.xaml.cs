using System.Diagnostics;
using System.Windows.Controls;

namespace Inker.Views
{
    /// <summary>
    /// Interaction logic for CanvasView.xaml
    /// </summary>
    public partial class CanvasView : UserControl
    {
        public CanvasView()
        {
            InitializeComponent();
            KeyDown += (sender, args) =>
            {
                Debug.WriteLine($"KeyDown: {args.Key} SysKey: {args.SystemKey}");
            };
        }
    }
}