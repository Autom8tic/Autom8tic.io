using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Dashboard1
{
    [ComVisible(true)]
    public class ScriptHandler
    {
        private Popup popup;
        public ScriptHandler(Popup popup)
        {
            this.popup = popup;
        }

        public void Close()
        {
            this.popup.IsOpen = false;
        }
    }
}
