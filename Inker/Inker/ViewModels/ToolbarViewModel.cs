using System.Collections.Generic;
using System.Windows.Media;
using Caliburn.Micro;
using Inker.Services;

namespace Inker.ViewModels
{
    public class ToolbarViewModel
    {
        #region Color Presets

        public List<Color> PenColors = new List<Color>()
        {
            Color.FromRgb(0, 0, 0), // Black
            Color.FromRgb(255,0,0), // Red
            Color.FromRgb(0,255,0), // Green
            Color.FromRgb(0,0,255), // Blue

            Color.FromRgb(255,255,0), // Yellow
            Color.FromRgb(255,0,255), // Purple
            Color.FromRgb(0,255,255), // Cyan

            Color.FromRgb(26,188,156), //Turquoise
            Color.FromRgb(46,204,113), //Emerland
            Color.FromRgb(52,152,219), //Peterriver
            Color.FromRgb(155,89,182), //Amethyst
            Color.FromRgb(52,73,94), //Wetasphalt
            Color.FromRgb(22,160,133), //Greensea
            Color.FromRgb(39,174,96), //Nephritis
            Color.FromRgb(41,128,185), //Belizehole
            Color.FromRgb(142,68,173), //Wisteria
            Color.FromRgb(44,62,80), //Midnightblue
            Color.FromRgb(241,196,15), //Sunflower
            Color.FromRgb(230,126,34), //Carrot
            Color.FromRgb(231,76,60), //Alizarin
            Color.FromRgb(236,240,241), //Clouds
            Color.FromRgb(149,165,166), //Concrete
            Color.FromRgb(243,156,18), //Orange
            Color.FromRgb(211,84,0), //Pumpkin
            Color.FromRgb(192,57,43), //Pomegranate
            Color.FromRgb(189,195,199), //Silver
            Color.FromRgb(127,140,141), //Asbestos
        };

        public List<Color> HighlighterColors = new List<Color>()
        {
            Color.FromArgb(100, 255, 255 ,0),
            Color.FromArgb(100, 255, 0 ,0),
            Color.FromArgb(100, 255, 0 ,255),
            Color.FromArgb(100, 0, 255 ,0),
        };

        #endregion Color Presets

        private BrushSettingsService _brushSettings;
        private GridSettingsService _gridSettings;
        private IEventAggregator _eventAggregator;

        public ToolbarViewModel()
        {
            _brushSettings = new BrushSettingsService();
            _gridSettings = new GridSettingsService();
        }

        public ToolbarViewModel(BrushSettingsService brushSettings, GridSettingsService gridSettings, IEventAggregator eventAggregator)
        {
            _brushSettings = brushSettings;
            _gridSettings = gridSettings;
            _eventAggregator = eventAggregator;
        }

        public bool GridEnabled
        {
            get => _gridSettings.Type != GridType.NONE;
            set => _gridSettings.Type = value ? GridType.DOTTED_PAPER : GridType.NONE;
        }

        public void Save()
        {
            // Be cheeky and just hook the existing hotkey handling :)
            _eventAggregator.PublishOnUIThread(Hotkey.SAVE);
        }

        public void Load()
        {
            _eventAggregator.PublishOnUIThread(Hotkey.LOAD);
        }

        public void Undo()
        {
            _eventAggregator.PublishOnUIThread(Hotkey.UNDO);
        }

        public void New()
        {
            _eventAggregator.PublishOnUIThread(Hotkey.NEW);
        }

    }
}