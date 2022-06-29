using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFTGTrainer.ModMenu.Elements
{
    public class MenuButton : MenuElement
    {
        public override MenuButtonType GetElementType()
        {
            return MenuButtonType.Normal;
        }
    }
}
