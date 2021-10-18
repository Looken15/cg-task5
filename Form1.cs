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
        L_System system;
        int generation = 2;
        Graphics g;
        Color line_color = Color.Black;
        int from_x;
        int from_y;

        string long_rule;

        private void Form1_Load(object sender, EventArgs e)
        {
            system = new L_System("l-system.txt");
            richTextBox1.Text = File.ReadAllText("l-system.txt");

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);

            var s = system.ApplyRules(generation);
            richTextBox1.Text += "\n\n" + s;

            textBox1.Text = generation.ToString();
        }

        public void Interpret(float x, float y)
        {
            double len = 100.0 / Math.Pow(2, generation);
            var angle = DirToAngle(system.dir) * Math.PI / 180.0;

            for (int i = 0; i < long_rule.Length; ++i)
            {
                if (new char[] { 'F', 'A', 'B', 'X', 'Y' }.Contains(long_rule[i]))
                {
                    (x, y) = DrawLine(x, y, angle, len);
                }
                else if (long_rule[i] == '-')
                {
                    angle -= system.angle;
                }
                else if (long_rule[i] == '+')
                {
                    angle += system.angle;
                }
                else if (long_rule[i] == '[')
                {
                    ++i;
                    InterpretStep(ref i, x, y, angle, len);
                }
            }
        }

        private void InterpretStep(ref int i, float x, float y, double angle, double len)
        {
            while (long_rule[i] != ']')
            {
                if (new char[] { 'F', 'A', 'B', 'X', 'Y' }.Contains(long_rule[i]))
                {
                    (x, y) = DrawLine(x, y, angle, len);
                }
                else if (long_rule[i] == '-')
                {
                    angle -= system.angle;
                }
                else if (long_rule[i] == '+')
                {
                    angle += system.angle;
                }
                else if (long_rule[i] == '[')
                {
                    ++i;
                    InterpretStep(ref i, x, y, angle, len);
                }
                ++i;
            }
        }

        double DirToAngle(dir dir)
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

        (float, float) DrawLine(float x, float y, double angle, double len)
        {
            float new_x = (float)(x + Math.Cos(angle) * len);
            float new_y = (float)(y + Math.Sin(angle) * len);

            g.DrawLine(new Pen(line_color, 2), x, y, new_x, new_y);
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
            long_rule = system.ApplyRules(generation);
            Interpret(me.X, me.Y);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox1.Text, out generation);
        }
    }
}
