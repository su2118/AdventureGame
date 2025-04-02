﻿using System;
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

        private int currentPlayerId;
        public InventoryForm()
        {
            InitializeComponent();
            gameManager = new GameManager();
            LoadInventory(1);
        }

        private void InventoryForm_Load(object sender, EventArgs e)
        {
           
        }
        private void LoadInventory(int playerId)
        {
            lstInventory.Items.Clear();
            List<PlayerInventory> items = gameManager.GetPlayerInventory(playerId);
            currentPlayerId = playerId;
            foreach (var item in items)
            {
                lstInventory.Items.Add($"{item.ItemName} x {item.Quantity}");
            }
        }
    }
}
