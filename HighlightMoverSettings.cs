using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using System.Windows.Forms;

namespace HighlightMover
{
    public class HighlightMoverSettings : ISettings
    {
        public ToggleNode Enable { get; set; }
        [Menu("Hotkey")] public HotkeyNode Hotkey { get; set; }

        public HighlightMoverSettings()
        {
            Enable = new ToggleNode(true);
            Hotkey = Keys.F3;
        }


    }
}
