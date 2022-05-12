using System.Net;
using System.Text;
using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models.PdfExporterModels;
using InterviewHelper.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace InterviewHelper.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class QuestionExportController : ControllerBase
{
    private readonly IQuestionsService _questionsService;

    public QuestionExportController(IQuestionsService questionsService)
    {
        _questionsService = questionsService;
    }

    [HttpPost]
    public IActionResult DownloadPdfFile(QuestionExportRequest modelRequest)
    {
        try
        {
            var questionsContent = _questionsService.GetQuestionsByIds(modelRequest.Questions);

            if (questionsContent.Count == 0)
            {
                return BadRequest(new {message = "No questions provided for the pdf"});
            }

            var model = new PdfExporterModel()
            {
                InterviewDate = modelRequest.InterviewDate,
                IntervieweePosition = modelRequest.IntervieweePosition,
                Questions = questionsContent
            };

            var html = RenderHtmlViewToString(model);
            var ironPdfRender = new IronPdf.ChromePdfRenderer();
            using var pdfDoc = ironPdfRender.RenderHtmlAsPdf(html);
            Response.Headers.Add("Content-Disposition", "attachment;filename=interview-questions.pdf");
            return File(pdfDoc.Stream.ToArray(), "application/pdf");
        }
        catch (Exception ex)
        {
            return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private string RenderHtmlViewToString(PdfExporterModel model)
    {
        var htmlPdf = System.IO.File.ReadAllText("../InterviewHelper.Core/Helper/PdfBaseTemplate.html",
            System.Text.Encoding.UTF8);
        var questionItem = "<tr ><td valign={0} style={1}>{2}</td></tr>";
        var questionsBlock = "";

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