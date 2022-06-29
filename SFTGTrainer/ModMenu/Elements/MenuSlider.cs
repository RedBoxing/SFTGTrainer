using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace SFTGTrainer.ModMenu.Elements
{
    public class MenuSlider : MenuElement
    {
        public MenuButton plusButton;
        public MenuButton lessButton;

        public float minValue = 1;
        public float maxValue = 100;
        public float step = 1;
        public float value = 1;

        public ValueChangeCallback Change = delegate { };

        public MenuSlider()
        {
            InitCallback = OnInit;
        }

        public override MenuButtonType GetElementType()
        {
            return MenuButtonType.Slider;
        }

        void OnInit(Button button)
        {
            plusButton = Menu.Instance.CreateButton("+");
            lessButton = Menu.Instance.CreateButton("-");

            plusButton.Pressed = Increase;
            lessButton.Pressed = Decrease;

           // Menu.Instance.GetElements().AddRange(new List<MenuElement>() { plusButton, lessButton });
           // Menu.Instance.totalElements = Menu.Instance.GetElements().Count;
        }

        public void Decrease()
        {
            float oldValue = value;
            value -= step;

            if (value < minValue)
            {
                value = minValue;
            }
            else
                Change(value);
        }

        public void Increase()
        {
            float oldValue = value;
            value += step;

            if (value > maxValue)
            {
                value = maxValue;
            }
            else
                Change(value);
        }

        public delegate void ValueChangeCallback(float newValue);

    }
}
