using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame.AdventureGame.Model
{
    public class PlayerInventory
    {
        public int UserID { get; set; }

        public int InventoryID { get; set; }

        public string ItemName { get; set; }
        public int Quantity { get; set; }

        public string Dispaly => $"{ItemName} x {Quantity}";
    }
}
