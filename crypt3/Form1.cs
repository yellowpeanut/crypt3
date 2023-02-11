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
        Huffman hfm = null;

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
                hfm = new Huffman(fileContent);
                label4.Text = $"Initial file size: {getFileSize(fileContent, hfm.bitsPerSymbol)} bytes";
                richTextBox1.Text = fileContent;
                string text = "";
                foreach(var symb in hfm.SymbolsFrequency)
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
            string compressedFile = hfm.Compress(fileContent);
            richTextBox4.Text = compressedFile;
            File.WriteAllText("2.txt", compressedFile);
            label5.Text = $"Compressed size: {getFileSize(compressedFile, 2)/8} bytes";

            string text = "";
            foreach (var symb in hfm.SymbolsFrequency)
            {
                text += $"{symb.Key}\t{symb.Value}\t{hfm.SymbolsCode[symb.Key]}\n";
            }
            richTextBox3.Text = text;
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
            if (hfm is null)
                hfm = new Huffman();
            string decompressedFile = hfm.Decompress(compressedFile);
            File.WriteAllText("3.txt", decompressedFile);
            richTextBox2.Text = decompressedFile;
            label6.Text = $"Decompressed size: {getFileSize(decompressedFile, hfm.bitsPerSymbol)} bytes";
        }

        private int getFileSize(string file, int bitsPerSymbol)
        {
            return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(file.Length * bitsPerSymbol) / 8.0));
        }
    }

}
