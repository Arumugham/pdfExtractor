using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text;
using System.Text.RegularExpressions;

namespace pdfExtrator
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			Console.WriteLine (Directory.GetCurrentDirectory());
			//Console.WriteLine(testPDF("../../docs/test2.pdf"));
			Console.WriteLine(testsearcharea("../../docs/testdata.pdf"));

			//Console.WriteLine(testStringSearch);
			//Console.WriteLine(ExtractTextFromPdf("../../docs/testdata.pdf"));
			Console.ReadLine();
		}
		public static string ExtractTextFromPdf(string path)
		{
			using (PdfReader reader = new PdfReader(path))
			{
				StringBuilder text = new StringBuilder();
				//Console.WriteLine(reader.NumberOfPages);
				for (int i = 1; i <= reader.NumberOfPages; i++)
				{
					//Console.WriteLine(text);
					text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
				}

				return text.ToString();
			}
		}

		public static string testPDF(string path)
		{
			var text = new StringBuilder();

			// The PdfReader object implements IDisposable.Dispose, so you can
			// wrap it in the using keyword to automatically dispose of it
			using (var pdfReader = new PdfReader(path))
			{
				// Loop through each page of the document
				for (var page = 1; page <= pdfReader.NumberOfPages; page++)
				{
					ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

					var currentText = PdfTextExtractor.GetTextFromPage(
						pdfReader, 
						page, 
						strategy);

					currentText =
						Encoding.UTF8.GetString(Encoding.Convert(
							Encoding.Default,
							Encoding.UTF8,
							Encoding.Default.GetBytes(currentText)));
					Console.WriteLine("text:" + currentText);
					text.Append(currentText);
				}
			}
			return text.ToString();
		}

		public static string testStringSearch(string str)
		{
			String St = "வாக்காளர் ெபயர் : பிரபாகர்வாக்காளர் ெபயர் : ெகளவாக்காளர் ெபயர் : அேசாமாகுக்ர் \nசந்தரா";
			int pFrom = St.IndexOf("ெபயர் : ") + "ெபயர் : ".Length;
			int pTo = St.LastIndexOf("ெபயர் : ");
			return St.Substring(pFrom, pTo - pFrom);
		}

		public static string testsearcharea(string path)
		{
			// In this example, I'll declare a pageNumber integer variable to 
			// only capture text from the page I'm interested in
			int pageNumber = 3;

			var text = new StringBuilder();

			// The PdfReader object implements IDisposable.Dispose, so you can
			// wrap it in the using keyword to automatically dispose of it
			using (var pdfReader = new PdfReader(path))
			{
				float distanceInPixelsFromLeft = 30;
				//float distanceInPixelsFromBottom = 710;
				float distanceInPixelsFromBottom = 70;
				//float width = 190;
				//float height = 70;
				float width = 190;
				float height = 700;

				var rect = new System.util.RectangleJ(
					distanceInPixelsFromLeft,
					distanceInPixelsFromBottom, 
					width, 
					height);

				var filters = new RenderFilter[1];
				filters[0] = new RegionTextRenderFilter(rect);

				ITextExtractionStrategy strategy =
					new FilteredTextRenderListener(
						new LocationTextExtractionStrategy(), 
						filters);

				var currentText = PdfTextExtractor.GetTextFromPage(
					pdfReader, 
					pageNumber, 
					strategy);
				currentText =
					Encoding.UTF8.GetString(Encoding.Convert(
						Encoding.Default,
						Encoding.UTF8,
						Encoding.Default.GetBytes(currentText)));

				text.Append(currentText);
			}
			string str = text.ToString();
			string[] lines = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
			foreach(string test in lines)
			{
				Console.WriteLine("test-->"+ test);
			}
			// You'll do something else with it, here I write it to a console window
			return text.ToString();
		}
	}
}
