using Caliburn.Micro;
using System.Diagnostics;
using System.Windows.Input;
using System;
using Inker.Views;

namespace Inker.ViewModels
{
    public class ShellViewModel : PropertyChangedBase, IShell
    {
        public CanvasViewModel Canvas { get; private set; }
        public ToolbarViewModel Toolbar { get; private set; }

        private IEventAggregator _eventAggregator;

        public event EventHandler<ViewAttachedEventArgs> ViewAttached;

        public ShellViewModel(IEventAggregator eventAggregator, CanvasViewModel canvasViewModel, ToolbarViewModel toolbarViewModel)
        {
            _eventAggregator = eventAggregator;
            Canvas = canvasViewModel;
            Toolbar = toolbarViewModel;
            ViewAttached += (sender, args) =>
            {
                Keyboard.Focus(((ShellView)args.View));
            };
        }

        public void HandleKeyInput(KeyEventArgs keyArgs)
        {
            switch (keyArgs.Key)
            {
                case Key.OemPlus:
                case Key.Add:
                    _eventAggregator.PublishOnUIThread(Hotkey.INCREASE_BRUSH);
                    break;

                case Key.OemMinus:
                case Key.Subtract:
                    _eventAggregator.PublishOnUIThread(Hotkey.DECREASE_BRUSH);
                    break;

                case Key.H:
                    _eventAggregator.PublishOnUIThread(Hotkey.TOGGLE_HIGHLIGHTER);
                    break;

                case Key.G:
                    _eventAggregator.PublishOnUIThread(Hotkey.TOGGLE_GRID);
                    break;

                case Key.S:
                    _eventAggregator.PublishOnUIThread(
                        (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control
                            ? Hotkey.SAVE
                            : Hotkey.TOGGLE_SMOOTHING);
                    break;

                case Key.L:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                        _eventAggregator.PublishOnUIThread(Hotkey.LOAD);
                    break;

                case Key.OemOpenBrackets:
                    _eventAggregator.PublishOnUIThread(Hotkey.DECREASE_GRID);
                    break;

                case Key.OemCloseBrackets:
                    _eventAggregator.PublishOnUIThread(Hotkey.INCREASE_GRID);
                    break;

                case Key.Z:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                        _eventAggregator.PublishOnUIThread(Hotkey.UNDO);
                    break;
            }
        }
    }
}