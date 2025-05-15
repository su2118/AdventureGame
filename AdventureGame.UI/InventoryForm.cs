using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdventureGame.AdventureGame.Business;
using AdventureGame.AdventureGame.Model;

namespace AdventureGame.AdventureGame.UI
{
    public partial class InventoryForm: Form
    {
        private GameManager gameManager;

        private int userId;

        List<PlayerInventory> currentPlayerInventory = new List<PlayerInventory>();
        public InventoryForm(int userId)
        {
            InitializeComponent();
            gameManager = new GameManager();
            this.userId = userId;
            LoadInventory(userId);
        }

        private void InventoryForm_Load(object sender, EventArgs e)
        {
           
        }
        private void LoadInventory(int userId)
        {
            lstInventory.Items.Clear();
            currentPlayerInventory = gameManager.GetPlayerInventory(userId);
            this.userId = userId;
            foreach (var item in currentPlayerInventory)
            {
                if (item.Quantity > 0)
                {
                    lstInventory.Items.Add($"{item.ItemName} x {item.Quantity}");
                }
            }
        }

         private void btnDrop_Click(object sender, EventArgs e)
         {
            List<object>selectedItems = new List<object>(lstInventory.SelectedItems);
            List<int> droppedItemIds = new List<int>();
            foreach (object item in selectedItems)
            {
                string seletedString = item.ToString();
                PlayerInventory matchItem = currentPlayerInventory.FirstOrDefault(x => $"{x.ItemName} x {x.Quantity}" == seletedString);
                if (matchItem != null)
                {
                    droppedItemIds.Add(matchItem.InventoryID);
                }
                lstInventory.Items.Remove(item);
            }
            gameManager.UpdatePlayerInventory(userId, droppedItemIds);

            LoadInventory(userId);
         }
       
    }
}
