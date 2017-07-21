using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inker.Services
{
    public class GridSettingsService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public GridType Type { get; set; } = GridType.DOTTED_PAPER;
        public float Scale { get; set; } = 1.0f;
    }
}
