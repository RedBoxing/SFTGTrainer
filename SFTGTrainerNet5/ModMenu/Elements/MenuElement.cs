using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SFTGTrainer.ModMenu.Elements
{
    public class MenuElement : MonoBehaviour
    {
        private Button button;
        private GUIStyle style;
        private bool initialized;
        private static Texture2D btntexture;
        private static Texture2D btnpresstexture;

        public InitializeCallback InitCallback = delegate { };
        public LeftArrowCallback LeftArrow = delegate { };
        public RightArrowCallback RightArrow = delegate { };
        public PressCallback Pressed = delegate { };
        public SelectCallback Select = delegate { };

        public bool IsSelected()
        {
            return EventSystem.current.currentSelectedGameObject == gameObject;
        }

        public Button GetButton()
        {
            return this.button;
        }

        public virtual MenuButtonType GetElementType()
        {
            return MenuButtonType.Unkown;
        }

        public void setText(string text)
        {
            GetTextInstance().text = text;
        }

        public Text GetTextInstance()
        {
            return button.transform.Find("Text").gameObject.GetComponent<Text>();
        }

        public void Init()
        {
            if (!initialized)
            {
                InitStyle();
                this.button = GetComponent<Button>();
                initialized = true;
                InitCallback(button);
            }
        }

        private void InitStyle()
        {
            style = new GUIStyle();
            style.normal.background = BtnTexture;
            style.onNormal.background = BtnTexture;
            style.active.background = BtnPressTexture;
            style.onActive.background = BtnPressTexture;
            style.normal.textColor = Color.white;
            style.onNormal.textColor = Color.white;
            style.active.textColor = Color.white;
            style.onActive.textColor = Color.white;
            style.fontSize = 18;
            style.fontStyle = 0;
            style.alignment = TextAnchor.MiddleCenter;
        }

        public static Texture2D BtnTexture
        {
            get
            {
                if (btntexture == null)
                {
                    btntexture = new Texture2D(1, 1);
                    btntexture.SetPixel(0, 0, new Color32(3, 155, 229, byte.MaxValue));
                    btntexture.Apply();
                }
                return btntexture;
            }
        }
        public static Texture2D BtnPressTexture
        {
            get
            {
                if (btnpresstexture == null)
                {
                    btnpresstexture = new Texture2D(1, 1);
                    btnpresstexture.SetPixel(0, 0, new Color32(2, 119, 189, byte.MaxValue));
                    btnpresstexture.Apply();
                }
                return btnpresstexture;
            }
        }

        public delegate void InitializeCallback(Button button);
        public delegate void SelectCallback(bool fromAbove);

        public delegate void LeftArrowCallback();
        public delegate void RightArrowCallback();
        public delegate void PressCallback();
    }
}
