using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
namespace Proiectul_Remi_Ioan_Hanzu
{
    public partial class ActualGame : Form
    {
        private StartScreen startScreen;
        private GameBoardManager gameBoardManager;
        private GameRules gameRules;
        private int MyScore;
        private bool firstMeld = false;
        private Color tileColor;
        private string tileValue;

        private TcpClient client;
        private NetworkStream streamClient;
        private Thread clientThread;
        private bool workThread;
        private string disposeEnemyName="Other Player";

        public ActualGame(StartScreen startScreen)
        {
            this.startScreen = startScreen;
            InitializeComponent();
            InitializeBoard();
            InitializeGame();
        }

        private void InitializeBoard()
        {
            gameBoardManager = new GameBoardManager(this);
            gameBoardManager.PanelsFromMyBoard();
            gameBoardManager.PanelsFromThrownBoard();
        }

        private void InitializeGame()
        {
            gameRules = new GameRules(gameBoardManager);
            gameRules.StartGame();
            gameRules.TileChecker();
        }

        //Getters and setters
        public bool getFirstMeld()
        {
            return firstMeld;
        }

        public Panel getLastBoardPanel()
        {
            return panelTables24;
        }

        public Panel getPanelNewTiles()
        {
            return panelNewTiles;
        }

        public TableLayoutPanel getTableThrownTiles()
        {
            return tableLayoutThrownTiles;
        }
        public TableLayoutPanel getTableMyBoard()
        {
            return tableLayoutMyBoard;
        }

        public bool checkMyTurn()
        {
            return gameRules.getMyTurn();
        }

        public void setFirstMeld(bool firstMeld)
        {
            this.firstMeld = firstMeld;
        }

        public void setName(String s)
        {
            if (startScreen.getName() != "Name" && startScreen.getName() != "")
            {
                lblPlayerName.Text = s;
            }

            StreamWriter streamWriter = new StreamWriter(streamClient);
            streamWriter.AutoFlush = true;
            streamWriter.WriteLine("enemyName" + lblPlayerName.Text);
        }

        //Methods from GameRules
        public void tileCheck()
        {
            gameRules.TileChecker();
        }

        public int onBoardScore()
        {
            return gameRules.getOnBoardScore();
        }

