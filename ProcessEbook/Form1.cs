using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Microsoft.Office.Interop;

namespace ProcessEbook
{
    public partial class Form1 : Form
    {
        int numPadLeft = 3;
        public Form1()
        {
            InitializeComponent();
            nmChapter.Value = 1;
            checkBox2.Checked = true;
            textBox3.Text = @"D:\CreateSach";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(textBox3.Text.Trim()))
            {
                fb.SelectedPath = textBox3.Text.Trim();
            }

            if (fb.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = fb.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                textBox1.Text = "";
                textBox1.Paste();
            }
            textBox1.SelectionStart = 0;
            button2.Enabled = false;
            try
            {
                Process();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }

            button2.Enabled = true;

            if (checkBox1.Checked)
            {
                nmChapter.Value = nmChapter.Value + 1;
                textBox1.Text = "";
                textBox1.Select();
            }

            if (checkBox4.Checked)
            {
                button5_Click(null, null);
            }

        }

        private void Process()
        {
            
            string str = textBox1.Text;
            string style = string.Empty;
            int indexStyle = str.IndexOf("<style");
            if (indexStyle != -1)
            {
                int indexEndStyle = str.IndexOf("</style");
                indexEndStyle += 8;

                style = str.Substring(indexStyle, indexEndStyle - indexStyle);
                str = str.Substring(indexEndStyle).Trim();
            }
            else
            {
                int indexDiv = str.IndexOf(">");
                str = str.Substring(indexDiv + 1);

            }

            int lastIndexDiv = str.LastIndexOf("</div");
            str = str.Substring(0, lastIndexDiv);

            str = "<body>" + str + "</body>";
            str = str.Replace("<br>", "<br/>");
            str = str.Replace("&nbsp;", "");
            str = str.Replace("&ndash;", "-");
            str = str.Replace("&mdash;", "-");
            str = str.Replace("&copy;", "©");
            //str = str.Replace("&lt;", "<");
            //str = str.Replace("&gt;", ">");
            //str = str.Replace("&amp;", "&");

            //process img
            int indexImg = str.IndexOf("<img");
            int indexEndImg;
            while (indexImg >= 0)
            {
                indexEndImg = str.IndexOf(">",indexImg +1);
                if (indexEndImg > indexImg)
                {
                    str = str.Insert(indexEndImg+1,"</img>");
                }
                indexImg = str.IndexOf("<img", indexEndImg +2);
            }
            XmlDocument doc = new XmlDocument();
           
            try
            {
                doc.LoadXml(str);
            }
            catch
            {
                //Process hr
                int indexhr = str.IndexOf("<hr");
                while (indexhr != -1)
                {
                    int indexEnd = str.IndexOf(">", indexhr+1);
                    if (indexEnd != -1)
                    {
                        str = str.Insert(indexEnd + 1, "</hr>");
                    }
                    else
                        break;
                    indexhr = str.IndexOf("<hr", indexEnd + 3);

                }

                //Process hr
                indexhr = str.IndexOf("<br ");
                while (indexhr != -1)
                {
                    int indexEnd = str.IndexOf(">", indexhr + 1);
                    if (indexEnd != -1)
                    {
                        str = str.Insert(indexEnd + 1, "</br>");
                    }
                    else
                        break;
                    indexhr = str.IndexOf("<br ", indexEnd + 3);

                }

                try
                {
                    doc.LoadXml(str);
                }
                catch
                {
                    //process spl
                    int indexSpl = str.IndexOf("<spl");
                    while (indexSpl > -1)
                    {
                        int indexEnd = str.IndexOf(">", indexSpl + 1);
                        string temp = str.Substring(indexSpl +1, indexEnd - indexSpl-1);
                        string t = temp.Substring(temp.LastIndexOf("=") + 1);
                        str = str.Remove(indexSpl + 1, indexEnd - indexSpl-1);
                        str = str.Insert(indexSpl + 1, "span rel = \"" + t +"\"");

                        int indexEndSpl = str.IndexOf("</spl",indexEnd +8);
                        indexEnd = str.IndexOf(">", indexEndSpl + 1);
                        str = str.Remove(indexEndSpl + 1, indexEnd - indexEndSpl-1);
                        str = str.Insert(indexEndSpl + 1, "/span");
                        indexSpl = str.IndexOf("<spl", indexEndSpl +1);

                    }

                    try
                    {
                        doc.LoadXml(str);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        
                    }
                }
            }

            ProcessDoc pDoc = new ProcessDoc(doc);
            pDoc.Process();
        
    
            str = "<html xmlns=\"http://www.w3.org/1999/xhtml\">";
            str += "\n <head>";
            str += "\n <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />";
            str += "\n <title>"+ nmChapter.Value.ToString() +"</title>";
            str += style;
            str += "<style type=\"text/css\">";
            str += "p {text-indent:30px;}";
            str += "td {	border: 1px solid black;}";
            str += "</style>";
            str += doc.InnerXml;
            str += "\n </head>";
            str += "\n </html>";

            string fileName = textBox3.Text + "\\" + nmChapter.Value.ToString().PadLeft(numPadLeft, '0') + ".htm";
            string fileWord = textBox3.Text + "\\" + nmChapter.Value.ToString().PadLeft(numPadLeft, '0') + ".doc";

            if(!Directory.Exists(textBox3.Text))
                Directory.CreateDirectory(textBox3.Text);
            TextWriter tw = new StreamWriter(fileName);
            tw.Write(str);
            tw.Close();
            CreateWord(fileName,fileWord);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }


