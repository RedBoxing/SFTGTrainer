using System;
using UnityEngine;

namespace SFTGTrainer.ModMenu
{
    public class TitleInfo
    {
        public Font font;
        public string text;
        public string footer;

        public TitleInfo(Font font, string text, string footer)
        {
            this.font = font;
            this.text = text;
            this.footer = footer;
        }
    }
}