        //Closes the app,network stream and client thread when ActualGame form closes
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.UserClosing)
            {
                streamClient.Close();
                clientThread.Abort();
                Application.Exit();
            }
        }

        //Calls Meld and sends MyScore to the other player 
        private void btMeld_Click(object sender, EventArgs e)
        {
            if (gameRules.getMyTurn() == true)
            {
                Meld();
                try
                {
                    StreamWriter streamWriter = new StreamWriter(streamClient);
                    streamWriter.AutoFlush = true;
                    streamWriter.WriteLine(lblScore.Text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Can't meld because: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("It's not your turn");
            }
        }

        //OnBoardScore becomes MyScore and 
        //the formations are removed from MyBoard
        public void Meld()
        {           
            {
                gameRules.TileChecker();
                if (gameRules.getOnBoardScore() > 44 || firstMeld == true)
                {
                    gameRules.setButtonPressed(true);
                    gameRules.TileChecker();

                    MyScore = MyScore + gameRules.getOnBoardScore();
                    lblScore.Text = MyScore.ToString();

                    gameRules.PanelBorderReset();
                    firstMeld = true;
                }
                else
                {
                    MessageBox.Show("You need "+ (45-gameRules.getOnBoardScore())+" more points to meld");
                }
                gameRules.setButtonPressed(false);
            }
        }


        //Methods that happen everytime a label is dropped in a panel
        public void LabelDroppedInPanel()
        {
            //Makes sure formations are not removed
            gameRules.setButtonPressed(false);
            gameRules.TileChecker();
        }

        //Shows who's turn it is on the screen
        public void WhosTurnItIs()
        {
            if(checkMyTurn()==false)
            {
                lblWhosTurn.Text = "It's " + enemyName.Text + "'s turn";
            }
            else
            {
                lblWhosTurn.Text = "It's your turn";
            }
        }

        //Client

        //Starts the client when you click Connect
        public void ConnectToServer(Button btnConnect, Button btnStart, TextBox txtbAddress)
        {
            if (btnConnect.Text == "Connect")
            {
                if (txtbAddress.Text.Length > 0)
                {
                    client = new TcpClient(txtbAddress.Text, 8888);
                    workThread = true;
                    clientThread = new Thread(new ThreadStart(Client));
                    clientThread.Start();
                    streamClient = client.GetStream();
                    btnStart.Enabled = true;
                    if (client.Connected)
                    {
                        Console.WriteLine("Connected");
                    }
                }
                else
                {
                    MessageBox.Show("Write the IP Address");
                }
            }
            else
            {
                workThread = false;
                clientThread.Abort();
                streamClient.Close();
            }
        }


        public void Client()
        {
             StreamReader clientRead = new StreamReader(streamClient);
             while (workThread)
             {
                string line = clientRead.ReadLine();
                if (line == null)
                {
                    break;
                }

                if (line.Contains("enemyName"))
                {
                    disposeEnemyName = line.Substring(9);
                    if (startScreen.Visible == false)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            ChangeEnemyName();
                        });
                    }
                }
                else if (line == "Turn")
                {
                    Invoke((MethodInvoker)delegate
                    {
                        gameRules.setMyTurn(true);
                        WhosTurnItIs();
                    });
                }
                else if (line == "thrownTilesIndex")
                {
                    Invoke((MethodInvoker)delegate
                    {
                        gameBoardManager.IncrementThrownTilesIndex();
                    });
                }
                else if (line.Contains("Blue") || line.Contains("Red") || line.Contains("Goldenrod") || line.Contains("Black"))
                {
                    Invoke((MethodInvoker)delegate
                    {
                        gameRules.setColor(line);
                    });
                }
                else if (line[0] == 'v')
                {
                    string disposableLine = line.Substring(1);
                    Invoke((MethodInvoker)delegate
                    {
                        gameRules.EnemyThrown(disposableLine);
                        gameBoardManager.AddEnemyThrownTiles();
                    });
                }
                else if (line[0] == 'i')
                {
                    string disposableLine = line.Substring(1);
                    Invoke((MethodInvoker)delegate
                    {
                        gameBoardManager.removeThrownTiles(int.Parse(disposableLine));
                    });
                }
                else if (line == "win")
                {
                    lblScore.Text = (int.Parse(lblScore.Text) + 100).ToString();
                    MessageBox.Show("YOU WON WITH: " + lblScore.Text);
                }
                else if (line == "lose")
                {
                    MessageBox.Show("YOU LOST WITH: " + lblScore.Text);
                }
                else
                {
                    Invoke((MethodInvoker)delegate
                    {
                        lblScoreEnemy.Text = line;
                    });
                }
             }            
        }

        //Changes turns and sends the updated thrown tiles board
        public void SendAndSetTurn()
        {
            gameRules.setMyTurn(false);
            WhosTurnItIs();
            CheckIfWon();
            try
            {
                StreamWriter streamWriter = new StreamWriter(streamClient);
                streamWriter.AutoFlush = true;
                streamWriter.WriteLine("Turn");
                streamWriter.WriteLine("thrownTilesIndex");
                streamWriter.WriteLine(tileColor);
                streamWriter.WriteLine("v" + tileValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't send turn " + ex.Message);
            }
        }

        //If there is less than 2 tiles left on MyBoard after
        //throwing a tile, it send every player their winning status
        private void CheckIfWon()
        {
            int tilesOnMyBoard = 0;
            gameRules.TileChecker();
            foreach (Panel panel in gameBoardManager.getPanelsFromMyBoard())
            {
                if (gameBoardManager.PanelHasLabel(panel) == true)
                {
                    tilesOnMyBoard++;
                }
            }
            if (tilesOnMyBoard <2)
            {
                lblScore.Text = (int.Parse(lblScore.Text) + 100).ToString();
                if (int.Parse(lblScore.Text) > int.Parse(lblScoreEnemy.Text))
                {
                    StreamWriter streamWriter = new StreamWriter(streamClient);
                    streamWriter.AutoFlush = true;
                    streamWriter.WriteLine("lose");
                    MessageBox.Show("YOU WON WITH: " + lblScore.Text);
                }
                else
                {
                    StreamWriter streamWriteam = new StreamWriter(streamClient);
                    streamWriteam.AutoFlush = true;
                    streamWriteam.WriteLine("win");
                    MessageBox.Show("YOU LOST WITH: " + lblScore.Text);
                }
            }
        }

        //Color,value and index on the thrown table
        //of the enemy thrown tile
        internal void setColorOfTile(Color foreColor)
        {
            tileColor = foreColor;
        }

        internal void setValueOfTile(string tileValue)
        {
            this.tileValue = tileValue;
        }

        public void setMinusIndex(int minusIndex)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(streamClient);
                streamWriter.AutoFlush = true;  
                streamWriter.WriteLine("i" + minusIndex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't send minus index " + ex.Message);
            }
        }

        //Sets the enemy name if they changed it from the default
        private void ChangeEnemyName()
        {
            if (disposeEnemyName != "Player" && enemyName.Text == "Other Player")
            {
                enemyName.Text = disposeEnemyName;
            }
        }

        private void ActualGame_Load(object sender, EventArgs e)
        {
            ChangeEnemyName();
            WhosTurnItIs();
        }
    }
}
