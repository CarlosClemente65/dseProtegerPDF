using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace dseProtegerPDF
{
    internal class ProtegerPDF
    {
        public string ficheroPDF; //Fichero PDF a procesar
        public string userPass; //Contraseña de apertura del PDF
        public string ownerPass = "D1agram."; //Contraseña maestra del PDF
        public string ficheroProtegido; //Fichero PDF protegido que se pasa por parametros
        public string listaPDF;

        public List<string> archivosPDF = new List<string>(); //Lista con todos los ficheros a procesar

        public bool ProtegePDF()
        {
            //Contraseñas de usuario y propietario

            using (var input = new FileStream(ficheroPDF, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var output = new FileStream(ficheroProtegido, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var reader = new PdfReader(input);
                    PdfEncryptor.Encrypt(reader, output, true, this.userPass, ownerPass, PdfWriter.AllowPrinting);
                    return true;
                }
            }
        }

        public bool ProcesarListaPDF()
        {
            string[] ficheros = File.ReadAllLines(listaPDF);
            string errores = string.Empty;
            bool resultado;

            for (int i = 0; i < ficheros.Length; i++)
            {
                //Dividir la linea en dos strings
                string[] valores = ficheros[i].Split('#');
                if (File.Exists(valores[0]))
                {
                    ficheroPDF = valores[0];
                    userPass = valores[1];
                    ficheroProtegido = AsignaNombreFichero(ficheroPDF);
                    resultado = ProtegePDF();
                    if (!resultado)
                    {
                        if (errores == "")
                        {
                            errores = "No se han podido proteger los ficheros siguientes:\r\n";
                        }
                        errores = errores + ($"\t- {Path.GetFileName(ficheroPDF)}\r\n");
                    }
                }
                else
                {
                    errores = $"El fichero {Path.GetFileName(ficheroPDF)} no existe";
                }
            }

            if (!string.IsNullOrEmpty(errores))
            {
                grabaFichero("resultado.txt", errores);
                return false;
            }
            else
            {
                return true;
            }
        }

        public string AsignaNombreFichero(string nombreFichero)
        {
            //Se forma el nombre del fichero protegido con el nombre del PDF
            string nombrePDF = Path.GetFileNameWithoutExtension(nombreFichero);
            string extensionPDF = Path.GetExtension(nombreFichero);
            string nombreFicheroProtegido = Path.Combine(Path.GetDirectoryName(nombreFichero), $"{nombrePDF}_pwd{extensionPDF}");
            return nombreFicheroProtegido;
        }

        public void grabaFichero(string nombreFichero, string texto)
        {
            string pathFichero = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombreFichero);
            File.WriteAllText(pathFichero, texto);
        }
    }
}
