﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaServer
{
    public class GameState
    {
        public List<object> Ships;

        public GameState()
        {
            Ships = new List<object>();
        }
    }
}