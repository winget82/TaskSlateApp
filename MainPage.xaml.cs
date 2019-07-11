﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Windows.Storage.Streams;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TaskSlateApp
{
    /// <summary>
    /// An app for keeping track of tasks to do for one to many people on the same app, no login required
    /// </summary>

    public sealed partial class MainPage : Page
    {

        public static List<Person> slateUsers = new List<Person>() { };
        public static string defaultAlarm = "12:00 PM";

        public MainPage()
        {
            this.InitializeComponent();

            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1);
            dtClockTime.Tick += new EventHandler<object>(DtClockTime_Tick);
            dtClockTime.Start();

            readPersonObjects();
            ClearScreen();
            CheckBoxStackPanel.Visibility = Visibility.Visible;
            InitialLoad();
        }

        public void InitialLoad()
        {
            PersonAndDate.Text = "";
            MainTextBlock.Visibility = Visibility.Visible;
            CollapseAddRemoveButtons();
            MainTextBlock.Text = "Welcome to TaskSlate!!!";//Here's a string to globalize for requirement
        }

        private void ShowTaskList(List<Task> Tasks)//need to pass active person.tasks in here
        {
            //task check boxes need to be generated dynamically
            //the code below adds a checkbox for each task in the tasklist
            ClearScreen();
            ButtonStackPanel.Visibility = Visibility.Collapsed;
            ShowAddRemoveButtons();
            AddButtonText.Text = "Add Task";
            RemoveButtonText.Text = "Remove Checked Task(s)";

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

                //need to find a way to adjust the padding, justification, etc. in the stackpanel for each checkbox
                //in the styling - SAVE FOR LAST

                Button button = new Button();
                button.Content = task.AlarmTime;
                button.Height = 32;
                button.Foreground = new SolidColorBrush(Colors.White);
                button.Background = this.Resources["ButtonGradient"] as LinearGradientBrush;
                button.Click += new RoutedEventHandler(TaskAlarmButton_Click);

                TaskAlarmsStackPanel.Children.Add(button);
            }
        }

        public void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ClearScreen();
            CheckBoxStackPanel.Visibility = Visibility.Visible;

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
            CheckBoxStackPanel.Visibility = Visibility.Collapsed;
            ButtonStackPanel.Visibility = Visibility.Visible;
            ShowAddRemoveButtons();
            AddButtonText.Text = "Add Person";
            RemoveButtonText.Text = "Remove Active Person";

            foreach (Person person in slateUsers)
            {
                Button button = new Button();
                button.Name = person.Name;
                button.Content = person.Name;
                button.Height = 32;
                button.MinWidth = 340;
                button.Foreground = new SolidColorBrush(Colors.White);
                button.Background = this.Resources["ButtonGradient"] as LinearGradientBrush;
                button.Click += new RoutedEventHandler(ActiveUserButton_Click);

                ButtonStackPanel.Children.Add(button);

                //IF NO ONE EXISTS AT OPENING OF PROGRAM
                //Start off with default person - who could then be renamed by rename button

                //Add button to person menu to rename person, add button to person menu to delete person
                //RENAME button is extra functionallity save it for last
            }
        }

        private void ClearScreen()
        {
            CheckBoxStackPanel.Children.Clear();
            ButtonStackPanel.Children.Clear();
            TaskAlarmsStackPanel.Children.Clear();
            TaskSlateCalendar.Visibility = Visibility.Collapsed;
            AddTextRelativePanel.Visibility = Visibility.Collapsed;
            MainTextBlock.Visibility = Visibility.Collapsed;
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
            /*
            CheckBox senderButtonName = (sender as CheckBox);
            //NOT WORKING - EXTRA FUNCTIONALITY SAVE FOR LAST
            senderButtonName.Content = TextDecorations.Strikethrough;
            //need a way to access the text block "CheckBoxText" within the check box "TaskCheckBox"
            //of each of the generated checkboxes
            */
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //this is for the add button
            AddTextRelativePanel.Visibility = Visibility.Visible;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            //this is for the remove button

            //copy slateUsers list to another list to enumerate in foreach statement and still be able
            //to modify slateUsers list
            List<Person> slateUsersCopy = new List<Person>(slateUsers);

            foreach (Person person in slateUsersCopy)
            {
                if (person.IsActivePerson)
                {
                    if (ButtonStackPanel.Visibility == Visibility.Visible)
                    {
                        //remove person from slateUsers
                        slateUsers.Remove(person);
                    }
                    else
                    {
                        if (CheckBoxStackPanel.Visibility == Visibility.Visible)
                        {
                            //make copy of person's task list so can enumerate through it and still be able
                            //to modify original person.Tasks list
                            List<Task> personTasksCopy = new List<Task>(person.Tasks);

                            foreach (CheckBox checkBox in CheckBoxStackPanel.Children)
                            {
                                if (checkBox.IsChecked == true)
                                {
                                    //remove task with task.name equal to checkBox.name from personTasksCopy
                                    foreach (Task task in personTasksCopy)
                                    {
                                        if (checkBox.IsChecked == true && checkBox.Name.Equals(task.TaskName))
                                        {
                                            person.Tasks.Remove(task);
                                        }
                                    }
                                    //regenerate checkboxes 
                                    ShowTaskList(person.Tasks);
                                }
                            }
                        }
                    }
                }
            }

            //regenerate list of users if buttonstackpanel visible
            if (ButtonStackPanel.Visibility == Visibility.Visible)
            {
                ClearScreen();
                ShowAddRemoveButtons();

                foreach (Person person in slateUsers)
                {
                    Button button = new Button();
                    button.Name = person.Name;
                    button.Content = person.Name;
                    button.Height = 32;
                    button.MinWidth = 340;
                    button.Foreground = new SolidColorBrush(Colors.White);
                    button.Background = this.Resources["ButtonGradient"] as LinearGradientBrush;
                    button.Click += new RoutedEventHandler(ActiveUserButton_Click);

                    ButtonStackPanel.Children.Add(button);
                }
                //set new active person to index[0] of slate users if any person left
                if (slateUsers.Count >= 1)
                {
                    slateUsers[0].IsActivePerson = true;
                }
            }
            writePersonObjects();
        }

        private void ActiveUserButton_Click(object sender, RoutedEventArgs e)
        {
            //if the name of person on button matches the name of person in person list, that person is active
            string senderButtonName = (sender as Button).Name.ToString();//get the name property of the dynamically generated button clicked

            foreach (Person person in slateUsers)
            {
                if (senderButtonName.Equals(person.Name.ToString()))//(name of person on button clicked matches the name of a person in person list)
                {
                    person.IsActivePerson = true;
                    PersonAndDate.Text = senderButtonName + " - " + DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    person.IsActivePerson = false;
                }
            }
        }

        private void AddTextEntryButton_Click(object sender, RoutedEventArgs e)
        {
            string clearText = "";

            //get text input from AddTextEntryBox upon click
            string text = AddTextEntryBox.Text;

            //WILL NEED TO FIX BUG HERE - PREVENT USING THE SAME NAME TWICE
            //BECAUSE WHEN REMOVED, IT WILL REMOVE BOTH SINCE NAMED THE SAME

            //clear text in addtextentrybox
            AddTextEntryBox.Text = clearText;

            //or add it to slateUsers if adding a person
            if (ButtonStackPanel.Visibility == Visibility.Visible)
            {
                //add person to slateUsers
                Person personNew = new Person(text);
                slateUsers.Add(personNew);

                //refresh person buttons
                ClearScreen();
                ShowAddRemoveButtons();

                foreach (Person person in slateUsers)
                {
                    Button button = new Button();
                    button.Name = person.Name;
                    button.Content = person.Name;
                    button.Height = 32;
                    button.MinWidth = 340;
                    button.Foreground = new SolidColorBrush(Colors.White);
                    button.Background = this.Resources["ButtonGradient"] as LinearGradientBrush;
                    button.Click += new RoutedEventHandler(ActiveUserButton_Click);

                    ButtonStackPanel.Children.Add(button);
                }

            }
            else if (CheckBoxStackPanel.Visibility == Visibility.Visible)
            {
                //add task to person.Tasks if that person is active
                foreach (Person person in slateUsers)
                {
                    if (person.IsActivePerson)
                    {
                        Task task = new Task(text, defaultAlarm);
                        person.Tasks.Add(task);
                        //refresh tasks
                        ShowTaskList(person.Tasks);
                    }
                }
            }
            //at end of this function change visibility of AddTextEntryButton and AddTextEntryBox back to collapsed
            AddTextEntryBox.Visibility = Visibility.Visible;
            AddTextEntryButton.Visibility = Visibility.Visible;
            writePersonObjects();
        }

        private void TaskAlarmButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public async void writePersonObjects()
        {
            /*
            List<WebUrls> objUrlsList = new List<WebUrls>();
            objUrlsList.Add(new WebUrls { Url_ID = "1", UrlName = "http://bing.com" });
            objUrlsList.Add(new WebUrls { Url_ID = "0", UrlName = "http://microsoft.com" });
            objUrlsList.Add(new WebUrls { Url_ID = "2", UrlName = "http://bsubramanyamraju.blogspot.in/" });
            */

            StorageFile slateUsersFile = await ApplicationData.Current.LocalFolder.CreateFileAsync
                ("SlateUsers", CreationCollisionOption.ReplaceExisting);

            IRandomAccessStream raStream = await slateUsersFile.OpenAsync(FileAccessMode.ReadWrite);

            using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
            {
                // Serialize the Session State
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<Person>));

                serializer.WriteObject(outStream.AsStreamForWrite(), slateUsers);

                await outStream.FlushAsync();
                outStream.Dispose();
                raStream.Dispose();
            }
        }

        public async void readPersonObjects()
        {
            slateUsers.Clear();
            List<Person> slateUsersReadList = new List<Person>();
            var Serializer = new DataContractSerializer(typeof(List<Person>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("SlateUsers"))

            {
                slateUsersReadList = (List<Person>)Serializer.ReadObject(stream);
            }
            //slateUsers.Clear();         
            slateUsers.AddRange(slateUsersReadList);

            if (slateUsers.Count != 0)
            {
                slateUsers[0].IsActivePerson = true;
            }
            else if (slateUsers.Count == 0)
            {
                //Start off with default person with a default task - if no person exists
                //upon load during start of app - who could then be renamed by rename button
                Person defaultPerson = new Person("Default Person");

                defaultPerson.IsActivePerson = true;

                Task defaultTask = new Task("Task1", defaultAlarm);

                List<Task> defaultTaskList = new List<Task>() { defaultTask };

                //Add default task list to task list in person object
                defaultPerson.Tasks.AddRange(defaultTaskList);
                slateUsers.Add(defaultPerson);
                //ShowTaskList(defaultPerson.Tasks);
            }
        }
    }

    [DataContract]
    public class Person
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<Task> Tasks;

        [DataMember]
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

    [DataContract]
    public class Task
    {
        [DataMember]
        public string TaskName { get; set; }
        

        [DataMember]
        public bool IsComplete { get; set; }
        //reocurrence could add new instances of object?

        [DataMember]
        public string AlarmTime { get; set; }

        [DataMember]
        public bool AlarmSet { get; set; }

        public Task(string taskName, string alarmTime, bool isCompleted=false, bool alarmSet=false)
        {
            TaskName = taskName;
            IsComplete = isCompleted;
            AlarmTime = alarmTime;
            AlarmSet = alarmSet;
        }
    }
}
