using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media;
using Caliburn.Micro;
using Inker.Services;
using System.Windows;
using System.Diagnostics;

namespace Inker.ViewModels
{
    public class CanvasViewModel : PropertyChangedBase
    {
        private BrushSettingsService _brushSettings;
        private GridSettingsService _gridSettings;

        public CanvasViewModel()
        {
            _brushSettings = new BrushSettingsService(); // Design time.
            _gridSettings = new GridSettingsService();
        }

        public CanvasViewModel(BrushSettingsService brushSettings, GridSettingsService gridSettingsService)
        {
            _brushSettings = brushSettings;
            _gridSettings = gridSettingsService;

            // TODO: Remember the fody thing that'll auto handle this
            _brushSettings.PropertyChanged += (sender, args) =>
            {
                NotifyOfPropertyChange(nameof(DrawingAttributes));
            };

            _gridSettings.PropertyChanged += (sender, args) =>
            {
                NotifyOfPropertyChange(nameof(DottedBrushViewport));
                NotifyOfPropertyChange(nameof(OverlayVisiblility));
            };
        }

        // Wrap our brush settings in to the Windows Ink drawing attributes class.
        public DrawingAttributes DrawingAttributes => new DrawingAttributes()
        {
            Color = (_brushSettings.Type == BrushType.PEN) ? _brushSettings.Color : _brushSettings.HighlightColor,
            FitToCurve = _brushSettings.Smoothing,
            Height = _brushSettings.Thickness,
            Width = _brushSettings.Thickness,
            IgnorePressure = false,
            IsHighlighter = _brushSettings.Type == BrushType.HIGHLIGHTER,
            StylusTip = StylusTip.Ellipse
        };

        public Rect DottedBrushViewport
        {
            get
            {
                return new Rect(0, 0, 12.5 * _gridSettings.Scale, 12.5 * _gridSettings.Scale);
            }
        }

        public Visibility OverlayVisiblility
        {
            get
            {
                return _gridSettings.Type != GridType.NONE ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }
}