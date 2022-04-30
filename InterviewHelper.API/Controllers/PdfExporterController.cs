using InterviewHelper.Core.Models.PdfExporterModels;
using Microsoft.AspNetCore.Mvc;
using RazorEngine;
using RazorEngine.Templating;

namespace InterviewHelper.Api.Controllers;
[ApiController]
    
[Route("[controller]")]
public class PdfController : Controller
{
    
    [HttpPost]
    public IActionResult DownloadPdfFile(PdfExporterModel model)
    {
        IronPdf.Installation.LinuxAndDockerDependenciesAutoConfig = true;
        var html = RenderRazorViewToString("myQuestionPdf", model);
        var ironPdfRender = new IronPdf.ChromePdfRenderer();
        using var pdfDoc = ironPdfRender.RenderHtmlAsPdf(html);
        return File(pdfDoc.Stream.ToArray(), "application/pdf");
    }
    
    private static string RenderRazorViewToString(string viewName, PdfExporterModel model)
    {
        var razorContent = System.IO.File.ReadAllText("../InterviewHelper.Core/Helper/PdfBaseTemplate.cshtml", System.Text.Encoding.UTF8);
         
        return RazorEngine.Engine.Razor.RunCompile(razorContent, viewName, typeof(PdfExporterModel), model);
    }
}