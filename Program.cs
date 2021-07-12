using System;
using System.IO;
using CommandLine;
using iText.Barcodes;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace Barcodes
{
    class Program
    {
        public class Options
        {
            [Option('b', "barcode", Required = true, HelpText = "Type of barcode that you want to create (2d or matrix)")]
            public string Barcode { get; set; }

            [Option('h', "height", Required = true, HelpText = "This is the height of the barcode")]
            public float Height { get; set; }

            [Option('w', "width", Required = false, HelpText = "This is the width of the barcode")]
            public float Width { get; set; }

            [Option('x', "xaxis", Required = true, HelpText = "X-axis with 0,0 being bottom left")]
            public float Xaxis { get; set; }

            [Option('y', "yaxis", Required = true, HelpText = "Y-axis with 0,0 being bottom left")]
            public float Yaxis { get; set; }

            [Option('d', "data", Required = true, HelpText = "this is the data that the barcode will be made of")]
            public string Data { get; set; }


        }
        static void Main(string[] args)
        {
            _ = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    if (o.Barcode == "2d" || o.Barcode == "matrix")
                    {
                        float height = o.Height;
                        float width = o.Width;
                        float xaxis = o.Xaxis;
                        float yaxis = o.Yaxis;
                        string data = o.Data;
                        Stream outStream = Console.OpenStandardOutput();
                        Stream inStream = Console.OpenStandardInput();
                        var writeProp = new WriterProperties();
                        var readProp = new ReaderProperties();
                        var writer = new PdfWriter(outStream, writeProp);
                        var reader = new PdfReader(inStream, readProp);
                        var doc = new PdfDocument(reader, writer);
                        BarcodeDataMatrix bar = new BarcodeDataMatrix("");
                        bar.SetCode(data);
                        PdfCanvas canvas = new PdfCanvas(doc.GetPage(1));
                        PdfFormXObject barcodeFormXObject = bar.CreateFormXObject(iText.Kernel.Colors.ColorConstants.BLACK, doc);
                        float x = xaxis; //76;
                        float y = yaxis; //350;
                        float w = width;
                        float h = height; //3;
                        canvas.AddXObject(barcodeFormXObject, x, y, h);
                        doc.Close();
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine($"Current Arguments:  -x {o.Xaxis}, -y {o.Yaxis}, -d {o.Data}, -b {o.Barcode}");
                        Console.WriteLine("Type help to make sure you are passing the right values");
                        Environment.Exit(1);
                    }
                });
        }
    }
}
