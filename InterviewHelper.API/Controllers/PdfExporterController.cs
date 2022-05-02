using System.Net;
using System.Text;
using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models.PdfExporterModels;
using InterviewHelper.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;


namespace InterviewHelper.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PdfController : Controller
{
    private readonly IQuestionsService _questionsService;

    public PdfController(IQuestionsService questionsService)
    {
        _questionsService = questionsService;
    }

    [HttpPost]
    public IActionResult DownloadPdfFile(PdfExporterModelRequest modelRequest)
    {
        try
        {
            IronPdf.Installation.LinuxAndDockerDependenciesAutoConfig = true;
            var html = RenderHtmlViewToString(modelRequest);
            var ironPdfRender = new IronPdf.ChromePdfRenderer();
            using var pdfDoc = ironPdfRender.RenderHtmlAsPdf(html);
            return File(pdfDoc.Stream.ToArray(), "application/pdf");
        }
        catch (NoQuestionsProvidedException)
        {
            return BadRequest(new {message = "No questions provided for the pdf"});
        }
        catch (Exception ex)
        {
            return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private string RenderHtmlViewToString(PdfExporterModelRequest modelRequest)
    {
        var htmlPdf = System.IO.File.ReadAllText("../InterviewHelper.Core/Helper/PdfBaseTemplate.txt",
            System.Text.Encoding.UTF8);

        var questionItem = "<tr ><td valign={0} style={1}>{2}</td></tr>";
        var questionsBlock = "";
        var model = new PdfExporterModel()
        {
            InterviewDate = modelRequest.InterviewDate,
            IntervieweePosition = modelRequest.IntervieweePosition,
            Questions = _questionsService.GetQuestionsByIds(modelRequest.Questions)
        };
        foreach (var question in model.Questions)
        {
            questionsBlock += string.Format(questionItem, "top", "font-size:12px;", question);
        }

        htmlPdf = htmlPdf.Replace("{ZPXF88jaM2nFuBnhmXo0}", model.IntervieweePosition);
        htmlPdf = htmlPdf.Replace("{gHk4UMD5mMe9hvQ3VUn0}", model.InterviewDate.ToShortDateString());
        htmlPdf = htmlPdf.Replace("{wR4C4o25BUbyvg0yZNJh}", model.InterviewDate.ToShortTimeString());
        htmlPdf = htmlPdf.Replace("{0CZVLarSAJX4pyWsOWtB}", questionsBlock);
        return htmlPdf;
    }
}