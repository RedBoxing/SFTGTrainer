using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SFTGTrainer.ModMenu
{
    public class Utils
    {
        public static int fontSize;

        public static T GetFieldValue<T>(object obj, string fieldName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field == null)
            {
                throw new ArgumentException("fieldName", "No such field was found.");
            }
            if (!typeof(T).IsAssignableFrom(field.FieldType))
            {
                throw new InvalidOperationException("Field type and requested type are not compatible.");
            }
            return (T)((object)field.GetValue(obj));
        }

        public static void SetFieldValue<T>(object obj, string fieldName, object value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field == null)
            {
                throw new ArgumentException("fieldName", "No such field was found.");
            }
            if (!typeof(T).IsAssignableFrom(field.FieldType))
            {
                throw new InvalidOperationException("Field type and requested type are not compatible.");
            }
            field.SetValue(obj, (T)((object)value));
        }

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

            block.normalColor = Constant.MENU_BUTTON;
            block.pressedColor = Constant.MENU_BUTTON_PRESSED;
            block.highlightedColor = Constant.MENU_BUTTON_HOVERED;

            button.colors = block;

            Navigation nav = button.navigation;
            nav.mode = Navigation.Mode.None;
            button.navigation = nav;

            text = (Text)CreateUIElement<Text>(out GameObject txt);
            txt.name = "Text";
            txt.transform.SetParent(obj.transform);

            text.text = "Button";
            text.color = Color.white;
            text.font = Font.CreateDynamicFontFromOSFont("Arial", fontSize);
            text.fontSize = fontSize;
            text.alignment = textAllign;

            return button;
        }

        public Color GetPlayerColorByIndex(int index)
        {
            Color result;
            switch (index)
            {
                case 0:
                    result = Color.yellow;
                    break;
                case 1:
                    result = Color.cyan;
                    break;
                case 2:
                    result = Color.red;
                    break;
                case 3:
                    result = Color.green;
                    break;
                default:
                    result = Color.white;
                    break;
            }
            return result;
        }
    }
}
