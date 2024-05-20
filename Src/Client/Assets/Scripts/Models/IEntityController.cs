﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public interface IEntityController
    {
        void PlayAnim(string name);
        void SetStandby(bool standby);
    }
}
