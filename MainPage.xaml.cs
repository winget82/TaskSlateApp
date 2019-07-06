using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Text;
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
    /// An app for keeping track of tasks to do for one to many people on the same app, no login required
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public static List<Person> slateUsers = new List<Person>() { };    
        
        public MainPage()
        {
            this.InitializeComponent();

            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1);
            dtClockTime.Tick += new EventHandler<object>(DtClockTime_Tick);
            dtClockTime.Start();

            //Start off with default person - if no person exists upon load during start of app
            //- who could then be renamed by rename button
            Person defaultPerson = new Person("Default Person");
            slateUsers.Add(defaultPerson);
            defaultPerson.IsActivePerson = true;

            Task defaultTask = new Task("Sweep", false);
            Task defaultTask2 = new Task("Dishes", false);
            Task defaultTask3 = new Task("Bathroom", false);
            Task defaultTask4 = new Task("Make Bed", false);

            List<Task> defaultTaskList = new List<Task>() { defaultTask, defaultTask2, defaultTask3, defaultTask4 };

            //Add default task list to task list in person object
            defaultPerson.Tasks.AddRange(defaultTaskList);

            ShowTaskList(defaultPerson.Tasks);

            //Add button to person menu to rename person, add button to person menu to delete person
            //- make unable to delete person if only one person remains

        }

        private void ShowTaskList(List<Task> Tasks)//need to pass active person.tasks in here
        {
            //task check boxes need to be generated dynamically
            //the code below adds a checkbox for each task in the tasklist
            ClearScreen();
            ShowAddRemoveButtons();
            AddButtonText.Text = "Add Task";
            RemoveButtonText.Text = "Remove Task";

            foreach (Task task in Tasks)
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Name = task.TaskName;
                checkbox.Content = task.TaskName;
                //need a way to access the text block "CheckBoxText" within the check box "TaskCheckBox"
                //of each of the generated checkboxes
                checkbox.Height = 31;
                checkbox.FontFamily = new FontFamily("Segoe UI");
                checkbox.FontSize = 20;
                checkbox.Checked += TaskCheckBox_Checked;

                CheckBoxStackPanel.Children.Add(checkbox);

                //need to find a way to adjust the padding, justification, etc. in the stackpanel for each button
                //in the styling
            }
        }

        public void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            
            foreach (Person person in slateUsers)
            {
                if (person.IsActivePerson)
                {
                    ShowTaskList(person.Tasks);
                }
            }
            
        }

        private void PersonButton_Click(object sender, RoutedEventArgs e)
        {
            ClearScreen();
            ShowAddRemoveButtons();
            AddButtonText.Text = "Add Person";
            RemoveButtonText.Text = "Remove Person";

            foreach (Person person in slateUsers)
            {
                Button button = new Button();
                button.Name = person.Name;
                button.Content = person.Name;
                button.Height = 32;
                button.MinWidth = 340;
                button.Foreground = new SolidColorBrush(Colors.White);
                button.Background = this.Resources["ButtonGradient"] as LinearGradientBrush;

                ButtonStackPanel.Children.Add(button);

                //Start off with default person - who could then be renamed by rename button
                //Add button to person menu to rename person, add button to person menu to delete person
                //- make unable to delete person if only one person remains
            }
        }

        private void ClearScreen()
        {
            CheckBoxStackPanel.Children.Clear();
            ButtonStackPanel.Children.Clear();
            TaskSlateCalendar.Visibility = Visibility.Collapsed;
        }

        private void CollapseAddRemoveButtons()
        {
            AddButton.Visibility = Visibility.Collapsed;
            AddButtonText.Visibility = Visibility.Collapsed;
            RemoveButton.Visibility = Visibility.Collapsed;
            RemoveButtonText.Visibility = Visibility.Collapsed;
        }

        private void ShowAddRemoveButtons()
        {
            AddButton.Visibility = Visibility.Visible;
            AddButtonText.Visibility = Visibility.Visible;
            RemoveButton.Visibility = Visibility.Visible;
            RemoveButtonText.Visibility = Visibility.Visible;
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxStackPanel.Children.Clear();
            ButtonStackPanel.Children.Clear();
            TaskSlateCalendar.Visibility = Visibility.Visible;
            CollapseAddRemoveButtons();
        }

        private void AlarmButton_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxStackPanel.Children.Clear();
            ButtonStackPanel.Children.Clear();
            TaskSlateCalendar.Visibility = Visibility.Collapsed;
            CollapseAddRemoveButtons();
        }

        private void DtClockTime_Tick(object sender, object e)
        {
            CurrentTimeText.Text = DateTime.Now.ToString("hh:mm tt");

            //if person in person list is active person than put name at top of screen         
            foreach (Person person in slateUsers)
            {
                if (person.IsActivePerson)
                {
                    PersonAndDate.Text = person.Name.ToString() + " - " + DateTime.Now.ToString("MM/dd/yyyy");
                }
            }
                
        }

        private void TaskCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //Here is what happens when TaskCheckBox_Checked
            //Change font to strikethrough text
            
            //NOT WORKING
            CheckBoxText.TextDecorations = TextDecorations.Strikethrough;
            //need a way to access the text block "CheckBoxText" within the check box "TaskCheckBox"
            //of each of the generated checkboxes
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //this is for the add button
            //this will generate a text box prompt for typing the name of task
            //and a box / picker for setting time of alarm            
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            //this is for the remove button
            //this will let you select with text/button to remove
            //this will remove any checked tasks showing currently
        }

        private void ActiveUserButton_Click(object sender, RoutedEventArgs e)
        {
            //if the name of person on button matches the name of person in person list, that person is active
            //how do you get the name of a button generated dynamically in order to access its properties?
            /*
            foreach (Person person in slateUsers)
            {
                //https://stackoverflow.com/questions/22591756/get-values-from-dynamically-added-textboxes-asp-net-c-sharp
                if (Button..ToString == person.Name.ToString())//(name of person on button clicked matches the name of a person in person list)
                {
                    person.IsActivePerson = true;
                }
            }
            */
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public List<Task> Tasks;
        public bool IsActivePerson { get; set; }
        //AlarmSetting

        public Person(string name, List<Task> tasks = null, bool activePerson=false)
        {
            Name = name;
            Tasks = tasks ?? new List<Task>();//set constructor to generate a new empty task list
            IsActivePerson = activePerson;
            //AlarmSetting
        }
    }

    public class Task
    {
        public string TaskName { get; set; }
        //datetime object for alarm reminder time and day?
        public bool IsComplete { get; set; }
        //recurrence could add new instances of object?

        public Task(string taskName, bool isCompleted=false)
        {
            TaskName = taskName;
            IsComplete = isCompleted;
        }
    }

}

//checkbox styling
//https://stackoverflow.com/questions/49774305/how-to-change-uwp-checkbox-text-color

//Will have to be some initial page to make a user / edit a user - could start with "Default"
//issues with static keyword
//https://stackoverflow.com/questions/13162437/how-to-add-the-objects-of-a-class-in-a-static-list-property-of-same-class

//define and use resources in xaml so they can be used in C#
//https://stackoverflow.com/questions/3308868/how-to-define-and-use-resources-in-xaml-so-they-can-be-used-in-c-sharp

//AWESOME explanation for timers and events on timers
//http://www.codescratcher.com/wpf/create-timer-wpf/
