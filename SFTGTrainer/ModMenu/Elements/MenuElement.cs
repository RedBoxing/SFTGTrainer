using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SFTGTrainer.ModMenu.Elements
{
    public class MenuElement : MonoBehaviour
    {
        private Button button;
        private bool initialized;

        protected InitializeCallback InitCallback = delegate { };
        protected UnityAction ClickCallback = delegate { };

        public LeftArrowCallback LeftArrow = delegate { };
        public RightArrowCallback RightArrow = delegate { };
        public PressCallback Pressed = delegate { };

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

        public string getText()
        {
            return GetTextInstance().text;
        }

        public Text GetTextInstance()
        {
            return button.transform.Find("Text").gameObject.GetComponent<Text>();
        }

        public void Init()
        {
            if (!initialized)
            {
                this.button = GetComponent<Button>();
                button.onClick.AddListener(ClickCallback);

                initialized = true;
                InitCallback(button);
            }
        }

        protected delegate void InitializeCallback(Button button);

        public delegate void LeftArrowCallback();
        public delegate void RightArrowCallback();
        public delegate void PressCallback();
    }
}
