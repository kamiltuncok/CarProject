using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
   public class Car:IEntity
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int ColorId { get; set; }
        public int LocationId { get; set; }
        public int ModelYear { get; set; }
        public int DailyPrice { get; set; }
        public string Description { get; set; }
        public bool IsRented { get; set; }
        public int FuelId { get; set; }
        public int GearId { get; set; }
        public int SegmentId { get; set; }
        public int Deposit { get; set; }
    }
}
