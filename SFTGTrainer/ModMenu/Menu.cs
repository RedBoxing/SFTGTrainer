using System.Collections.Generic;
using UnityEngine;
using SFTGTrainer.ModMenu.Elements;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SFTGTrainer.ModMenu
{
    public class Menu : MonoBehaviour
    {
        public static Menu Instance;
        private Canvas canvas;
        private GameObject mainObject;
        private GameObject eventSystem;
        private GameObject panelObj;

        public List<MenuElement> elements;
        public int totalElements;
        private ButtonPositioning _pos;

        public Image background { get; private set; }

        private bool drawGUI = false;

        private TextAnchor textAllign = TextAnchor.MiddleCenter;

        public ButtonPositioning pos
        {
            get
            {
                return this._pos;
            }
        }

        public List<MenuElement> GetElements()
        {
            return this.elements;
        }

        public bool isRendering()
        {
            return this._pos != null;
        }

        public bool IsDrawing()
        {
            return this.drawGUI;
        }

        public void SetTextAllign(TextAnchor allign)
        {
            this.textAllign = allign;
        }

        private T CreateElement<T>(string label) where T : MenuElement
        {
            if (!isRendering()) return null;

            Utils.CreateButton(out GameObject obj, out Text text, textAllign);

            obj.transform.SetParent(mainObject.transform);
            text.text = label;
            obj.AddComponent<T>();
            obj.name = label;

            return obj.GetComponent<T>();
        }

        public MenuButton CreateButton(string label)
        {
            if (!isRendering()) return null;

            MenuButton mButton = CreateElement<MenuButton>(label);
            mButton.Init();

            return mButton;
        }

        public MenuToggleableButton CreateToogleableButton(string label)
        {
            if (!isRendering()) return null;

            MenuToggleableButton mButton = CreateElement<MenuToggleableButton>(label);
            mButton.Init();

            return mButton;
        }

        public MenuLabel CreateLabel(string label, int fontSize = -1)
        {
            if (!isRendering()) return null;

            MenuLabel mLabel = CreateElement<MenuLabel>(label);
            mLabel.Init();

            if (fontSize != -1)
            {
                mLabel.GetTextInstance().fontSize = fontSize;
            }

            return mLabel;
        }

        public MenuSlider CreateSlider(string label)
        {
            if (!isRendering()) return null;

            MenuSlider mSlider = CreateElement<MenuSlider>(label);

            mSlider.Init();

            return mSlider;
        }

        public void RegisterElement(MenuElement element)
        {
            if (!isRendering()) return;
           
            elements.Add(element);
            totalElements = elements.Count;
            PositionButtons();
        }

        public void UnregisterElement(MenuElement element)
        {
            if (!isRendering()) return;
            
            elements.Remove(element);
            totalElements = elements.Count;
            PositionButtons();
        }

        public void StartRendering(float x, float y, float margin, float elementHeight, float elementWidth, float elementSpace, int buttonFontSize)
        {
            this._pos = new ButtonPositioning(x, y, margin, elementHeight, elementWidth, elementSpace);
            Utils.fontSize = buttonFontSize;

            background = (Image)Utils.CreateUIElement<Image>(out panelObj);
            panelObj.name = "Background";
            panelObj.transform.transform.SetParent(mainObject.transform);

            background.color = Constant.MENU_BACKGROUND;
        }

        public static Menu CreateModMenuObject()
        {
            GameObject modMenu = new GameObject();
            modMenu.AddComponent<Menu>();
            DontDestroyOnLoad(modMenu);
            return modMenu.GetComponent<Menu>();
        }

        void Start()
        {
            Instance = this;
            elements = new List<MenuElement>();
            LogUtils.debug("ModMenu by RedBoxing is initializing !");

            //Create Canvas GameObject
            mainObject = new GameObject();
            mainObject.AddComponent<Canvas>();
            mainObject.layer = 5;
            mainObject.name = "ModMenu Canvas";

            //Create Canvas
            canvas = mainObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 0.4F;

            mainObject.AddComponent<GraphicRaycaster>();

            CreateEventSystem();

            DontDestroyOnLoad(mainObject);
            totalElements = elements.Count;
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            CreateEventSystem();
        }

        void CreateEventSystem()
        {
            if (FindObjectOfType<EventSystem>() == null)
            {
                eventSystem = new GameObject();
                eventSystem.name = "Event System";
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
                DontDestroyOnLoad(eventSystem);
            }
        }

        public void PositionButtons()
        {
            if (isRendering())
            {
                _pos.SetElements(totalElements);

                foreach (MenuElement element in elements)
                {
                    Rect rect = new Rect(_pos.NextControlRect(true));

                    Button button = element.GetButton();

                    RectTransform buttonTrans = button.GetComponent<RectTransform>();
                    RectTransform textTrans = element.GetTextInstance().GetComponent<RectTransform>();

                    SetRectTransformValues(rect, buttonTrans);
                    SetRectTransformValues(new Rect(0, 0, rect.width - 10, rect.height), textTrans);

                    if (element.GetElementType() == MenuButtonType.Slider)
                    {
                        MenuSlider slider = (MenuSlider)element;

                        SetRectTransformValues(new Rect(rect.x - 45, rect.y, rect.width - 90, rect.height), buttonTrans);

                        SetRectTransformValues(_pos.GetIconPos(MenuIcon.Left, rect), slider.lessButton.GetButton().GetComponent<RectTransform>());
                        SetRectTransformValues(_pos.GetIconPos(MenuIcon.Right, rect), slider.plusButton.GetButton().GetComponent<RectTransform>());
                    }
                        

                   // if (element.HasIcon(MenuIcon.Right2)) SetRectTransformValues(_pos.GetIconPos(MenuIcon.Right2), element.rightIcon2.GetComponent<RectTransform>());
                }

                RectTransform pTrans = panelObj.GetComponent<RectTransform>();
                SetRectTransformValues(_pos.GetBackgroundRect(elements.Count, elements[elements.Count / 2].GetComponent<RectTransform>().localPosition.y), pTrans);
            }
        }

        private void SetRectTransformValues(Rect values, RectTransform rTrans)
        {
            rTrans.sizeDelta = new Vector2(values.width, values.height);

            rTrans.localScale = Vector3.one;
            rTrans.localRotation = new Quaternion(0, 0, 0, 0);

            rTrans.localPosition = new Vector3(values.x, values.y, 0);
        }

        public void ChangeMode(bool draw)
        {
            mainObject.SetActive(draw);

            foreach (MenuElement element in elements)
            {
                element.GetButton().interactable = draw;
            }

            drawGUI = draw;
        }

        void ChangePos(Vector2 newPos)
        {
            _pos.ChangePos(newPos.x, newPos.y);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                ChangeMode(!drawGUI);
            }
        }
    }
}
