﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
   public class CarImage:IEntity
    {
        public int CarImageId { get; set; }
        public int BrandId { get; set; }
        public int ColorId { get; set; }
        public string ImagePath { get; set; }
    }
}
