﻿using BtcKpi.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtcKpi.Data.Configuration
{
    public class GadgetConfiguration: EntityTypeConfiguration<Gadget>
    {
        public GadgetConfiguration()
        {
            ToTable("store.Gadgets");
            Property(g => g.Name).IsRequired().HasMaxLength(50);
            Property(g => g.Price).IsRequired().HasPrecision(8, 2);
            Property(g => g.CategoryID).IsRequired();
        }
    }
}
