using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Proiectul_Remi_Ioan_Hanzu
{
    internal class GameBoardManager
    {
        private ActualGame actualGame;

        private List<Panel> MyBoard = new List<Panel>();
        private List<Panel> ThrownBoard = new List<Panel>();
        private List<Label> ThrownTiles = new List<Label>();

        private Label currentTile;
        private Panel enemyPanel;
        private int thrownTilesIndex = -1;


        public GameBoardManager(ActualGame actualGame)
        {
            this.actualGame = actualGame;
        }

        //Getters and setters
        public Panel PanelNewTiles()
        {
            return actualGame.getPanelNewTiles();
        }

        public List<Panel> getPanelsFromMyBoard()
        {
            return MyBoard;
        }

        public Label getCurrentCheckedTile()
        {
            return currentTile;
        }

        public Panel getEnemyThrownPanel()
        {
            enemyPanel = ThrownBoard[thrownTilesIndex];
            return enemyPanel;
        }

        public Label getDockedLabel(Panel panel)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is Label label && label.Dock == DockStyle.Fill)
                {
                    return label;
                }
            }
            return null;
        }

        private int getPanelIndexForLabel(Label label, List<Panel> panels)
        {
            // Find the parent panel of the label
            Panel parentPanel = (Panel)label.Parent;

            // Find the index of the parent panel in the list
            int panelIndex = panels.IndexOf(parentPanel);

            return panelIndex;
        }

        public void setCurrentCheckedTile(Label label)
        {
            currentTile = label;
        }

        public void setNewTileEnable(bool enable)
        {
            getDockedLabel(actualGame.getPanelNewTiles()).Enabled = enable;
        }

        //Add events + allow drop + add panels sorted by their tab index to lists
        public void PanelsFromMyBoard()
        {
            TableLayoutPanel tableLayoutMyBoard = actualGame.getTableMyBoard();

            foreach (Control control in tableLayoutMyBoard.Controls)
            {
                if (control is Panel panel)
                {
                    panel.AllowDrop = true;
                    panel.DragEnter += board_DragEnter;
                    panel.DragLeave += board_DragLeave;
                    panel.DragDrop += board_DragDrop;

                    MyBoard.Add(panel);
                    MyBoard.Sort((panel1, panel2) => panel1.TabIndex.CompareTo(panel2.TabIndex));
                }
            }
        }

        public void PanelsFromThrownBoard()
        {
            TableLayoutPanel tableLayoutThrownTiles = actualGame.getTableThrownTiles();
            tableLayoutThrownTiles.AllowDrop = true;
            tableLayoutThrownTiles.DragEnter += board_DragEnter;
            tableLayoutThrownTiles.DragLeave += board_DragLeave;
            tableLayoutThrownTiles.DragDrop += board_DragDrop;
            foreach (Control control in tableLayoutThrownTiles.Controls)
            {
                if (control is Panel panel)
                {
                    panel.AllowDrop = true;
                    panel.DragEnter += board_DragEnter;
                    panel.DragLeave += board_DragLeave;
                    panel.DragDrop += board_DragDrop;

                    ThrownBoard.Add(panel);
                    ThrownBoard.Sort((panel1, panel2) => panel1.TabIndex.CompareTo(panel2.TabIndex));
                }
            }
        }

        //Check if panel has an enabled label docked 
        public bool PanelHasLabel(Panel panel)
        {
            foreach (Label label in panel.Controls.OfType<Label>())
            {
                if (label.Dock == DockStyle.Fill && label.Enabled == true)
                {
                    setCurrentCheckedTile(label);
                    return true;
                }
            }
            return false;
        }

        //Pretty self-explanatory
        public void IncrementThrownTilesIndex()
        {
            thrownTilesIndex++;
            ClearThrownTiles();
        }

        //Updates the thrown board with the enemy thrown tile
        public void AddEnemyThrownTiles()
        {
            if (enemyPanel.Controls.Count == 1 && enemyPanel.Controls[0] is Label label)
            {
                ThrownTiles.Add(label);
            }
        }

        //EVENTS
        private void board_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Label)))
            {
                e.Effect = DragDropEffects.Copy;

                if (sender is Panel panel)
                {
                    if (panel.BorderStyle != BorderStyle.Fixed3D)
                    {
                        if (ThrownBoard.Contains(panel))
                        {
                            actualGame.getTableThrownTiles().BorderStyle = BorderStyle.FixedSingle;
                        }
                        else
                        {
                            panel.BorderStyle = BorderStyle.FixedSingle;
                        }
                    }
                }
                else if (sender is TableLayoutPanel tableLayoutPanel)
                {
                    tableLayoutPanel.BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        private void board_DragLeave(object sender, EventArgs e)
        {
            if (sender is Panel panel)
            {
                if (panel.BorderStyle != BorderStyle.Fixed3D)
                {
                    if (ThrownBoard.Contains(panel))
                    {
                        actualGame.getTableThrownTiles().BorderStyle = BorderStyle.None;
                    }
                    else
                    {
                        panel.BorderStyle = BorderStyle.None;
                    }
                }
            }
            if (sender is TableLayoutPanel tableLayoutPanel)
            {
                tableLayoutPanel.BorderStyle = BorderStyle.None;
            }
        }

        private void board_DragDrop(object sender, DragEventArgs e)
        {
            Label label = e.Data.GetData(typeof(Label)) as Label;
            if (e.Data.GetDataPresent(typeof(Label)))
            {
                Panel destPanel = sender as Panel;
                TableLayoutPanel destTable = sender as TableLayoutPanel;
                //Not allowed to drop if there's tile on the destPanel
                if (!PanelHasLabel(destPanel))
                {
                    if (!ThrownTiles.Contains(label))
                    {
                        if (ThrownBoard.Contains(destPanel) || destTable == actualGame.getTableThrownTiles())
                        {
                            //Throw a tile
                            if (actualGame.checkMyTurn() == true && !PanelHasLabel(actualGame.getPanelNewTiles()))
                            {
                                thrownTilesIndex++;
                                ClearThrownTiles();
                                DockTheLabel(label, ThrownBoard[thrownTilesIndex]);

                                actualGame.setColorOfTile(label.ForeColor);
                                actualGame.setValueOfTile(label.Text);
                                actualGame.LabelDroppedInPanel();
                                actualGame.SendAndSetTurn();
                                ThrownTiles.Add(label);
                                actualGame.getTableThrownTiles().BorderStyle = BorderStyle.None;
                            }
                            else if (PanelHasLabel(actualGame.getPanelNewTiles()))
                            {
                                MessageBox.Show("You need to draw a new card first");
                            }
                            else if (actualGame.checkMyTurn() == false)
                            {
                                MessageBox.Show("It's not your turn");
                            }
                        }
                        else
                        {
                            //Move the tiles on MyBoard
                            DockTheLabel(label, destPanel);

                            actualGame.LabelDroppedInPanel();
                        }
                    }
                    else if (actualGame.checkMyTurn() == true && PanelHasLabel(PanelNewTiles()))
                    {
                        //Drag a tile from thrown board
                        DragFromThrown(label, destPanel);
                        destPanel.BorderStyle = BorderStyle.None;
                    }
                }
                else
                {
                    destPanel.BorderStyle = BorderStyle.None;
                }
            }
        }

        //Requirements to drag a tile from the thrown board
        public void DragFromThrown(Label label, Panel panel)
        {
            //Can't drag the first thrown tile
            if (ThrownTiles[0] != label)
            {
                //In case you drag the last tile from the thrown board
                if (label == ThrownTiles.Last())
                {
                    int initialScore = actualGame.onBoardScore();
                    DockTheLabel(label, panel);
                    actualGame.tileCheck();
                    //No meld happened
                    if (initialScore < actualGame.onBoardScore() && actualGame.getFirstMeld() == false)
                    {
                        if (actualGame.onBoardScore() > 44)
                        {
                            actualGame.Meld();
                            actualGame.setFirstMeld(true);
                            actualGame.LabelDroppedInPanel();
                            thrownTilesIndex--;
                            actualGame.setMinusIndex(1);
                            ThrownTiles.Remove(ThrownTiles.Last());
                            setNewTileEnable(false);
                        }
                        else
                        {
                            Console.WriteLine("You need " + (45 - actualGame.onBoardScore()) + " more points to meld");
                        }
                    }
                    //At least one meld happened
                    else if (initialScore < actualGame.onBoardScore() && actualGame.getFirstMeld() == true)
                    {
                        actualGame.LabelDroppedInPanel();
                        thrownTilesIndex--;
                        actualGame.setMinusIndex(1);
                        ThrownTiles.Remove(ThrownTiles.Last());
                        setNewTileEnable(false);
                    }
                    else
                    {
                        DockTheLabel(label, ThrownBoard[thrownTilesIndex]);
                        Console.WriteLine("You did not create a formation");
                    }
                }
                //In case you drag any other tile from the ThrownTiles beside the last tile
                else
                {
                    //Goes through ThrownBoard
                    int index = getPanelIndexForLabel(label, ThrownBoard);
                    int initialScore = actualGame.onBoardScore();
                    DockTheLabel(label, panel);
                    actualGame.tileCheck();
                    if (initialScore < actualGame.onBoardScore() && actualGame.getFirstMeld() == true)
                    {
                        ThrownTiles.RemoveAt(index);
                        //Goes through ThrownTiles
                        int initialIndex = index;
                        actualGame.LabelDroppedInPanel();
                        index++;
                        Panel nextPanel = ThrownBoard[index];
                        while (PanelHasLabel(nextPanel))
                        {
                            ThrownTiles.RemoveAt(initialIndex);
                            DockTheLabel(getDockedLabel(nextPanel), actualGame.getLastBoardPanel());
                            index++;
                            nextPanel = ThrownBoard[index];
                        }
                        thrownTilesIndex = initialIndex - 1;
                        actualGame.setMinusIndex(initialIndex);
                        setNewTileEnable(false);
                    }
                    else
                    {
                        DockTheLabel(label, ThrownBoard[index]);
                    }
                }
                actualGame.tileCheck();
            }
        }

        //Removes all tile from ThrownBoard in case it's full
        public void ClearThrownTiles()
        {
            if (thrownTilesIndex > 14)
            {
                ThrownTiles.Clear();
                for (int i = 0; i < thrownTilesIndex; i++)
                {
                    ThrownBoard[i].Controls.Clear();
                }
                thrownTilesIndex = -1;
            }
        }

        //Removes the tiles drag by the enemy from the ThrownBoard
        public void removeThrownTiles(int minusIndex)
        {
            if (minusIndex == 1)
            {
                ThrownBoard[ThrownTiles.Count - 1].Controls.Clear();
                ThrownTiles.Remove(ThrownTiles.Last());
                thrownTilesIndex--;
            }
            else
            {
                for (int i = minusIndex; i <= thrownTilesIndex; i++)
                {
                    ThrownBoard[i].Controls.Clear();
                    ThrownTiles.RemoveAt(minusIndex);
                }
                thrownTilesIndex = minusIndex - 1;
            }
        }

        //Fills the specified panel with the specified label
        private void DockTheLabel(Label label, Panel panel)
        {
            label.Parent = panel;
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleCenter;
        }
    }
}





