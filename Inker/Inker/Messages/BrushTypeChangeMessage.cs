using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inker.Messages
{
    public class BrushTypeChangeMessage
    {
        public BrushType Type { get; private set; }
        public BrushTypeChangeMessage(BrushType type)
        {
            Type = type;
        }
    }
}
