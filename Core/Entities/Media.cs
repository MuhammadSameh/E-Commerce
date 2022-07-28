using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Media
    {
        public int Id { get; set; }
        [Required]
        public string PicUrl { get; set; }


        // Navigator Props
        [Required]
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

    }
}
