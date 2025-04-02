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

namespace AdventureGame.AdventureGame.UI
{
    public partial class PlayerLogin: Form
    {
        private UserManager userManager;

        private GameManager gameManager;
        public PlayerLogin()
        {
            InitializeComponent();

            userManager = new UserManager();
            gameManager = new GameManager();

        }
        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if(txtUsername.Text == "Username")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Password")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            int userId = userManager.LoginUser(username, password);

            if (string.IsNullOrEmpty(username) || username=="Username" || string.IsNullOrEmpty(password) || password == "Password")
            {
                MessageBox.Show("Username and Password cannot be empty!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (userId != -1)
            {
                MessageBox.Show("Login Successful");
                this.Hide();
                gameManager.SetUser(userId);
                GameForm gameForm = new GameForm(userId);
                gameForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Your username or password is wrong. Try Again");
            }
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            bool success = userManager.RegisterUser(username, password);

            if (string.IsNullOrEmpty(username) || username == "Username" || string.IsNullOrEmpty(password) || password == "Password")
            {
                MessageBox.Show("Username and Password cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (success)
            {
                MessageBox.Show("Account created successfully! Please Log in");
                this.Close(); //close the form 
            }
            else
            {
                MessageBox.Show("Username already taken. Please try another one");
            }

        }
    }
}
