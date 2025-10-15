using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
   public class CarDetailDto:IDto
    {
        public int CarId { get; set; }
        public int ModelYear { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public string ColorName { get; set; }
        public string LocationName { get; set; }
        public int DailyPrice { get; set; }
        public DateTime? ReturnDATE { get; set; }
        public int BrandId { get; set; }
        public int ColorId { get; set; }
        public int GearId { get; set; }
        public int FuelId { get; set; }
        public bool IsRented { get; set; }
        public string FuelName { get; set; }
        public string GearName { get; set; }
        public int Deposit { get; set; }

    }
}
