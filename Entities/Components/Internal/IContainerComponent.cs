﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Entities.Components.Internal
{
    interface IContainerComponent
    {
        void ChildAdded(Entity entity);
    }
}
