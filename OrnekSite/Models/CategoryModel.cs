﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// Bir kategoride kaç tane ürün olduğunu bulmak için oluşturuldu.

namespace OrnekSite.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }


    }
}
