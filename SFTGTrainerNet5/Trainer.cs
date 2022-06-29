using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SFTGTrainer.ModMenu;
using SFTGTrainer.ModMenu.Elements;

namespace SFTGTrainer
{
    class Trainer : MonoBehaviour
    {
        private Menu menu;

        public IEnumerator Init()
        {
            yield return new WaitForSeconds(5);

            if (menu == null) menu = Menu.Instance;
            menu.StartRendering(10, 10, 500, 1, 280, 40, 20, 18);

            MenuButton testbtn = menu.CreateButton("Test Button");
            testbtn.Pressed = Test;

            MenuToggleableButton toggletest = menu.CreateToogleableButton("Toggle Button");

            menu.RegisterElement(testbtn);
            menu.RegisterElement(toggletest);

            menu.ChangeMode(true);

            foreach (MenuElement element in menu.elements)
            {
                ColorBlock c = element.GetButton().colors;

                Color n = c.normalColor;

                c.normalColor = new Color(n.r, n.g, n.b, 0.6F);
                c.pressedColor = new Color(n.r, n.g, n.b, 0.6F);
                element.GetButton().colors = c;

                element.GetTextInstance().color = Color.white;
            }

            yield return null;
        }

        public void Start()
        {
            menu = Menu.CreateModMenuObject();
            StartCoroutine(nameof(Init));
        }

        void Test()
        {
            LogUtils.info("test !");
        }
    }
}
