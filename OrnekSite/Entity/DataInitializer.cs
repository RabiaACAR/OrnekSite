using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace OrnekSite.Entity
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            var kategoriler = new List<Category>()
            {
                new Category(){Name="Roman", Description="Roman Kitapları"},
                new Category(){Name="Ders", Description="Ders Kitapları"},
                new Category(){Name="Masal", Description="Masal Kitapları"}
            };
            foreach (var kategori in kategoriler)
            {
                context.Categoris.Add(kategori);
            }
            context.SaveChanges();
            var urunler = new List<Product>()
            {
                new Product(){Name="Ahmet Ümit",Description="Ahmet Ümit",UnitPrice=30,Stock=12,IsHome=true, IsApproved=true,IsFeatured=false, Slider=true, CategoryId=1, Image="i.jpg"},
               
            };

            foreach (var urun in urunler)
            {
                context.Products.Add(urun);
            }
            context.SaveChanges();
            base.Seed(context);
        }
    }
}