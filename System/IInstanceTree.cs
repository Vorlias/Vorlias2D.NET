﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VorliasEngine2D.Entities;

namespace VorliasEngine2D.System
{
    /// <summary>
    /// Interface for instance containers
    /// </summary>
    public interface IInstanceTree
    {
        Entity FindFirstChild(string name);
        Entity[] GetChildren();
        void AddEntity(Entity child);
        Entity SpawnEntity();
    }
}