        public void CreateWord(String HtmlFile, string fileWord)
        {



            object filename1 = HtmlFile;

            object oMissing = System.Reflection.Missing.Value;


            object oFalse = false;

            Microsoft.Office.Interop.Word.Application oWord = new
            Microsoft.Office.Interop.Word.Application();

            Microsoft.Office.Interop.Word.Document oDoc = new
            Microsoft.Office.Interop.Word.Document();

            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref
oMissing, ref oMissing);

            oWord.Visible = false;

            oDoc = oWord.Documents.Open(ref filename1, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            filename1 = fileWord;

            object fileFormat =
            Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;

            oDoc.SaveAs(ref filename1, ref fileFormat, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing);

            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);

            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);



        }

        public void Merge(string[] filesToMerge, string outputFilename, bool insertPageBreaks, string documentTemplate)
        {
            object defaultTemplate = documentTemplate;
            object missing = System.Type.Missing;
            object pageBreak = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
            object outputFile = outputFilename;

            // Create  a new Word application
            Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();

            try
            {
                // Create a new file based on our template
                Microsoft.Office.Interop.Word._Document wordDocument = wordApplication.Documents.Add(
                                              ref missing
                                            , ref missing
                                            , ref missing
                                            , ref missing);

                // Make a Word selection object.
                Microsoft.Office.Interop.Word.Selection selection = wordApplication.Selection;

                // Loop thru each of the Word documents
                foreach (string file in filesToMerge)
                {
                    // Insert the files to our template
                    selection.InsertFile(
                                                file
                                            , ref missing
                                            , ref missing
                                            , ref missing
                                            , ref missing);

                    //Do we want page breaks added after each documents?
                    if (insertPageBreaks)
                    {
                        selection.InsertBreak(ref pageBreak);
                    }
                }

                object fileFormat =
            Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                // Save the document to it's output file.
                wordDocument.SaveAs(
                                ref outputFile
                            , ref fileFormat
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing);

                // Clean up!
                wordDocument = null;
            }
            catch (Exception ex)
            {
                //I didn't include a default error handler so i'm just throwing the error
                throw ex;
            }
            finally
            {
                // Finally, Close our Word application
                wordApplication.Quit(ref missing, ref missing, ref missing);
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBox3.Text))
            {
                return;
            }

            if(string.IsNullOrEmpty(textBox2.Text.Trim()))
            {
                return;
            }

            string fileName = textBox3.Text + "\\" + textBox2.Text + ".doc";
            if (File.Exists(fileName))
                File.Delete(fileName);
            string[] fileInfo = Directory.GetFiles(textBox3.Text, "*.doc");
            if (fileInfo.Length == 0)
            {
                return;
            }
            button4.Enabled = false;
            string defaultWordDocumentTemplate = @"Normal.dot";
            this.Merge(fileInfo, fileName, true, defaultWordDocumentTemplate);
            button4.Enabled = true;

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            
            this.TopMost = checkBox2.Checked;
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string file;
            if(!checkBox1.Checked)
                file = textBox3.Text + "\\" + nmChapter.Value.ToString().PadLeft(numPadLeft, '0') + ".htm";
            else
                file = textBox3.Text + "\\" + ((int)nmChapter.Value - 1).ToString().PadLeft(numPadLeft, '0') + ".htm";
            if (File.Exists(file))
                System.Diagnostics.Process.Start(file);
            else
            {
                MessageBox.Show("File is not exist");
            }
        }

    }
}
