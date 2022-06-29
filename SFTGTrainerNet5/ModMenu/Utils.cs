using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SFTGTrainer.ModMenu
{
    public class Utils
    {
        public static int fontSize;

        public static Component CreateUIElement<T>(out GameObject obj) where T : Component
        {
            obj = new GameObject();

            obj.AddComponent<CanvasRenderer>();
            obj.AddComponent<RectTransform>();

            RectTransform rTrans = obj.GetComponent<RectTransform>();
            rTrans.localPosition = Vector3.zero;
            rTrans.anchoredPosition = Vector3.zero;

            return obj.AddComponent<T>();
        }

        public static Button CreateButton(out GameObject obj, out Text text, TextAnchor textAllign)
        {
            Button button = (Button)CreateUIElement<Button>(out obj);

            button.targetGraphic = obj.AddComponent<Image>();

            ColorBlock block = button.colors;

            Color clr = Color.gray;

         //   block.normalColor = clr;
            block.highlightedColor = clr;
            block.pressedColor = clr;
            button.colors = block;

            Navigation nav = button.navigation;
            nav.mode = Navigation.Mode.None;
            button.navigation = nav;

            text = (Text)CreateUIElement<Text>(out GameObject txt);
            txt.name = "Text";
            txt.transform.SetParent(obj.transform);

            text.text = "Button";
            text.color = Color.black;
            text.font = Font.CreateDynamicFontFromOSFont("Arial", fontSize);
            text.fontSize = fontSize;
            text.alignment = textAllign;

            return button;
        }
    }
}
