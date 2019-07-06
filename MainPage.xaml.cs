using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
        //TEMPORARY DEFAULTS

        //TEMPORARY Tasks
        public static Task defaultTask = new Task("Sweep", false);
        public static Task defaultTask2 = new Task("Dishes", false);
        public static Task defaultTask3 = new Task("Bathroom", false);
        public static Task defaultTask4 = new Task("Make Bed", false);

        //TEMPORARY Task List
        public static List<Task> defaultTaskList = new List<Task>() { defaultTask, defaultTask2, defaultTask3, defaultTask4 };

        //TEMPORARY Persons / Users
        public static Person defaultPerson = new Person("default1", defaultTaskList);
        public static Person defaultPerson2 = new Person("default2", defaultTaskList);
        public static Person defaultPerson3 = new Person("default3", defaultTaskList);
        public static List<Person> slateUsers = new List<Person>() { defaultPerson, defaultPerson2, defaultPerson3 };

        public static string activePerson = defaultPerson.Name.ToString();      
        
        public MainPage()
        {
            this.InitializeComponent();

            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1);
            dtClockTime.Tick += new EventHandler<object>(DtClockTime_Tick);
            dtClockTime.Start();

            CheckBoxStackPanel.Children.Clear();
            ButtonStackPanel.Children.Clear();
            TaskSlateCalendar.Visibility = Visibility.Collapsed;

            PersonAndDate.Text = activePerson + " - " + DateTime.Now.ToString("MM/dd/yyyy");

            //Time is not updating...
            //CurrentTimeText.Text = DateTime.Now.ToString("hh:mm tt");

            //Start off with default person - who could then be renamed by rename button
            //Add button to person menu to rename person, add button to person menu to delete person
            //- make unable to delete person if only one person remains

            //the code below adds a checkbox for each task in the tasklist, but settings need setup
            //for spacing font, size, etc. utilize STACKPANEL for this

            foreach (Task task in defaultTaskList)
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Name = task.TaskName;
                checkbox.Content = task.TaskName;
                checkbox.Height = 31;
                checkbox.FontFamily = new FontFamily("Segoe UI");
                checkbox.FontSize = 20;
                checkbox.Checked += TaskCheckBox_Checked;
                
                CheckBoxStackPanel.Children.Add(checkbox);
                //need to find a way to adjust the padding, justification, etc. in the stackpanel for each button
            }
        }

        //task check boxes need to be generated dynamically

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
            CheckBoxStackPanel.Children.Clear();
            ButtonStackPanel.Children.Clear();
            TaskSlateCalendar.Visibility = Visibility.Collapsed;
            AddButton.Visibility = Visibility.Visible;
            AddButtonText.Visibility = Visibility.Visible;
            AddButtonText.Text = "Add Task";
            

            //What happens when HomeButton is clicked?

            foreach (Task task in defaultTaskList)
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Name = task.TaskName;
                checkbox.Content = task.TaskName;
                checkbox.Height = 31;
                checkbox.FontFamily = new FontFamily("Segoe UI");
                checkbox.FontSize = 20;
                checkbox.Checked += TaskCheckBox_Checked;

                CheckBoxStackPanel.Children.Add(checkbox);
                //need to find a way to adjust the padding, justification, etc. in the stackpanel for each button
            }
        }

        private void PersonButton_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxStackPanel.Children.Clear();
            ButtonStackPanel.Children.Clear();
            TaskSlateCalendar.Visibility = Visibility.Collapsed;
            AddButton.Visibility = Visibility.Visible;
            AddButtonText.Visibility = Visibility.Visible;
            AddButtonText.Text = "Add Person";

            //What happens when PersonButton is clicked?
            //screen changes to show user buttons from a List
            foreach (Person person in slateUsers)
            {
                Button button = new Button();
                button.Name = person.Name;
                button.Content = person.Name;
                button.Height = 32;
                button.MinWidth = 340;
                button.Foreground = new SolidColorBrush(Colors.White);
                button.Background = this.Resources["ButtonGradient"] as LinearGradientBrush;

                //button.HorizontalAlignment = new Stretch
                ButtonStackPanel.Children.Add(button);

                //Start off with default person - who could then be renamed by rename button
                //Add button to person menu to rename person, add button to person menu to delete person
                //- make unable to delete person if only one person remains
            }
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxStackPanel.Children.Clear();
            ButtonStackPanel.Children.Clear();
            TaskSlateCalendar.Visibility = Visibility.Visible;
            AddButton.Visibility = Visibility.Collapsed;
            AddButtonText.Visibility = Visibility.Collapsed;

            //What happens when CalendarButton is clicked?
        }

        private void AlarmButton_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxStackPanel.Children.Clear();
            ButtonStackPanel.Children.Clear();
            TaskSlateCalendar.Visibility = Visibility.Collapsed;
            AddButton.Visibility = Visibility.Collapsed;
            AddButtonText.Visibility = Visibility.Collapsed;
            

            //What happens when AlarmButton is clicked?
        }

        private void DtClockTime_Tick(object sender, object e)
        {
            CurrentTimeText.Text = DateTime.Now.ToString("hh:mm tt");
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

//dynamically add buttons**
//https://stackoverflow.com/questions/42306705/programmatically-add-buttons-to-a-uwp-app
//https://stackoverflow.com/questions/6406868/c-sharp-add-controls-to-panel-in-a-loop

//checkbox styling
//https://stackoverflow.com/questions/49774305/how-to-change-uwp-checkbox-text-color

//Will have to be some initial page to make a user / edit a user - could start with "Default"
//issues with static keyword
//https://stackoverflow.com/questions/13162437/how-to-add-the-objects-of-a-class-in-a-static-list-property-of-same-class

//define and use resources in xaml so they can be used in C#
//https://stackoverflow.com/questions/3308868/how-to-define-and-use-resources-in-xaml-so-they-can-be-used-in-c-sharp

//AWESOME explanation for timers and events on timers
//http://www.codescratcher.com/wpf/create-timer-wpf/