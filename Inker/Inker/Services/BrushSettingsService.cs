using Caliburn.Micro;
using System.ComponentModel;
using System.Windows.Media;

namespace Inker.Services
{
    // Acts as a single source of truth on brush settings.
    public class BrushSettingsService : INotifyPropertyChanged, IHandle<Hotkey>
    {
        public Color Color { get; set; } = Color.FromRgb(0, 0, 0);
        public Color HighlightColor { get; set; } = Color.FromArgb(255 / 2, 255, 255, 0);
        public int Thickness { get; set; } = 4;
        public BrushType Type { get; set; } = BrushType.PEN;
        public bool Smoothing { get; set; } = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public BrushSettingsService()
        {
            // ignored for design time
        }

        public BrushSettingsService(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
        }

        public void Handle(Hotkey message)
        {
            switch (message)
            {
                case Hotkey.INCREASE_BRUSH:
                    Thickness += 1;
                    break;

                case Hotkey.DECREASE_BRUSH:
                    if (Thickness > 1)
                        Thickness -= 1;
                    break;

                case Hotkey.TOGGLE_HIGHLIGHTER:
                    Type = (Type != BrushType.HIGHLIGHTER ? BrushType.HIGHLIGHTER : BrushType.PEN);
                    break;

                case Hotkey.TOGGLE_SMOOTHING:
                    Smoothing = !Smoothing;
                    break;
            }
        }
    }
}