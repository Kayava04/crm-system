using System.Windows.Media;

namespace CRMSystemApp.ViewModels;

public class StudentItem
{
    public string Name { get; set; }
    public string Avatar { get; set; }
    public string Teacher { get; set; }
    public string Group { get; set; }
    public string Stage { get; set; }
    public Brush StageColor { get; set; }
    public string Days { get; set; }
    public string Payment { get; set; }
    public string EndDate { get; set; }
}