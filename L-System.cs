using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace task5
{
    public enum dir { up, down, left, right };

    public class L_System
    {
        public String init;
        public Dictionary<char, String> rules = new Dictionary<char, string>();
        public dir dir;
        public double angle;

        public L_System(String filename)
        {
            try
            {
                var lines = File.ReadAllLines(filename).Select(s => s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray());
                init = lines.First()[0];
                angle = double.Parse(lines.First()[1]) * Math.PI / 180.0;
                dir = (dir)Enum.Parse(typeof(dir), lines.First()[2]);

                foreach (var line in lines.Skip(1))
                {
                    if (line[1] == "_")
                        rules.Add(line[0][0], "");
                    else
                        rules.Add(line[0][0], line[1]);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public String ApplyRules(int generation)
        {
            String res = init;

            for (int i = 0; i < generation; ++i)
            {
                string new_str = "";
                for (int j = 0; j < res.Length; ++j)
                {
                    if (rules.ContainsKey(res[j]))
                    {
                        new_str += rules[res[j]];
                    }
                    else
                    {
                        new_str += res[j];
                    }
                }
                res = new_str;
            }
            return res;
        }
    }
}
