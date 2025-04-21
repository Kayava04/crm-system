using System.Collections.ObjectModel;
using System.Windows.Media;

namespace CRMSystemApp.ViewModels;

public class DashboardViewModel
{
    public ObservableCollection<StudentItem> ActiveStudents { get; set; }
    public ObservableCollection<StudentItem> CompletedCourses { get; set; }

    public DashboardViewModel()
    {
        ActiveStudents = new ObservableCollection<StudentItem>
        {
            new StudentItem { Name = "Olena Ivanova", Avatar = "https://i.pravatar.cc/32?img=1", Teacher = "Ms. Carter", Group = "En-1", Stage = "Learning", StageColor = Brushes.DodgerBlue, Days = "Mon/Wed", Payment = "$1500" },
            new StudentItem { Name = "Ihor Petrenko", Avatar = "https://i.pravatar.cc/32?img=2", Teacher = "Mr. Green", Group = "Fr-2", Stage = "Trial", StageColor = Brushes.OrangeRed, Days = "Tue/Thu", Payment = "$300" }
        };

        CompletedCourses = new ObservableCollection<StudentItem>
        {
            new StudentItem { Name = "Maria Bondar", Avatar = "https://i.pravatar.cc/32?img=3", Teacher = "Ms. Carter", Group = "En-1", EndDate = "12.01.2024", Payment = "$1800" },
            new StudentItem { Name = "Andriy Koval", Avatar = "https://i.pravatar.cc/32?img=4", Teacher = "Mr. Green", Group = "Fr-2", EndDate = "22.12.2023", Payment = "$1600" }
        };
    }
}