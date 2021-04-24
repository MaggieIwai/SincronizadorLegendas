using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;


namespace SincronizadorLegendas {
    public partial class UpLoad : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void btnSubmit_Click(object sender, EventArgs e) {
            String nomeUp = "";
            String nomeDown = "";
            String strOffset = "";
            String caminhoUp = Server.MapPath(@"upload\");
            String caminhoDown = Server.MapPath(@"download\");


            lblMensagem.Text = "";
            if (!fuArquivo.HasFile) {
                lblMensagem.Text = "Informe o arquivo a ser processado.";
                return;
            }
            else if ((fuArquivo.FileName.Contains(",") || (fuArquivo.FileName.Contains(";")))) {
                lblMensagem.Text = @"O nome do arquivo contém caracteres tais como ' , e ; '. Altere o nome e processe novamente.";
                return;
            }

            if (txtOffset.Text.Trim() == "") {
                lblMensagem.Text = "Informe o offset em milisegundos.";
                return;
            }

            nomeUp = fuArquivo.FileName;
            nomeDown = nomeUp.ToLower().Replace(".srt", ".offset.srt");
            strOffset = txtOffset.Text;
            fuArquivo.PostedFile.SaveAs(caminhoUp + nomeUp);

            Processamento(caminhoUp + nomeUp, caminhoDown + nomeDown, Convert.ToInt64(strOffset));

            DataTable dt = new DataTable();
            dt.Columns.Add("File", typeof(string));

            foreach (string strFile in Directory.GetFiles(caminhoDown)) {
                FileInfo fi = new FileInfo(strFile);
                dt.Rows.Add(fi.Name);
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();

            lblMensagem.Text = "Processamento finalizado. Faça do download do arquivo, selecionando-o no grid abaixo.";
        }

        public static void Processamento(string sourcePath, string targetPath, long offset) {
            Regex rx = new Regex(@"^\d{2}:\d{2}:\d{2},\d{3} --> \d{2}:\d{2}:\d{2},\d{3}$");
            String msg;
            try {
                string[] lines = File.ReadAllLines(sourcePath);

                if (File.Exists(targetPath)) {
                    File.Delete(targetPath);
                }

                using (StreamWriter sw = File.AppendText(targetPath)) {
                        foreach (string line in lines) {
                            if (rx.IsMatch(line)) {
                                string newLine = line.Replace(" --> ", "|");
                                String[] timeOld = newLine.Split('|');

                                int iTam = timeOld.Length;
                                String texto = "";
                                String timeNew = "";
                                for (int i = 0; i < iTam; i++) {
                                    timeNew = AplicaOffset(timeOld[i], offset, out msg);
                                    if (i + 1 != iTam) {
                                        texto = texto + timeNew + " --> ";
                                    }
                                    else {
                                        texto = texto + timeNew;
                                    }

                                }
                                sw.WriteLine(texto);
                            }
                            else {
                                sw.WriteLine(line);
                            }
                        }
                    }
            }
            catch (IOException ex) {
                Console.WriteLine("An error occurred");
                Console.WriteLine(ex.Message);
            }
        }


        public static String AplicaOffset(String value, long offset, out String msg) {

            msg = "";
            string valueOut = "";
            try {
                TimeSpan ts = TimeSpan.Parse(value);
                TimeSpan duration = TimeSpan.FromMilliseconds(offset);
                TimeSpan answer = ts.Add(duration);

                DateTime dt = new DateTime() + answer;

                valueOut = dt.ToString("HH:mm:ss,fff");
            }
            catch (FormatException) {
                msg = value + ": Bad Format";
            }
            catch (OverflowException) {
                msg = value + ": Overflow";
            }

            return valueOut;
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName == "Download") {
                Response.Clear();
                Response.ContentType = "application/octect-stream";
                Response.AppendHeader("content-disposition", "filename=" + e.CommandArgument);
                Response.TransmitFile(Server.MapPath(@"download\") + e.CommandArgument);
                Response.End();
            }
        }
    }
}