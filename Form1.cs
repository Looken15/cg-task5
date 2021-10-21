using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace task5
{
    public partial class Form1 : Form
    {
        private L_System system;
        private static int generation = 2;
        private Graphics g;
        private Color color = Color.Black;
        private double len = 300.0 / Math.Pow(1.5, generation);
        private float width = 5;

        private Random random = new Random();

        private string long_rule;

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = File.ReadAllText("l-system.txt");

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);

            textBox1.Text = generation.ToString();
        }

        public void Interpret(float x, float y)
        {
            var angle = DirToAngle(system.dir);

            for (int i = 0; i < long_rule.Length; ++i)
            {
                if (new char[] { 'F', 'A', 'B', 'X', 'Y' }.Contains(long_rule[i]))
                {
                    (x, y) = DrawLine(x, y, angle, len, width, color);
                }
                else if (long_rule[i] == '-')
                {
                    if (randomCheckBox.Checked)
                        angle += random.NextDouble() * system.angle;
                    else
                        angle += system.angle;
                }
                else if (long_rule[i] == '+')
                {
                    if (randomCheckBox.Checked)
                        angle -= random.NextDouble() * system.angle;
                    else
                        angle -= system.angle;
                }
                else if (long_rule[i] == '[')
                {
                    ++i;
                    InterpretStep(ref i, x, y, angle, len, width, color);
                }
                else if (long_rule[i] == '@')
                {
                    width = width * 2 / 3;
                    len = len * 4 / 5.0;
                    color = Color.FromArgb((color.R + 30) * 4 / 5, color.G, color.B);
                }
            }
        }

        private void InterpretStep(ref int i, float x, float y, double angle, double len, float width, Color color)
        {
            while (long_rule[i] != ']')
            {
                if (new char[] { 'F', 'A', 'B', 'X', 'Y' }.Contains(long_rule[i]))
                {
                    (x, y) = DrawLine(x, y, angle, len, width, color);
                }
                else if (long_rule[i] == '-')
                {
                    if (randomCheckBox.Checked)
                        angle += random.NextDouble() * system.angle;
                    else
                        angle += system.angle;
                }
                else if (long_rule[i] == '+')
                {
                    if (randomCheckBox.Checked)
                        angle -= random.NextDouble() * system.angle;
                    else
                        angle -= system.angle;
                }
                else if (long_rule[i] == '[')
                {
                    ++i;
                    InterpretStep(ref i, x, y, angle, len, width, color);
                }
                else if (long_rule[i] == '@')
                {
                    width = width * 2 / 3;
                    len = len * 4 / 5.0;
                    color = Color.FromArgb((color.R + 50) * 4 / 5, color.G, color.B);
                }
                ++i;
            }
        }

        private double DirToAngle(dir dir)
        {
            switch (dir)
            {
                case dir.up: return 270.0;
                case dir.right: return 0.0;
                case dir.down: return 90.0;
                case dir.left: return 180.0;
                default: return 0.0;
            };
        }

        private (float, float) DrawLine(float x, float y, double angle, double len, float width, Color color)
        {
            angle = angle * Math.PI / 180.0;

            float new_x = (float)(x + Math.Cos(angle) * len);
            float new_y = (float)(y + Math.Sin(angle) * len);

            g.DrawLine(new Pen(color, width), x, y, new_x, new_y);
            pictureBox1.Refresh();

            return (new_x, new_y);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            var me = (MouseEventArgs)e;

            File.WriteAllText("l-system.txt", richTextBox1.Text);
            system = new L_System("l-system.txt");

            long_rule = system.ApplyRules(generation);

            width = 10;
            color = Color.Black;
            Interpret(me.X, me.Y);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox1.Text, out generation);
        }
    }
}