using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Proiectul_Remi_Ioan_Hanzu
{
    internal class GameRules : Tile
    {
        private GameBoardManager boardManager;

        List<Panel> panelsFromMyBoard = new List<Panel>();
        List<int> runList = new List<int>();
        List<int> setList = new List<int>();
        List<Panel> runListPanels = new List<Panel>();
        List<Panel> setListPanels = new List<Panel>();

        int CurrentTile = 0;
        int OnBoardScore = 0;
        int i = 0;
        int j = 0;

        bool buttonPressed = false;
        bool MyTurn = true;
        int color;

        public GameRules(GameBoardManager boardManager)
        {
            this.boardManager = boardManager;
            panelsFromMyBoard = boardManager.getPanelsFromMyBoard();
        }

        //Getters and setters
        public int getOnBoardScore()
        {
            return OnBoardScore;
        }

        public bool getMyTurn()
        {
            return MyTurn;
        }

        public void setButtonPressed(bool status)
        {
            buttonPressed = status;
        }

        public void setOnBoardScore(int OnBoardScore)
        {
            this.OnBoardScore = OnBoardScore;
        }

        public void setMyTurn(bool turn)
        {
            MyTurn = turn;
            Console.WriteLine(MyTurn);
            if (turn == true)
            {
                if (boardManager.getDockedLabel(boardManager.PanelNewTiles()) != null)
                {
                    if (boardManager.getDockedLabel(boardManager.PanelNewTiles()).Enabled == false)
                    {
                        boardManager.setNewTileEnable(true);
                    }
                }
                else
                {
                    GetNewTiles();
                }

            }
        }

        public void setColor(string line)
        {
            if (line.Contains("Blue"))
            {
                color = 0;
            }
            else if (line.Contains("Red"))
            {
                color = 1;
            }
            else if (line.Contains("Goldenrod"))
            {
                color = 2;
            }
            else
            {
                color = 3;
            }
        }

        //Checks every panel from MyBoard and whenever there's a tile it
        //calls CheckRun and CheckSet
        public void TileChecker()
        {
            OnBoardScore = 0;
            PanelBorderReset();
            foreach (Panel panel in panelsFromMyBoard)
            {
                Console.WriteLine(panelsFromMyBoard.IndexOf(panel) + " " + panel.Name);
                if (boardManager.PanelHasLabel(panel) == true)
                {
                    CurrentTile = int.Parse(boardManager.getCurrentCheckedTile().Text);
                    Console.WriteLine("TileNumber: " + CurrentTile);
                    CheckRun(CurrentTile, panel);
                    CheckSet(CurrentTile, panel);
                }
                else
                {
                    if (runList.Count > 2)
                    {
                        OnBoardScoreCalculator(runList);
                        PanelsWithFormations(runListPanels, buttonPressed);
                    }
                    if (setList.Count > 2)
                    {
                        OnBoardScoreCalculator(setList);
                        PanelsWithFormations(setListPanels, buttonPressed);
                    }
                    runList.Clear();
                    runListPanels.Clear();
                    setList.Clear();
                    setListPanels.Clear();
                    i = 0;
                    j = 0;
                }

            }
        }

        //Checks if 2 or more tiles make a formation
        private void CheckRun(int currentTile, Panel currentPanel)
        {
            runList.Add(currentTile);
            runListPanels.Add(currentPanel);
            if (runList.Count > 1)
            {
                //Checks if the last two added tile values are ascending
                if (runList[j + 1] - runList[j] == 1 || (runList[j + 1] == 1 && runList[j] == 13))
                {
                    setList.Clear();
                    setListPanels.Clear();
                    i = 0;
                    if (runList[j + 1] == 1 && runList[j] == 13)
                    {
                        runList[j + 1] = 14;
                    }
                    j++;
                    Console.WriteLine("RunList: " + runList.Count);
                }
                //Values not ascending
                else
                {
                    //Removes the first tile from the list
                    if (runList.Count == 2)
                    {
                        runList.RemoveAt(0);
                        runListPanels.RemoveAt(0);
                    }
                    //Keeps only the last tile in the list and clears both lists
                    else if (runList.Count == 3)
                    {
                        Panel remainingPanel = runListPanels[runListPanels.Count - 1];
                        int remainingTile = runList[runList.Count - 1];
                        clearAndAddLists(remainingTile, remainingPanel);
                    }
                    //Same as last one + removes the last tile + adds point to OnBoardScore +
                    //if buttonPressed==true, removes the tiles from the board
                    else
                    {
                        Panel remainingPanel = runListPanels[runListPanels.Count - 1];
                        int remainingTile = runList[runList.Count - 1];
                        runListPanels.RemoveAt(runListPanels.Count - 1);
                        runList.RemoveAt(runList.Count - 1);
                        OnBoardScoreCalculator(runList);
                        PanelsWithFormations(runListPanels, buttonPressed);
                        clearAndAddLists(remainingTile, remainingPanel);
                    }
                }
            }

        }

        //Checks if 2 or more tiles make a formation
        private void CheckSet(int currentTile, Panel currentPanel)
        {
            setList.Add(currentTile);
            setListPanels.Add(currentPanel);
            if (setList.Count > 1)
            {
                //Checks if the last two added tile values are equal
                if (setList[i + 1] == setList[i])
                {
                    runList.Clear();
                    runListPanels.Clear();
                    j = 0;
                    i++;
                    Console.WriteLine("SetList: " + setList.Count);
                }
                //Values not equal
                else
                {
                    //Removes the first tile from the list
                    if (setList.Count == 2)
                    {
                        setList.RemoveAt(0);
                        setListPanels.RemoveAt(0);
                    }
                    //Keeps only the last tile in the list and clears both lists
                    else if (setList.Count == 3)
                    {
                        int remainingTile = setList[setList.Count - 1];
                        Panel remainingPanel = setListPanels[setListPanels.Count - 1];
                        clearAndAddLists(remainingTile, remainingPanel);
                    }
                    //Same as last one + removes the last tile + adds point to OnBoardScore +
                    //if buttonPressed==true, removes the tiles from the board
                    else
                    {
                        int remainingTile = setList[setList.Count - 1];
                        Panel remainingPanel = setListPanels[setListPanels.Count - 1];
                        setList.RemoveAt(setList.Count - 1);
                        setListPanels.RemoveAt(setListPanels.Count - 1);
                        OnBoardScoreCalculator(setList);
                        PanelsWithFormations(setListPanels, buttonPressed);
                        clearAndAddLists(remainingTile, remainingPanel);
                    }
                }
            }
        }

        //Clears all lists and adds the remainig tile to the runList(since it's the first one called)
        public void clearAndAddLists(int remainingTile, Panel remainingPanel)
        {
            setListPanels.Clear();
            runListPanels.Clear();
            setList.Clear();
            runList.Clear();
            j = 0;
            i = 0;
            runListPanels.Add(remainingPanel);
            runList.Add(remainingTile);
        }

        //Adds points to OnBoardScore based on the values of the number from the list received
        private void OnBoardScoreCalculator(List<int> checkedList)
        {
            if (checkedList.All(i => i == 1))
            {
                OnBoardScore = +25 * checkedList.Count;
            }
            else
            {
                for (int i = 0; i < checkedList.Count; i++)
                {
                    if (checkedList[i] < 10)
                    {
                        OnBoardScore = OnBoardScore + 5;
                    }
                    else
                    {
                        OnBoardScore = OnBoardScore + 10;
                    }
                }
            }
            setOnBoardScore(OnBoardScore);
        }

        //Removes all tiles from every panel in the specified list if the buttonPressed==true
        public void PanelsWithFormations(List<Panel> panelList, bool buttonPressed)
        {
            foreach (Panel panel in panelList)
            {
                panel.BorderStyle = BorderStyle.Fixed3D;
                if (buttonPressed == true)
                {
                    panel.Controls.Clear();
                }
            }
        }

        //Removes borders for all panels from MyBoard
        public void PanelBorderReset()
        {
            foreach (Panel panel in panelsFromMyBoard)
            {
                panel.BorderStyle = BorderStyle.None;
            }
        }

        //Places 15 tiles on random locations on the board at the start of the game
        public void StartGame()
        {
            TileGenerator(15, null, panelsFromMyBoard);
        }

        //Places 1 tile in the new tiles place?
        public void GetNewTiles()
        {
            TileGenerator(1,boardManager.PanelNewTiles());
        }

        //Places the enemy thrown tile on my thrown board
        public void EnemyThrown(string value)
        {
            TileGenerator(1, boardManager.getEnemyThrownPanel(), null, color, int.Parse(value));
        }
    }
}
