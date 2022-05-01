using System.Drawing;
using InterviewHelper.Core.Models.PdfExporterModels;
using InterviewHelper.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using RazorEngine;
using RazorEngine.Templating;

namespace InterviewHelper.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PdfController : Controller
{
    private readonly IQuestionsServices _questionsService;

    public PdfController(IQuestionsServices questionsService)
    {
        _questionsService = questionsService;
    }

    [HttpPost]
    public IActionResult DownloadPdfFile(PdfExporterModelRequest modelRequest)
    {
        IronPdf.Installation.LinuxAndDockerDependenciesAutoConfig = true;
        var model = new PdfExporterModel()
        {
            InterviewDate = modelRequest.InterviewDate,
            IntervieweePosition = modelRequest.IntervieweePosition,
            Questions = _questionsService.GetQuestionsByIds(modelRequest.Questions)
        };
        var html = RenderRazorViewToString("myQuestionPdf", model);
        var ironPdfRender = new IronPdf.ChromePdfRenderer();
        using var pdfDoc = ironPdfRender.RenderHtmlAsPdf(html);
        return File(pdfDoc.Stream.ToArray(), "application/pdf");
    }

    private static string RenderRazorViewToString(string viewName, PdfExporterModel model)
    {
        var razorContent = System.IO.File.ReadAllText("../InterviewHelper.Core/Helper/PdfBaseTemplate.cshtml",
            System.Text.Encoding.UTF8);

        return Engine.Razor.RunCompile(razorContent, viewName, typeof(PdfExporterModel), model);
    }
}