using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dooz
{
    public partial class Form1 : Form
    {
        private bool turn = false;
        Node[] nodes = null;
        private int NodeCounter = 0;
        private int nodeDiameter = 40;

        public Form1()
        {
            InitializeComponent();

            nodes = new Node[24];            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            NodeCounter = 0;    
            Graphics g = e.Graphics;
            DrawScreen(g, nodeDiameter);
        }

        private void DrawScreen(Graphics g, int diameter)
        {
            if (turn == false)
                g.FillRectangle(Brushes.Red, 350, 350, 100, 100);
            else
                g.FillRectangle(Brushes.Green, 350, 350, 100, 100);

            g.DrawRectangle(Pens.Black, 100, 100, 600, 600);
            g.DrawRectangle(Pens.Black, 150, 150, 500, 500);
            g.DrawRectangle(Pens.Black, 200, 200, 400, 400);

            Brush myBrush = Brushes.White;

            DrawCircles(g, myBrush, 400, 100, 0, 50, diameter);
            DrawCircles(g, myBrush, 400, 600, 0, 50, diameter);
            DrawCircles(g, myBrush, 100, 100, 50, 50, diameter);
            DrawCircles(g, myBrush, 700, 100, -50, 50, diameter);
            DrawCircles(g, myBrush, 100, 400, 50, 0, diameter);
            DrawCircles(g, myBrush, 700, 400, -50, 0, diameter);
            DrawCircles(g, myBrush, 100, 700, 50, -50, diameter);
            DrawCircles(g, myBrush, 700, 700, -50, -50, diameter);
        }

        private void DrawCircles(Graphics g, Brush b, int startX, int startY, int offsetX, int offsetY, int diameter)
        {
            int x = startX - diameter / 2;
            int y = startY - diameter / 2;

            for (int i = 0; i < 3; i++)
            {
                if (nodes[NodeCounter]== null)
                    nodes[NodeCounter] = new Node { Number = NodeCounter + 1, X = x, Y = y, State = 0 };

                if (nodes[NodeCounter].State == 0)
                    g.FillEllipse(b, x, y, diameter, diameter);
                else if (nodes[NodeCounter].State == 1)
                    g.FillEllipse(Brushes.Red, x, y, diameter, diameter);
                else
                {
                    if (chkComputer.Checked == false || NodeCounter!=numericUpDown1.Value-1)
                        g.FillEllipse(Brushes.Green, x, y, diameter, diameter);
                    else
                        for (int dtmp = 1; dtmp <= diameter; dtmp++)
                        {
                            g.FillEllipse(Brushes.Green, x+nodeDiameter/2-dtmp/2, y + nodeDiameter / 2 - dtmp / 2, dtmp, dtmp);
                            System.Threading.Thread.Sleep(3);
                        }
                }
                g.DrawEllipse(Pens.Black, x, y, diameter, diameter);

                string str = (NodeCounter + 1).ToString();

                var sizeString = g.MeasureString(str, SystemFonts.DefaultFont);



                g.DrawString(str, SystemFonts.DefaultFont, Brushes.Black, x + diameter / 2 - sizeString.Width / 2, y + diameter / 2 - sizeString.Height / 2);
                NodeCounter++;

                x += offsetX;
                y += offsetY;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Width = 800;
            this.Height = 800;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnPut_Click(object sender, EventArgs e)
        {
            if (nodes[(int)numericUpDown1.Value - 1].State == 0)
            {
                if (turn==false)
                    nodes[(int)numericUpDown1.Value - 1].State = 1;
                else
                    nodes[(int)numericUpDown1.Value - 1].State = 2;
                this.Invalidate();
                this.Update();

                int chk = Check();
                if (chk != 0)
                {
                    MessageBox.Show((chk == 1) ? "Red is won!" : "Green is won!");
                    nodes = new Node[24];
                }

                turn = !turn;
                this.Invalidate();
                this.Update();

                if (chkComputer.Checked && turn == true)
                {
                    Node[] nodesCopy = nodes;

                    // Check whether the computer can win?
                    for (int i = 0; i < 24; i++)
                    { 
                        if (nodesCopy[i].State==0)
                        {
                            nodesCopy[i].State = 2;
                            if (Check(nodesCopy) == 2)
                            {
                                nodesCopy[i].State = 0;
                                numericUpDown1.Value = i + 1;
                                //System.Threading.Thread.Sleep(500);
                                btnPut_Click(null, new EventArgs());
                                return;
                            }
                            else
                                nodesCopy[i].State = 0;
                        }
                    }


                    // check whether the computer can prevent the player to win
                    for (int i = 0; i < 24; i++)
                    {
                        if (nodesCopy[i].State == 0)
                        {
                            nodesCopy[i].State = 1;

                            if (Check(nodesCopy) == 1)
                            {
                                nodesCopy[i].State = 0;
                                numericUpDown1.Value = i + 1;
                                //System.Threading.Thread.Sleep(500);
                                btnPut_Click(null, new EventArgs());
                                return;
                            }
                            else
                                nodesCopy[i].State = 0;
                        }
                    }

                    // use a random node => the first empty node
                    for (int i = 0; i < 24; i++)
                    {
                        if (nodesCopy[i].State == 0)
                        {
                            numericUpDown1.Value = i + 1;
                            System.Threading.Thread.Sleep(500);
                            btnPut_Click(null, new EventArgs());
                            return;
                        }
                    }
                }
            }
        }

        private int Check()
        {
            return Check(nodes);
        }

        private int Check(Node[] nodes)
        {
            for (int i = 0; i < 24; i += 3)
            {
                if (nodes[i].State!=0 && nodes[i].State == nodes[i + 1].State &&
                    nodes[i + 1].State == nodes[i + 2].State)
                    return nodes[i].State;
            }
            int[,] arr = new int[12, 3]
                {
                    { 1,7,10},
                    { 2,8,11},
                    { 3,9,12},
                    { 4,21,24},
                    { 5,20,23},
                    { 6,19,22},
                    { 7,13,19},
                    { 8,14,20},
                    { 9,15,21},
                    { 10,16,22},
                    { 11,17,23},
                    { 12,18,24}
                };

            for (int i=0;i<12;i++)
            {
                if (Check3(nodes, arr[i, 0]-1, arr[i, 1]-1, arr[i, 2]-1))
                    return nodes[arr[i, 0]-1].State;
            }

            return 0;
            
        }

        private bool Check3(Node[] nodes, int i, int j, int k)
        {
            if (nodes[i].State!=0 && nodes[i].State == nodes[j].State &&
                nodes[j].State == nodes[k].State)
                return true;
            else
                return false;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            int i = FindNode(e.X, e.Y);

            if (i >= 0 && i < 24)
            {
                numericUpDown1.Value = i + 1;
                btnPut_Click(null, new EventArgs());
            }

        }


        private int FindNode(int x, int y)
        {
            for (int i = 0; i < 24; i++)
            {
                if (x >= nodes[i].X && x <= nodes[i].X + nodeDiameter &&
                    y >= nodes[i].Y && y <= nodes[i].Y + nodeDiameter
                    )
                    return i;
            }

            return -1;
        }

    }
}
