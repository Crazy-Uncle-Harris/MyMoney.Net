﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using Walkabout.Tests.Interop;
using System.Threading;

namespace Walkabout.Tests.Wrappers
{
    public class CategoryPropertiesWrapper : DialogWrapper
    {
        internal CategoryPropertiesWrapper(AutomationElement e)
            : base(e)
        {
        }

        public void ClickOk()
        {
            this.window.ClickButton("okButton");
        }

        public void ClickCancel()
        {
            this.window.ClickButton("cancelButton");
        }


    }
}
