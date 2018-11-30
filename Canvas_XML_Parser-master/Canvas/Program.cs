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

namespace Canvas
{
    class Program
    {
        static void Main(string[] args)
        { 



            string pathFile = "";
            Canvas canvas = new Canvas();
            canvas.Add(new Point(10, 15));
            canvas.Add(new Line(new Point(1, 1), new Point(150, 120)));
            canvas.Add(new Line(5, 5, 25, 35));
            canvas.Add(new Rectangle(5, 5, 20, 20));
            canvas.Add(new Circle(new Point(50, 50), 30));
            canvas.Draw();
            canvas.SendToFront();

            //---------------------------SERIALIZACON--------------------------//
            string canvas_name = "test";

            CanvasSerializer serializer = new CanvasSerializer();
            var serialization = serializer.Serialize(canvas, canvas_name);

            Console.WriteLine("\nSerializando documento...");

            if (serialization.hasError)
                Console.WriteLine("-Error al serializar el documento :: " + serialization.error);
            else
            {
                Console.WriteLine("-Documento serializado correctamente en: " + serialization.path);
             pathFile = serialization.path;
            }


            //---------------------------END SERIALIZACON--------------------------//
            string path = string.Empty;
            string internal_path = Path.Combine(RootDirectory, "modiify.xslt");
            string pathOutput = Path.Combine(RootDirectory, "result.xml");


            XPathDocument myXPathDoc = new XPathDocument(pathFile);
            XslTransform myXslTrans = new XslTransform();
            myXslTrans.Load(internal_path);
            XmlTextWriter myWriter = new XmlTextWriter(pathOutput, null);
            myXslTrans.Transform(myXPathDoc, null, myWriter);



            //---------------------------DESERIALIZACION--------------------------//

            var deserialization = serializer.Deserialize(canvas_name);

            Console.WriteLine("\n\nDeserializando documento...");

            if (deserialization.hasError)
                Console.WriteLine("-Error al deserializar el documento :: " + deserialization.error);
            else
                Console.WriteLine("-Documento deserializador correctamente");
            //---------------------------END DESERIALIZACION--------------------------//


            //---------------------------CANVAS DESERIALIZADO------------------------//

            Console.WriteLine("\n\n\n---CANVAS DESERIALIZADO---\n\n\n" + deserialization.path);

            deserialization.canvas.Draw();
            deserialization.canvas.SendToFront();

            Console.WriteLine();

            Console.ReadLine();
            Console.ReadKey();

        }
        static string RootDirectory = Path.Combine(
           System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
           "CanvasDocuments"
       );

    }
}
