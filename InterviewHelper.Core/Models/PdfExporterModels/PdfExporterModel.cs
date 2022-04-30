namespace InterviewHelper.Core.Models.PdfExporterModels;

public class PdfExporterModel
{
    public int IntervieweeId { get; set; }
    public DateTime InterviewDate { get; set; }
    public string IntervieweePosition { get; set; }
    public List<string> Questions { get; set; }
}