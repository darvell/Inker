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