using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Physarealm.Setting
{
    public abstract class AbstractSettingComponent : AbstractComponent
    {
        /// <summary>
        /// Initializes a new instance of the AbstractSettingComponent class.
        /// </summary>
        public AbstractSettingComponent(string name, string nickname, string description,
                                    Bitmap icon, string componentGuid)
            : base(name, nickname,
                description,
                "Physarealm", "Setting", icon, componentGuid)
        {
        }


    }
}