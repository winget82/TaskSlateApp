using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TaskSlateApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
        }

        //task check boxes need to be generated dynamically, see notes at bottom of this file

        //person will be an object, and the person at top of screen will be populated by accessing that person's properties

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //this is for addbutton
            //this will generate a text box prompt for typing the name of task
            //and a box / picker for setting time of alarm
        }

        private void TaskCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Here is what happens when TaskCheckBox_Checked
            //Change font to scratched out font
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            //What happens when HomeButton is clicked?
        }

        private void PersonButton_Click(object sender, RoutedEventArgs e)
        {
            //What happens when PersonButton is clicked?
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            //What happens when CalendarButton is clicked?
        }

        private void AlarmButton_Click(object sender, RoutedEventArgs e)
        {
            //What happens when AlarmButton is clicked?
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public List<Task> Tasks;
        //AlarmSetting

        public Person(string name, List<Task> tasks)
        {
            Name = name;
            Tasks = tasks;
            //AlarmSetting
        }
    }

    public class Task
    {
        public string TaskName { get; set; }
        //datetime object for alarm reminder time and day?
        public bool IsComplete { get; set; }
        //recurrence could add new instances of object?

        public Task(string taskName, bool isCompleted)
        {
            TaskName = taskName;
            IsComplete = isCompleted;
        }
    }
}

//dynamically adding checkboxes:
//https://stackoverflow.com/questions/15005333/dynamically-adding-checkboxes-to-a-windows-form-only-shows-one-checkbox
//https://www.youtube.com/watch?v=TfE2vcDy0G8

//dynamically remove something from wup layout
//https://www.youtube.com/watch?time_continue=2&v=RB4caGumBiU

//dynamically add:
//https://www.youtube.com/watch?time_continue=158&v=ohcdseuil5E

//icons
//https://www.technical-recipes.com/2017/how-to-use-resource-files-in-your-csharp-project/
//checkbox styling
//https://stackoverflow.com/questions/49774305/how-to-change-uwp-checkbox-text-color