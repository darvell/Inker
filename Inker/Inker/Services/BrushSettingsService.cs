using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Inker.Annotations;
using System.Drawing;
using System.Windows.Media;
using Caliburn.Micro;
using Inker.Messages;

namespace Inker.Services
{
    // Acts as a single source of truth on brush settings.
    public class BrushSettingsService : INotifyPropertyChanged, IHandle<BrushSizeIncreaseMessage>, IHandle<BrushSizeDecreaseMessage>
    {
        public Color Color { get; set; } = Color.FromRgb(0,0,0);
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

        public void Handle(BrushSizeIncreaseMessage message)
        {
            Thickness += 1;
        }

        public void Handle(BrushSizeDecreaseMessage message)
        {
            if (Thickness > 1)
                Thickness -= 1;
        }
    }
}
