﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Contract
{
    public interface IRpcClient
    {
        Task<string> Call(string message);
    }
}
