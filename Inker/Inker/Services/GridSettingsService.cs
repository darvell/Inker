using Caliburn.Micro;
using Inker.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inker.Services
{
    public class GridSettingsService : INotifyPropertyChanged, IHandle<GridScaleIncreaseMessage>, IHandle<GridScaleDecreaseMessage>, IHandle<GridToggle>
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

        public void Handle(GridScaleIncreaseMessage message)
        {
            Scale += 0.05f;
        }

        public void Handle(GridScaleDecreaseMessage message)
        {
            if (Scale > 0.05)
                Scale -= 0.05f;
        }

        public void Handle(GridToggle message)
        {
            if (Type != GridType.NONE)
            {
                Type = GridType.NONE;
            }
            else
            {
                Type = GridType.DOTTED_PAPER;
            }
        }
    }
}