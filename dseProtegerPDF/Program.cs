using System;
using System.IO;
using System.Reflection;

/*
    Aplicacion para proteger con contraseña un PDF 
    Version 1.0 - Version inicial
    Desarrollada por Carlos Clemente - 02/204
    
    Uso:
        dseprotegepdf fichero.pdf -c password [fichero_protegido.pdf]
        dseprotegepdf ficheros.txt -cl

    Parametros:
        -h                    : Esta ayuda
        fichero.pdf           : nombre del fichero PDF a procesar (unico fichero)
        -c                    : parametro que indica que se proteja con contraseña el fichero.pdf
        password              : contraseña para proteger el PDF
        fichero_protegido.pdf : nombre del fichero protegido; si no se pasa se utiliza el nombre del PDF modificado
        -cl                   : parametro para protejer con contraseña la lista de ficheros que hay en ficheros.txt
        ficheros.txt          : fichero que contiene el nombre de cada PDF a proteger junto a su contraseña

     */

namespace dseProtegerPDF
{
    class Program
    {
        // Obtener la información del ensamblado actual
        static Assembly assembly = Assembly.GetExecutingAssembly();

        // Obtener el atributo del copyright del ensamblado
        static AssemblyCopyrightAttribute copyrightAttribute =
            (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute));

        //Obtener el atributo del nombre del ensamblado
        static AssemblyProductAttribute nombreProducto = (AssemblyProductAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute));

        // Obtener el valor de la propiedad Copyright y nombre del producto
        static string copyrightValue = copyrightAttribute?.Copyright;
        static string nombreValue = nombreProducto?.Product;


        //Variable para chequear si los parametros pasados son correctos
        static bool continuar = false;

        //Variable para controlar el tipo de proceso a realizar
        static string tipoProceso = string.Empty;

        //Instanciacion de la clase procesosPDF para acceder a los metodos definidos en ella
        public static ProtegerPDF proceso = new ProtegerPDF();

        static void Main(string[] args)
        {
            continuar = gestionParametros(args);
            bool resultado;

            //Si los parametros pasados son correctos realiza los procesos que se hayan pasado como parametros
            if (continuar)
            {
                switch (tipoProceso)
                {
                    case "ProtegePDF":
                        resultado = proceso.ProtegePDF();
                        if (resultado)
                        {
                            proceso.grabaFichero("resultado.txt", "Constraseña aplicada correctamente");
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                        break;

                    case "ProtegeMasivo":
                        resultado = proceso.ProcesarListaPDF();
                        if (resultado)
                        {
                            proceso.grabaFichero("resultado.txt", "Constraseñas aplicadas correctamente");
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                        break;
                }
            }
        }

        static bool gestionParametros(string[] parametros)
        {
            int totalParametros = parametros.Length;
            switch (totalParametros)
            {
                case 0:
                    //Si no se pasan argumentos debe ser porque se ha ejecutado desde windows
                    // Abre una ventana de consola para mostrar el mensaje
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetWindowSize(120, 28);
                    Console.SetBufferSize(120, 28);
                    Console.Clear();
                    Console.Title = $"{nombreValue} - {copyrightValue}";
                    Console.WriteLine("\r\nEsta aplicacion debe ejecutarse por linea de comandos y pasarle los parametros correspondientes.");
                    mensajeAyuda();
                    Console.SetWindowSize(120, 28);
                    Console.SetBufferSize(120, 28);
                    Console.ResetColor();
                    Console.Clear();
                    Environment.Exit(0);
                    break;

                case 1:
                    //Si solo se pasa un parametro puede ser la peticion de ayuda
                    if (parametros[0] == "-h")
                    {
                        mensajeAyuda();
                    }
                    break;

                default:
                    break;
            }

            //Procesado del resto de parametros
            int controlParametros = 0;
            for (int i = 1; i < totalParametros; i++)
            {
                switch (parametros[i])
                {
                    //Proceso para proteger con contraseña el fichero
                    case "-c":
                        //Comprueba si existe el fichero PDF
                        if (!File.Exists(parametros[0]))
                        {
                            proceso.grabaFichero("resultado.txt", "El fichero PDF no existe");
                            return false;
                        }
                        else
                        {
                            proceso.ficheroPDF = parametros[0];
                            //Si se pasan 4 parametros el ultimo es el nombre del fichero protegido
                            if (totalParametros == 4)
                            {
                                proceso.ficheroProtegido = parametros[3];
                            }
                            else
                            {
                                proceso.ficheroProtegido = proceso.AsignaNombreFichero(parametros[0]);
                            }

                            //El siguiente parametro debe ser la contraseña de apertura del PDF
                            if (parametros[i + 1].Length > 2)
                            {
                                proceso.userPass = parametros[i + 1];
                                tipoProceso = "ProtegePDF";
                                controlParametros++;
                            }
                        }
                        break;

                    //Proceso para proteger una lista de ficheros
                    case "-cl":
                        //Se comprueba si existe el fichero con la relacion de PDFs
                        if (!File.Exists(parametros[0]))
                        {
                            proceso.grabaFichero("resultado.txt", "El fichero con la relacion de PDFs no existe");
                        }
                        else
                        {
                            proceso.listaPDF = parametros[0];
                            tipoProceso = "ProtegeMasivo";
                            controlParametros++;
                        }
                        break;
                }
            }

            //Control si los parametros pasados son correctos
            if (controlParametros > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void mensajeAyuda()
        {
            string mensaje =
                "\r\nUso:" +
                "\r\n\tdsepdfatexto fichero.pdf -c password [fichero_prot.pdf]" +
                "\r\n\tdsepdfatexto ficheros.txt -cl" +
                "\r\n\r\nParametros:" +
                "\r\n\t-h                  : Esta ayuda" +
                "\r\n\tfichero.pdf         : nombre del fichero PDF a procesar (unico fichero)" +
                "\r\n\t-c                  : parametro que indica que se proteja con contraseña el fichero.pdf" +
                "\r\n\tpassword            : contraseña para proteger el PDF" +
                "\r\n\tfichero_prot.pdf    : nombre del fichero protegido; si no se pasa se utiliza el nombre del PDF modificado" +
                "\r\n\tficheros.txt        : fichero con el nombre de los PDFs y contraseñas (fichero.pdf#contraseña)" +
                "\r\n\t-cl                 : parametro para protejer con contraseña la lista de ficheros que hay en ficheros.txt" +
                "\r\n\r\n\r\nPulse una tecla para salir";

            Console.WriteLine(mensaje);
            Console.ReadKey();
        }
    }
}
