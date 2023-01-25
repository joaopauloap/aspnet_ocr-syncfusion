using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using ProjetoOCR.Models;
using System.Diagnostics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf;
using Syncfusion.Drawing;
using Syncfusion.OCRProcessor;

namespace ProjetoOCR.Controllers
{
    public static class FormFileExtensions
    {
        public static byte[] GetBytes(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CarregarAnexo(IFormFile anexo)
        {
            var file = FormFileExtensions.GetBytes(anexo);

            //PdfLoadedDocument loadedDocument = new PdfLoadedDocument(file);
            //PdfPageBase page = loadedDocument.Pages[0];
            //var lineCollection = new TextLineCollection();
            //string extractedText = page.ExtractText(out lineCollection);
            //foreach (var line in lineCollection.TextLine)
            //{
            //    RectangleF lineBounds = line.Bounds;
            //   string text = line.Text;
            //}

            using (OCRProcessor processor = new OCRProcessor("../../Tesseractbinaries/"))
            {
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(file);
                PdfPageBase page = loadedDocument.Pages[0];
                string extractedTexts = page.ExtractText(true);
                loadedDocument.Close(true);
                TempData["text"] = extractedTexts;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}