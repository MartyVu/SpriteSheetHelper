using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteSheetHelper
{
    public class ToolsType : Enumeration
    {
        public static ToolsType Mouse = new MouseType();
        public static ToolsType Frame = new OriginType();
        public static ToolsType Zoom = new ZoomType();
        public static ToolsType Hand = new HandType();

        public ToolsType() : base()
        {
        }

        public ToolsType(int id, string name) : base(id, name)
        { }

        private class MouseType : ToolsType
        {
            public MouseType() : base(1, "Mouse Tool")
            { }
        }

        private class OriginType : ToolsType
        {
            public OriginType() : base(2, "Frame Tool")
            { }
        }

        private class ZoomType : ToolsType
        {
            public ZoomType() : base(3, "Zoom Tool")
            { }
        }

        private class HandType : ToolsType
        {
            public HandType() : base(4, "Hand Tool")
            { }
        }
    }
}
