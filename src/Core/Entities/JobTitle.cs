﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class JobTitle : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}