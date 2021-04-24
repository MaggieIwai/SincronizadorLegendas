using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SincronizadorLegendas.service
{
    public static class Sincronizador
    {


        public static String teste3(String value) {

            //string value = "6:12:14:45,3448";
            //string[] cultureNames = { "hr-HR", "en-US" };
            //string cultureName = "fr-FR";
            //Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            //Console.WriteLine("Current Culture: {0}",
            //                  Thread.CurrentThread.CurrentCulture.Name);
            string valueOut = "";
            try {
                TimeSpan ts = TimeSpan.Parse(value);
                Console.WriteLine("{0} --> {1}", value, ts.ToString("c"));
                long offset = 1000;
                TimeSpan duration = TimeSpan.FromMilliseconds(offset);
                TimeSpan answer = ts.Add(duration);
                Console.WriteLine("{0} --> {1}", answer, answer.ToString("c"));
                DateTime dt = new DateTime() + answer;
                Console.WriteLine("{0} --> {1}", dt.ToString("HH:mm:ss,fff"), answer.ToString("c"));
                valueOut = dt.ToString("HH:mm:ss,fff");


            }
            catch (FormatException) {
                Console.WriteLine("{0}: Bad Format", value);
            }
            catch (OverflowException) {
                Console.WriteLine("{0}: Overflow", value);
            }
            Console.WriteLine();
            return valueOut;
        }
        //private String nomeArquivoUp;
        //private String nomeArquivoDown;
        //private String pathUpload;
        //private String pathDownload;
        //private long offset;

        //public Sincronizador() {

        //}

        //public String getNomeArquivoUp() {
        //    return nomeArquivoUp;
        //}

        //public void setNomeArquivoUp(String nomeArquivoUp) {
        //    this.nomeArquivoUp = nomeArquivoUp;
        //}

        //public String getNomeArquivoDown() {
        //    return nomeArquivoUp;
        //}

        //public void setNomeArquivoDown(String nomeArquivoDown) {
        //    this.nomeArquivoDown = nomeArquivoDown;
        //}
        //public String getPathUpload() {
        //    return pathUpload;
        //}

        //public void setPathUpload(String pathUpload) {
        //    this.pathUpload = pathUpload;
        //}
        //public String getPathDownload() {
        //    return pathUpload;
        //}

        //public void setPathDownload(String pathDownload) {
        //    this.pathDownload = pathDownload;
        //}
        //public long getOffset() {
        //    return offset;
        //}

        //public void setOffset(long offset) {
        //    this.offset = offset;
        //}


    }
}