using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Diagnostics;

namespace Canvas
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathFile = "";
            string canvas_name = "test";
            bool onRun = true;
            String myMenu = "0";
            Canvas canvas = new Canvas();
            Actions action = new Actions();

            action.DrawPresentation();

            while (onRun)
            {
                if (myMenu != "0")
                    Console.Clear();

                switch (myMenu)
                {

                    case "0":
                        Console.WriteLine(" ----------------------------------------------------------------------");
                        Console.WriteLine(" 1 - Generar Canvas");
                        Console.WriteLine(" 2- Serializar documento");
                        Console.WriteLine(" 3- Deserializar documento");
                        Console.WriteLine(" 4- Modificar con XSLT");
                        Console.WriteLine(" 5- Abrir en Navegador");
                        Console.WriteLine(" 6- Salir");
                        Console.WriteLine(" -- Elija una opción --");
                        Console.WriteLine(" ----------------------------------------------------------------------");

                        myMenu = Console.ReadLine();
                        break;
                    case "1":
                        canvas = action.generateCanvas();
                        myMenu = "0";
                        break;
                    case "2":
                        action.sertializeCanvas(canvas, canvas_name, pathFile);
                        myMenu = "0";
                        break;
                    case "3":
                        action.deserializeCanvas(canvas_name);
                        myMenu = "0";
                        break;
                    case "4":
                        action.mutataInXSLT();
                        myMenu = "0";
                        break;
                    case "5":
                        action.openInNavigator();
                        myMenu = "0";
                        break;
                    case "6":
                        onRun = false;
                        myMenu = "0";
                        break;
                    default:
                        Console.WriteLine("Opción incorrecta");
                        myMenu = "0";
                        break;

                }
            }
        }

        static string RootDirectory = Path.Combine(
           System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
           "CanvasDocuments"
       );


        public class Actions
        {
            public Actions() { }

            /// <summary>
            /// Agrega todas las figuras al objeto Canvas
            /// </summary>
            public Canvas generateCanvas()
            {
                try
                {
                    Canvas canvas = new Canvas();
                    canvas.Add(new Point(500, 500));
                    canvas.Add(new Point(400, 400));
                    canvas.Add(new Point(300, 300));
                    canvas.Add(new Line(new Point(1, 1), new Point(150, 120)));
                    canvas.Add(new Line(400, 700, 800, 35));
                    canvas.Add(new Rectangle(200, 90, 400, 20));
                    canvas.Add(new Circle(new Point(50, 50), 30));
                    canvas.Add(new Circle(new Point(200, 180), 70));

                    //Estrella
                    canvas.Add(new Line(750, 700, 721, 790));
                    canvas.Add(new Line(721, 790, 798, 735));
                    canvas.Add(new Line(798, 735, 702, 735));
                    canvas.Add(new Line(702, 735, 779, 799));
                    canvas.Add(new Line(779, 799, 750, 700));
                    canvas.Draw();
                    canvas.SendToFront();
                    return canvas;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.ToString());
                    return null;
                }
            }

            /// <summary>
            /// Serializa el objeto Canvas
            /// </summary>
            public void sertializeCanvas(Canvas _canvas, string canvas_name, string pathFile)
            {
                CanvasSerializer serializer = new CanvasSerializer();

                var serialization = serializer.Serialize(_canvas, canvas_name);

                Console.WriteLine("\nSerializando documento...");

                if (serialization.hasError)
                    Console.WriteLine("-Error al serializar el documento :: " + serialization.error);
                else
                {
                    Console.WriteLine("-Documento serializado correctamente en: " + serialization.path);
                    pathFile = serialization.path;
                }

            }

            /// <summary>
            ///  Deserializa el archivo XML y lo vuelca al objeto Canvas
            /// </summary>
            public void deserializeCanvas(string canvas_name)
            {
                try
                {
                    CanvasSerializer serializer = new CanvasSerializer();
                    var deserialization = serializer.Deserialize(canvas_name);
                    Console.WriteLine("\n\nDeserializando documento...");

                    if (deserialization.hasError)
                        Console.WriteLine("-Error al deserializar el documento :: " + deserialization.error);
                    else
                        Console.WriteLine("-Documento deserializador correctamente");

                    deserialization.canvas.Draw();
                    deserialization.canvas.SendToFront();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            /// <summary>
            /// Muestra en pantalla la descripción de los objetos
            /// </summary>
            public void DrawPresentation()
            {
                try
                {
                    Random r = new Random();
                    Console.ForegroundColor = (ConsoleColor)r.Next(6, 16);
                
                    string line;
                    StreamReader file = new StreamReader(Path.Combine(@"../../description.txt"));
                    while ((line = file.ReadLine()) != null)
                    {
                        System.Console.WriteLine(line);
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            /// <summary>
            /// Transforma es archivo XML -> HTML mediante XSLT
            /// </summary>
            public void mutataInXSLT()
            {
                try
                {
                    string path = string.Empty;
                    string internal_path = Path.Combine(@"../../modifier.xslt");
                    string pathOutput = Path.Combine(RootDirectory, "result.html");
                    XPathDocument myXPathDoc = new XPathDocument(Path.Combine(RootDirectory, "test.xml"));
                    XslCompiledTransform trans = new XslCompiledTransform();
                    trans.Load(internal_path);
                    XmlTextWriter myWriter = new XmlTextWriter(pathOutput, null);
                    trans.Transform(myXPathDoc, null, myWriter);
                    myWriter.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            /// <summary>
            /// Abre el archivo generado en el navegador
            /// </summary>
            public void openInNavigator()
            {
                Process myProcess = new Process();

                try
                {
                    myProcess.StartInfo.UseShellExecute = true;
                    string pathOutput = Path.Combine(RootDirectory, "result.html");
                    myProcess.StartInfo.FileName = pathOutput;
                    myProcess.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }
    }
}


