using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Nodes;
using SharpDX;

namespace HighlightMover
{
    public class HighlightMoverCore : BaseSettingsPlugin<HighlightMoverSettings>
    {
        private Coroutine worker;
        private const string coroutineName = "Move";
        Random rand = new Random();
        private bool running = false;

        public HighlightMoverCore()
        {
            Name = "HighlightMover";
        }

        public override bool Initialise()
        {
            Input.RegisterKey(Settings.Hotkey);

            Settings.Hotkey.OnValueChanged += () => { Input.RegisterKey(Settings.Hotkey); };

            return true;
        }

        public override void Render()
        {
            if (Settings.Hotkey.PressedOnce())
            {
                if (!running)
                {
                    running = true;
                    worker = new Coroutine(ProcessItems(), this, coroutineName);
                    Core.ParallelRunner.Run(worker);
                }
            }
        }

        private IEnumerator ProcessItems()
        {
            var currentStashItems = GameController.Game.IngameState.IngameUi.StashElement.VisibleStash.VisibleInventoryItems;

            foreach (NormalInventoryItem item in currentStashItems)
            {
                var uiTabsOpened = GameController.Game.IngameState.IngameUi.InventoryPanel.IsVisible &&
                                   GameController.Game.IngameState.IngameUi.StashElement.IsVisibleLocal;

                if (!uiTabsOpened)
                {
                    running = false;
                    yield break;
                }

                if (item.isHighlighted)
                {
                    MoveMouseToElement(item.GetClientRect().Center);
                    yield return new WaitTime(2);
                    Input.KeyDown(System.Windows.Forms.Keys.LControlKey);
                    yield return new WaitTime(2);
            
                    uiTabsOpened = GameController.Game.IngameState.IngameUi.InventoryPanel.IsVisible &&
                              GameController.Game.IngameState.IngameUi.StashElement.IsVisibleLocal;

                    if (!uiTabsOpened)
                    {
                        Input.KeyUp(System.Windows.Forms.Keys.LControlKey);
                        running = false;
                        yield break;
                    }

                    Input.Click(System.Windows.Forms.MouseButtons.Left);
                    yield return new WaitTime(3);
                    Input.Click(System.Windows.Forms.MouseButtons.Left);
                    Input.Click(System.Windows.Forms.MouseButtons.Left);

                    Input.KeyUp(System.Windows.Forms.Keys.LControlKey);
                    yield return new WaitTime(3);
                    
                }

            }
            running = false;
        }

        private void MoveMouseToElement(Vector2 pos)
        {
            Input.SetCursorPos(pos + GameController.Window.GetWindowRectangle().TopLeft);
        }

    }

}
