using System.Runtime.InteropServices;
using Caliburn.Micro;
using Inker.Messages;
using Inker.ViewModels;
using System.Windows.Input;

namespace Inker.ViewModels
{
    public class ShellViewModel : PropertyChangedBase, IShell
    {
        public CanvasViewModel Canvas { get; private set; }
        private IEventAggregator _eventAggregator;
        public ShellViewModel(IEventAggregator eventAggregator, CanvasViewModel canvasViewModel)
        {
            _eventAggregator = eventAggregator;
            Canvas = canvasViewModel;
        }

        public void HandleKeyInput(KeyEventArgs keyArgs)
        {
            switch (keyArgs.Key)
            {
                case Key.OemPlus:
                case Key.Add:
                    _eventAggregator.PublishOnUIThread(new BrushSizeIncreaseMessage());
                    break;
                case Key.OemMinus:
                case Key.Subtract:
                    _eventAggregator.PublishOnUIThread(new BrushSizeDecreaseMessage());
                    break;
                case Key.H:
                    _eventAggregator.PublishOnUIThread(new BrushTypeChangeMessage(BrushType.HIGHLIGHTER));
                    break;
                case Key.P:
                    _eventAggregator.PublishOnUIThread(new BrushTypeChangeMessage(BrushType.PEN));
                    break;
            }
        }
    }
}