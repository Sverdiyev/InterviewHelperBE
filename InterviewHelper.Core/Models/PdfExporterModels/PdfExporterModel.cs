namespace InterviewHelper.Core.Models.PdfExporterModels;

public class PdfExporterModel
{
    public DateTime InterviewDate { get; set; }
    public string IntervieweePosition { get; set; }
    public List<string> Questions { get; set; }
}