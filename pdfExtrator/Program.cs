using System;
using System.Diagnostics;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced; 

namespace pdfExtrator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			Console.WriteLine (Directory.GetCurrentDirectory());
			const string filename = "test.pdf";
			File.Copy(Path.Combine("../../docs/", filename), 
				Path.Combine(Directory.GetCurrentDirectory(), filename), true); 
			PdfDocument document = PdfReader.Open(filename); 
			PdfDictionary dict = new PdfDictionary(document); 
			dict.Elements["/S"] = new PdfName("/GoTo"); 
			PdfArray array = new PdfArray(document); 
			dict.Elements["/D"] = array; 
			PdfReference iref = PdfInternals.GetReference(document.Pages[2]); 
			array.Elements.Add(iref);
			array.Elements.Add(new PdfName("/FitV"));
			array.Elements.Add(new PdfInteger(-32768));
			document.Internals.AddObject(dict); 
			document.Internals.Catalog.Elements["/OpenAction"] = 
				PdfInternals.GetReference(dict); 
			document.Save(filename);
			Process.Start(filename);
			Console.ReadLine();
		}
	}
}
