using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace crypt3
{
    public partial class Form1 : Form
    {
        private string fileContent = String.Empty;
        int bitsPerSymbol = 0;
        Dictionary<char, int> symbols = new Dictionary<char, int> { };
        Dictionary<char, int> symbolsTree = new Dictionary<char, int> { };

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var fileStream = dialog.OpenFile();
                fileContent = File.ReadAllText(dialog.FileName);
                analizeFile(fileContent);
                label4.Text = $"Initial file size: {getFileSize(fileContent, bitsPerSymbol)} bytes";
                richTextBox1.Text = fileContent;
                string text = "Symbol\tFrequency\n";
                foreach(var symb in symbols)
                {
                    text += $"{symb.Key}\t{symb.Value}\n";
                }
                richTextBox3.Text = text;
            }
            else
            {
                MessageBox.Show("Error opening the file", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //compress
        private void button2_Click(object sender, EventArgs e)
        {
            if (fileContent == String.Empty)
            {
                MessageBox.Show("Initial file is empty!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string compressedFile = huffmanCompress(fileContent);
            richTextBox4.Text = compressedFile;
            File.WriteAllText("2.txt", compressedFile);
            label5.Text = $"Compressed size: {getFileSize(compressedFile, 2)} bytes";
        }

        //decompress
        private void button3_Click(object sender, EventArgs e)
        {
            string compressedFile = File.ReadAllText("2.txt");
            if (compressedFile == String.Empty)
            {
                MessageBox.Show("Compressed file is empty!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string decompressedFile = huffmanDecompress(compressedFile);
            File.WriteAllText("3.txt", decompressedFile);
            //setPicture(pictureBox2, decompressedFile);
            label6.Text = $"Decompressed size: {getFileSize(decompressedFile, bitsPerSymbol)} bytes";
        }

        private string huffmanCompress(string file)
        {
            return "";
        }

        private string huffmanDecompress(string file) 
        {
            return "";
        }

        private void analizeFile(string file)
        {
            for (int i = 0; i < file.Length; i++)
            {
                if (symbols.ContainsKey(file[i]))
                    symbols[file[i]] += 1;
                else
                    symbols[file[i]] = 1;
            }
            bitsPerSymbol = Convert.ToInt32(Math.Ceiling(Math.Log(symbols.Count, 2)));
        }

        private int getFileSize(string file, int bitsPerSymbol)
        {
            return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(file.Length * bitsPerSymbol) / 8.0));
        }
    }

}
