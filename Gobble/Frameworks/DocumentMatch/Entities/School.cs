﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constraints;

namespace DocumentMatch.Entities
{
    public class School : Entity
    {
        public string EmailPattern { get; set; }
        public Cities City { get; set; }
    }
}
