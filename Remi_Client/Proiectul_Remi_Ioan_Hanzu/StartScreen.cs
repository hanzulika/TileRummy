using System;
using System.Windows.Forms;

namespace Proiectul_Remi_Ioan_Hanzu
{
    public partial class StartScreen : Form
    {
        ActualGame actualGame;

        public StartScreen()
        {
            InitializeComponent();
            actualGame = new ActualGame(this);
        }

        public void CanStartGame(bool status)
        {
            if (btnStart.InvokeRequired)
            {
                btnStart.Invoke((MethodInvoker)(() => CanStartGame(status)));
            }
            else
            {
                btnStart.Enabled = status;
            }

        }

        private void btnConnectServer_Click(object sender, EventArgs e)
        {
            actualGame.ConnectToServer(btnConnectServer, btnStart, txtbAddress);
        }

        //Inchide jocul
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Actiuni de start joc
        private void btnStart_Click(object sender, EventArgs e)
        {
            actualGame.setName(txtbEnterName.Text);

            actualGame.Show();
            this.Hide();
        }
        //Sterge "name" de pe textbox
        private void txtbEnterName_Click(object sender, EventArgs e)
        {
            if (txtbEnterName.Text == "Name")
                txtbEnterName.Clear();
        }

        private void txtbEnterName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnStart_Click(sender, e);

                e.Handled = true;
            }
        }

        public String getName()
        {
            return txtbEnterName.Text;
        }
    }


}
