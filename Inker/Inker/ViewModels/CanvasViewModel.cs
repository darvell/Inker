using Caliburn.Micro;
using Inker.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Ink;
using System.Linq;
using Inker.Utilities;
using Microsoft.Win32;

namespace Inker.ViewModels
{
    public class CanvasViewModel : PropertyChangedBase, IHandle<Hotkey>
    {
        private BrushSettingsService _brushSettings;
        private GridSettingsService _gridSettings;
        private const int MAX_HISTORY = 50;

        private List<Tuple<StrokeCollection, bool>> _strokeHistory = new List<Tuple<StrokeCollection, bool>>();

        public CanvasViewModel()
        {
            _brushSettings = new BrushSettingsService(); // Design time.
            _gridSettings = new GridSettingsService();
        }

        public CanvasViewModel(BrushSettingsService brushSettings, GridSettingsService gridSettingsService, IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
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

            UserCanvasStrokes.StrokesChanged += (sender, args) =>
            {
                if (_strokeHistory.Count >= MAX_HISTORY)
                    _strokeHistory.RemoveAt(0);

                if (args.Added?.Count > 0)
                {
                    _strokeHistory.Add(Tuple.Create(args.Added, true));
                }
                else if (args.Removed?.Count > 0)
                {
                    _strokeHistory.Add(Tuple.Create(args.Removed, false));
                }
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

        public StrokeCollection UserCanvasStrokes { get; set; } = new StrokeCollection();

        public void Handle(Hotkey message)
        {
            var lastThing = _strokeHistory.LastOrDefault();
            if (lastThing == null)
                return;

            switch (message)
            {
                case Hotkey.UNDO:
                    _strokeHistory.Remove(lastThing);
                    if (lastThing.Item2)
                    {
                        UserCanvasStrokes.Remove(lastThing.Item1);
                    }
                    else
                    {
                        UserCanvasStrokes.Add(lastThing.Item1);
                    }
                    break;
                case Hotkey.SAVE:
                    Save();
                    break;
                case Hotkey.LOAD:
                    Load();
                    break;
            }
        }

        public void Save()
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                FileName = "Sketch",
                DefaultExt = ".svg",
                Filter = "Sketches (.svg)|*.svg"
            };

            bool? result = saveDialog.ShowDialog();
            if (result == true)
            {
                File.WriteAllText(saveDialog.FileName, UserCanvasStrokes.ToSvg(true));
            }
        }

        public void Load()
        {

            OpenFileDialog openDialog = new OpenFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".svg",
                Filter = "Sketches (.svg)|*.svg"
            };
            bool? result = openDialog.ShowDialog();
            if (result == true)
            {
                try
                {
                    UserCanvasStrokes.LoadFromSvgFileMetadata(openDialog.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show("No stroke metadata found in SVG file.", Application.Current.MainWindow.Title,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}