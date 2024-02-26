using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Proiectul_Remi_Ioan_Hanzu
{
    internal class Tile
    {
        private Color[] tileColors = new Color[] { Color.Blue, Color.Red, Color.Goldenrod, Color.Black };
        private Random randNr = new Random();
        private List<Panel> occupiedPanels = new List<Panel>();

        int tileColor;
        int tileValue = 0;

        bool SearchAgain;
        bool allZeros1 = false;
        bool allZeros2 = false;
        bool allZeros3 = false;
        bool allZeros4 = false;
        List<int> allNumbers1 = new List<int>(Enumerable.Range(1, 13).ToList());
        List<int> allNumbers2 = new List<int>(Enumerable.Range(1, 13).ToList());
        List<int> allNumbers3 = new List<int>(Enumerable.Range(1, 13).ToList());
        List<int> allNumbers4 = new List<int>(Enumerable.Range(1, 13).ToList());

        //Returns an unoccupied panel from the specified list and
        //populates the occupiedPanels with that panel
        private Panel getRandomPanelFromList(List<Panel> panels)
        {
            if (panels != null && panels.Count > 0)
            {
                int randomIndex = randNr.Next(panels.Count);
                while (occupiedPanels.Contains(panels[randomIndex]))
                {
                    randomIndex = randNr.Next(panels.Count);
                }
                occupiedPanels.Add(panels[randomIndex]);
                return panels[randomIndex];
            }
            else
            {
                return null;
            }
        }

        //Generates a random tile or a specific tile,
        //updates the list of remaining tiles and
        //sends the tile to be placed in a specific
        //or random location
        public void TileGenerator(int TileNr, Panel singlePanel = null, List<Panel> panels = null, int TileColor = -1, int TileValue = 0)
        {
            //Generate a specific tile without updating the list of remaining tiles
            if (TileNr == 1 && TileColor != -1 && TileValue != 0)
            {
                panels = new List<Panel>
                {
                    singlePanel
                };
                TileProperties(panels, TileColor, TileValue);
            }
            //Generate a random tile and update the lists of remaining tiles
            else
            {
                //If only a panel is specified
                if (panels == null)
                {
                    panels = new List<Panel>()
                    {
                        singlePanel
                    };
                }
                for (int i = 1; i <= TileNr; i++)
                {
                    allZeros1 = allNumbers1.Distinct().Count() == 1;
                    allZeros2 = allNumbers2.Distinct().Count() == 1;
                    allZeros3 = allNumbers3.Distinct().Count() == 1;
                    allZeros4 = allNumbers4.Distinct().Count() == 1;
                    do
                    {
                        SearchAgain = false;
                        int index;
                        tileColor = randNr.Next(0, tileColors.Length);
                        switch (tileColor)
                        {
                            case 0:
                                //Generates another tile color if the list is full of zeros
                                if (allZeros1 == true)
                                {
                                    SearchAgain = true;
                                    break;
                                }
                                SearchAgain = false;
                                //Generates a random number + assign the number to the tile value +
                                //removes the number from the list and adds a 0 in its place
                                do
                                {
                                    index = randNr.Next(allNumbers1.Count);
                                    tileValue = allNumbers1[index];
                                    if (tileValue != 0)
                                    {
                                        allNumbers1.RemoveAt(index);
                                        allNumbers1.Insert(index, 0);
                                    }
                                } while (tileValue == 0);
                                break;

                            case 1:
                                if (allZeros2 == true)
                                {
                                    SearchAgain = true;
                                    break;
                                }
                                SearchAgain = false;
                                do
                                {
                                    index = randNr.Next(allNumbers2.Count);
                                    tileValue = allNumbers2[index];
                                    if (tileValue != 0)
                                    {
                                        allNumbers2.RemoveAt(index);
                                        allNumbers2.Insert(index, 0);
                                    }
                                } while (tileValue == 0);
                                break;

                            case 2:
                                if (allZeros3 == true)
                                {
                                    SearchAgain = true;
                                    break;
                                }
                                SearchAgain = false;
                                do
                                {
                                    index = randNr.Next(allNumbers3.Count);
                                    tileValue = allNumbers3[index];
                                    if (tileValue != 0)
                                    {
                                        allNumbers3.RemoveAt(index);
                                        allNumbers3.Insert(index, 0);
                                    }
                                } while (tileValue == 0);
                                break;

                            case 3:
                                if (allZeros4 == true)
                                {
                                    SearchAgain = true;
                                    break;
                                }
                                SearchAgain = false;
                                do
                                {
                                    index = randNr.Next(allNumbers4.Count);
                                    tileValue = allNumbers4[index];
                                    if (tileValue != 0)
                                    {
                                        allNumbers4.RemoveAt(index);
                                        allNumbers4.Insert(index, 0);
                                    }
                                } while (tileValue == 0);
                                break;
                        }

                    } while (SearchAgain == true);
                    //Sends the generated tiles to be added to their specific location
                    TileProperties(panels, tileColor, tileValue);
                }
            }
        }

        //Assigns the same properties to the tiles + add event +
        //add the tile to the specfied list of panels
        public void TileProperties(List<Panel> panels, int tileColor, int tileValue)
        {
            Label tile = new Label();

            //Adds properties
            tile.BorderStyle = BorderStyle.FixedSingle;
            tile.Text = tileValue.ToString();
            tile.Size = new Size(25, 43);
            tile.Font = new Font("Roboto", 7, FontStyle.Bold);
            tile.BackColor = Color.White;
            tile.ForeColor = tileColors[tileColor];
            tile.Cursor = Cursors.Hand;
            tile.TextAlign = ContentAlignment.MiddleCenter;
            tile.UseCompatibleTextRendering = true;

            //Adds events
            tile.Paint += Label_Paint;
            tile.MouseDown += labelTile_MouseDown;

            //Place the tile to a random panel from the list
            //if there are more than one element in the list
            if (panels.Count > 1)
            {
                tile.Parent = getRandomPanelFromList(panels);
            }
            //Place the tile to the only panel in the li
            else
            {
                tile.Parent = panels[0];
            }
            tile.Dock = DockStyle.Fill;
        }

        //Events
        private void labelTile_MouseDown(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.DoDragDrop(label, DragDropEffects.Copy);
        }

        private void Label_Paint(object sender, PaintEventArgs e)
        {
            Label label = (Label)sender;
            ControlPaint.DrawBorder(e.Graphics, label.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }
    }
}
