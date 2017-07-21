using Caliburn.Micro;
using System.ComponentModel;

namespace Inker.Services
{
    public class GridSettingsService : INotifyPropertyChanged, IHandle<Hotkey>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public GridSettingsService()
        {
        }

        public GridSettingsService(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
        }

        public GridType Type { get; set; } = GridType.DOTTED_PAPER;
        public float Scale { get; set; } = 1.0f;

        public void Handle(Hotkey message)
        {
            switch (message)
            {
                case Hotkey.TOGGLE_GRID:
                    Type = (Type != GridType.NONE ? GridType.NONE : GridType.DOTTED_PAPER);
                    break;

                case Hotkey.INCREASE_GRID:
                    Scale += 0.05f;
                    break;

                case Hotkey.DECREASE_GRID:
                    if (Scale > 0.05f)
                        Scale -= 0.05f;
                    break;
            }
        }
    }
}