
namespace Template_WatchShop.Models
{
    public class ShowObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quality { get; set; }
        public decimal Price { get; set; }
        public decimal FaceDiameter { get; set; }
        public decimal WaterProof { get; set; }
        public string FaceMaterial { get; set; }
        public string EnergyUse { get; set; }
        public decimal Size { get; set; }
        public string StringMaterial { get; set; }
        public string MadeIn { get; set; }
        public string Insurance { get; set; }
        public string Image { get; set; }
        public string MenuAlias { get; set; }

        public decimal? PriceDiscount { get; set; } 
        public decimal Percent { get; set; }

        public string Alias { get; set; }
        public string Description { get; set; }
        public int? Index { get; set; }

        public string MetaTitle { get; set; }
        public string TypeWatch { get; set; }
        public string Categary { get; set; }
        public int CategaryID { get; set; }

    }
}