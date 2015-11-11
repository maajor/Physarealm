using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.Kernel.Types;
using Physarealm;

namespace Physarealm.Setting
{
    abstract class AbstractSettingType:GH_Goo<object>
    {
        public override bool IsValid
        {
            get { return true; }
        }
        public abstract void setParameter(Physarum p);

        public override string ToString()
        {
            return this.TypeName;
        }

        public override string TypeDescription
        {
            get { return "AbstractSettingType Description"; }
        }

        public override string TypeName
        {
            get { return "AbstractSettingType"; }
        }
    }
}
