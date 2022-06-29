using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SFTGTrainer.ModMenu.Elements
{
    public class MenuToggleableButton : MenuElement
    {
        public new bool enabled = false;

        public ToggleCallback Toggled = delegate { };

        public string baseText;

        public MenuToggleableButton()
        {
            InitCallback = OnInit;
            ClickCallback = OnClick;
        }

        public override MenuButtonType GetElementType()
        {
            return MenuButtonType.Toggle;
        }

        public void Toggle()
        {
            OnClick();
        }

        private void setColor(Button button)
        {
            if (button == null) button = GetButton();

            ColorBlock block = GetButton().colors;

            if (enabled)
            {
                block.normalColor = Constant.MENU_BUTTON_ON;
                block.pressedColor = Constant.MENU_BUTTON_ON_PRESSED;
                block.highlightedColor = Constant.MENU_BUTTON_ON_HOVERED;
            }
            else
            {
                block.normalColor = Constant.MENU_BUTTON_OFF;
                block.pressedColor = Constant.MENU_BUTTON_OFF_PRESSED;
                block.highlightedColor = Constant.MENU_BUTTON_OFF_HOVERED;
            }

            GetButton().colors = block;
        }

        void OnInit(Button button)
        {
            setColor(button);
            baseText = getText();
            setText(baseText + ": " + (enabled ? "ON" : "OFF"));
        }

        private void OnClick()
        {
            enabled = !enabled;
            setText(baseText + ": " + (enabled ? "ON" : "OFF"));
            setColor(null);
            Toggled(enabled);
        }

        public delegate void ToggleCallback(bool state);
    }
}
