using System.Collections.Generic;
using IronPdf;

namespace IronPdfToc
{
    internal class Program
    {
        private static readonly Chapter Page3 = new Chapter
        {
            Title = "Intro",
            Content = @"
<h1>Introduction</h1>
<div>This is the intro of the page</div>


    <div style='page-break-after: always;'>&nbsp;</div>

another intro page


    <div style='page-break-after: always;'>&nbsp;</div>

and another
"
        };

        private static readonly Chapter Page4 = new Chapter
        {
            Title = "Person list",
            Content = @"
<h1>All of the persons</h1>
<div>A lot of persons over here...</div>
<div style='page-break-after: always;'>&nbsp;</div>
really a lot
<div style='page-break-after: always;'>&nbsp;</div>
of persons!
"
        };

        private static void Main()
        {
            var renderer = CreateRenderer();

            var tableOfContents = new List<TableOfContents>();
            var currentPage = 3;
            tableOfContents.Add(new TableOfContents {PageNumber = currentPage, Title = Page3.Title});

            var thirdPage = CreatePageDocument(renderer, Page3.Content, currentPage);
            currentPage = currentPage + thirdPage.PageCount;

            tableOfContents.Add(new TableOfContents {PageNumber = currentPage, Title = Page4.Title});
            var fourthPage = CreatePageDocument(renderer, Page4.Content, currentPage);
            currentPage = currentPage + fourthPage.PageCount;

            var tableOfContentsTemplate = string.Empty;
            foreach (var tocItem in tableOfContents)
                tableOfContentsTemplate += $"<p>{tocItem.Title}_________________{tocItem.PageNumber}</p>";
            var tocDocument = CreatePageDocument(renderer, tableOfContentsTemplate, 2);

            var mergedDocument = PdfDocument.Merge(new[] {tocDocument, thirdPage, fourthPage});
            mergedDocument.SaveAs("HtmlToPDF.pdf");
        }

        private static PdfDocument CreatePageDocument(HtmlToPdf renderer, string page1Content, int startingPage = 0)
        {
            if (startingPage > 0) renderer.PrintOptions.FirstPageNumber = startingPage;
            var document = renderer.RenderHtmlAsPdf(page1Content);

            return document;
        }

        private static HtmlToPdf CreateRenderer()
        {
            var renderer = new HtmlToPdf();
            renderer.PrintOptions.MarginTop = 50; //millimeters
            renderer.PrintOptions.MarginBottom = 50;
            renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            renderer.PrintOptions.EnableJavaScript = false;
            renderer.PrintOptions.RenderDelay = 100;
            renderer.PrintOptions.Header = new SimpleHeaderFooter
            {
                CenterText = "{pdf-title}",
                DrawDividerLine = true,
                FontSize = 16
            };
            renderer.PrintOptions.Footer = new SimpleHeaderFooter
            {
                LeftText = "{date} {time}",
                RightText = "Page {page}",
                DrawDividerLine = true,
                FontSize = 14
            };
            return renderer;
        }

        private class Chapter
        {
            public string Title { get; set; }
            public string Content { get; set; }
        }

        private class TableOfContents
        {
            public int PageNumber { get; set; }
            public string Title { get; set; }
        }
    }
}