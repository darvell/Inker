using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media;
using Caliburn.Micro;
using Inker.Services;

namespace Inker.ViewModels
{
    public class CanvasViewModel : PropertyChangedBase
    {
        private BrushSettingsService _brushSettings;

        public CanvasViewModel()
        {
            _brushSettings = new BrushSettingsService(); // Design time.
        }

        public CanvasViewModel(BrushSettingsService brushSettings)
        {
            _brushSettings = brushSettings;
            // TODO: Remember the fody thing that'll auto handle this
            _brushSettings.PropertyChanged += (sender, args) =>
            {
                NotifyOfPropertyChange(nameof(DrawingAttributes));
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

    }
}
