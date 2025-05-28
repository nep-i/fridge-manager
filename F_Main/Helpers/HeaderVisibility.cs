using F_Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F_Main.Helpers
{
    public class HeaderVisibility : IHeaderVisibility
    {
        public void VisibilityChanged(bool onOf)
        {

            var view = Shell.Current.FindByName<ShellContent>("FridgeView");
            //var element = view.Ch FindByName<FlexLayout>("FridgeEditMenu");
            //if (view != null && element != null)
            //    element.IsVisible = onOf;
        }
    }
}
