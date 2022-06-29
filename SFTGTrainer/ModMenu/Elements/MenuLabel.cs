using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SFTGTrainer.ModMenu.Elements
{
    public class MenuLabel : MenuElement
    {
        public MenuLabel()
        {
            InitCallback = OnInit;
        }

        void OnInit(Button button)
        {
            button.image.color = new Color32(0, 0, 0, 0);
        }
    }
}
