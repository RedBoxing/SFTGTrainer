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
        private int totalElements;
        private int selectedElementIndex;
        private MenuElement selectedElement;
        private ButtonPositioning _pos;

        public Image background { get; private set; }

        private float posX = 100F;
        private float posY = 100F;

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

        public MenuElement getSelectedElement()
        {
            return this.selectedElement;
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

            Button btn = Utils.CreateButton(out GameObject obj, out Text text, textAllign);

            //btn.image.sprite = Utils.Base64ToSprite(Base64Icons.buttonImage);

            obj.transform.SetParent(mainObject.transform);
            text.text = label;
            obj.AddComponent<T>();
            obj.name = label;

            T element = obj.GetComponent<T>();

            return element;
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

        public void RegisterElement(MenuElement element)
        {
            if (isRendering())
            {
                elements.Add(element);
                totalElements = elements.Count;
                PositionButtons();
            }
        }

        public void UnregisterElement(MenuElement element)
        {
            if (isRendering())
            {
                elements.Remove(element);
                totalElements = elements.Count;
                PositionButtons();
            }
        }

        public void StartRendering(float x, float y, float height, float margin, float elementHeight, float elementWidth, float elementSpace, int buttonFontSize)
        {
            this._pos = new ButtonPositioning(posX = x, posY = y, margin, elementHeight, elementWidth, elementSpace);
            Utils.fontSize = buttonFontSize;

            background = (Image)Utils.CreateUIElement<Image>(out panelObj);
            panelObj.name = "Background";
            panelObj.transform.transform.SetParent(mainObject.transform);
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
            LogUtils.debug("[ModMenu] ModMenu by RedBoxing is initializing !");

            //Create Canvas GameObject
            mainObject = new GameObject();
            mainObject.AddComponent<Canvas>();
            mainObject.layer = 5;
            mainObject.name = "ModMenu Canvas";

            //Create Canvas
            canvas = mainObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 0.4F;

            CreateEventSystem();

            DontDestroyOnLoad(mainObject);

            selectedElement = null;
            selectedElementIndex = 0;
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
                if (selectedElementIndex == 0)
                {
                    selectedElement = elements[0];
                    selectedElement.GetButton().Select();
                }

                _pos.SetElements(totalElements);

                foreach (MenuElement element in elements)
                {
                    Rect rect = new Rect(_pos.NextControlRect(true));

                    Button button = element.GetButton();

                    RectTransform buttonTrans = button.GetComponent<RectTransform>();
                    RectTransform textTrans = element.GetTextInstance().GetComponent<RectTransform>();

                    SetRectTransformValues(rect, buttonTrans);
                    SetRectTransformValues(new Rect(17, 0, rect.width - 10, rect.height), textTrans);

                 /*   if (element.HasIcon(MenuIcon.Left))
                        SetRectTransformValues(_pos.GetIconPos(MenuIcon.Left), element.leftIcon.GetComponent<RectTransform>());

                    if (element.HasIcon(MenuIcon.Right))
                        SetRectTransformValues(_pos.GetIconPos(MenuIcon.Right), element.rightIcon.GetComponent<RectTransform>());

                    if (element.HasIcon(MenuIcon.Right2))
                        SetRectTransformValues(_pos.GetIconPos(MenuIcon.Right2), element.rightIcon2.GetComponent<RectTransform>());*/
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
            if (draw)
            {
                selectedElement.GetButton().Select();
            }
            drawGUI = draw;
        }

        void ChangePos(Vector2 newPos)
        {
            _pos.ChangePos(newPos.x, newPos.y);
        }

        public void Update()
        {
            if (selectedElement != null && !selectedElement.IsSelected()) selectedElement.GetButton().Select();
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                ChangeMode(!drawGUI);
            }

            if (IsDrawing() && isRendering())
            {
                if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.I))
                {
                    SelectPreviousButton();
                }

                if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.M))
                {
                    SelectNextButton();
                }

                /*if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.J))
                {
                    selectedElement.LeftArrow();
                }

                if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.L))
                {
                    selectedElement.RightArrow();
                }*/

                if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.K))
                {
                    selectedElement.Pressed();
                }
            }
        }

        public void SelectPreviousButton()
        {
            selectedElementIndex--;
            if (selectedElementIndex == -1)
            {
                selectedElementIndex = elements.Count - 1;
                selectedElement = elements[selectedElementIndex];
            }
            else
                selectedElement = elements[selectedElementIndex];

            selectedElement.GetButton().Select();
            selectedElement.Select(false);
        }

        public void SelectNextButton()
        {
            selectedElementIndex++;
            if (selectedElementIndex == elements.Count)
            {
                selectedElementIndex = 0;
                selectedElement = elements[0];
            }
            else
                selectedElement = elements[selectedElementIndex];

            selectedElement.GetButton().Select();
            selectedElement.Select(true);
        }
    }
}
