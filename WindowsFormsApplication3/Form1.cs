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

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        BindingList<string> Std = new BindingList<string>();
        BindingList<string> Topic = new BindingList<string>();
        Dictionary<string, BindingList<string>> tm = new Dictionary<string, BindingList<string>>();

        public Form1()
        {
            InitializeComponent();

            comboBox1.DataSource = Topic;
            listBox2.DataSource = Std;
            listBox3.DataSource = Topic;
            listBox4.DataSource = Std;
        }

        // Move Student from stock to topic
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                if (comboBox1.Text.Trim() != "")
                {
                    BindingList<string> temp;
                    if (tm.TryGetValue(comboBox1.Text, out temp))
                    {
                        //value exists!
                        temp.Add(listBox2.SelectedItem.ToString());
                        tm[comboBox1.Text] = temp;

                        Std.Remove(listBox2.SelectedItem.ToString());
                    }
                }
            }
        }

        // Move Student from topic to stock
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                Std.Add(listBox1.SelectedItem.ToString());
                tm[comboBox1.Text].Remove(listBox1.SelectedItem.ToString());
            }
        }

        // Add topic
        private void button3_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Trim().Equals("") && !tm.Keys.Contains(textBox1.Text.Trim()))
            {
                Topic.Add(textBox1.Text.Trim());
                BindingList<string> temp;
                temp = new BindingList<string>();
                tm.Add(textBox1.Text.Trim(), temp);
                textBox1.Clear();
            }
            else if (tm.Keys.Contains(textBox1.Text.Trim()))
            {
                MessageBox.Show("Topic is exists");
            }
        }

        // Delete topic
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {
                foreach (var item in tm[listBox3.SelectedItem.ToString()])
                {
                    Std.Add(item);
                }
                tm.Remove(listBox3.SelectedItem.ToString());
                Topic.Remove(listBox3.SelectedItem.ToString());
                
                if (Topic.Count == 0)
                {
                    listBox1.DataSource = null;
                }
            }
        }

        // Save Student
        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text File|*.txt";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.OpenFile());
                foreach (var item in Std)
                {
                    writer.WriteLine(item);
                }
                writer.Close();
            }
        }

        // Load Student
        private void button6_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            StreamReader reader;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        Std.Clear();
                        using (myStream)
                        {
                            reader = new StreamReader(myStream);
                            while (reader.EndOfStream == false)
                            {
                                Std.Add(reader.ReadLine());
                            }
                            reader.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk.");
                }
            }
        }

        // Add Student
        private void button7_Click(object sender, EventArgs e)
        {
            if (!textBox2.Text.Trim().Equals(""))
            {
                Std.Add(textBox2.Text.Trim());
                textBox2.Clear();
            }

        }

        // Delete Student
        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedItem != null)
            {
                Std.Remove(listBox4.SelectedItem.ToString());
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // ComboBox of Topic
        // when value change load list of member in these topic
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.DataSource = null;
            BindingList<string> temp2 = new BindingList<string>();
            if (tm.TryGetValue(comboBox1.Text, out temp2))
            {
                listBox1.DataSource = temp2;
            }

        }

		// Update Report
        private void button9_Click(object sender, EventArgs e)
        {
            string str = "";
            foreach (var items in tm)
            {
                str += items.Key + " : ";
                Console.Write(items.Key + "  : ");
                foreach (var item in items.Value)
                {
                    str += item + " ";
                    Console.Write(item + "  ");
                }
                str += "\n";
                Console.WriteLine();
            }

            label6.Text = str;
        }

        private void label6_Click(object sender, EventArgs e)
        {
 
        }

		// Random Group
        private void button10_Click(object sender, EventArgs e)
        {
            if (Topic.Count != 0)
            {
                foreach (var items in tm)
                {
                    foreach (var item in items.Value)
                    {
                        Std.Add(item);
                    }
                    items.Value.Clear();
                }
                int ratio = Std.Count / Topic.Count;
                Random rand = new Random();
                foreach (var items in tm)
                {
                    for (int i = 0; i < ratio; i++)
                    {
                        int index = rand.Next() % Std.Count;
                        items.Value.Add(Std.ElementAt(index));
                        Std.RemoveAt(index);
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

		// Remove All
        private void button11_Click(object sender, EventArgs e)
        {
            foreach (var items in tm)
            {
                foreach (var item in items.Value)
                {
                    Std.Add(item);
                }
                items.Value.Clear();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Stop) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Project Manager : Nattapon Phonpo\nDesigner : Vipapohn\nProgrammer : Anukul\nTester : Yukonthon"
                ,"Developer",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
