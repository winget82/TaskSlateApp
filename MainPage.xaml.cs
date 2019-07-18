using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Runtime.Serialization;
using Windows.Storage.Streams;
using Windows.ApplicationModel.Resources;
using System.Globalization;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TaskSlateApp
{
    /// <summary>
    /// An app for keeping track of tasks to do for one to many people on the same app, no login required
    /// Currently not set up for phone or tablet use
    /// </summary>

    public sealed partial class MainPage : Page
    {
        public static List<Person> slateUsers = new List<Person>() { };

        //Global variables for temporary storage and flow of logic
        public static string buttonFlow;
        public static DateTime futureAlarmDate;
        public static DateTime futureAlarmDayAndTime;
        public static bool addDateFromCalendar = false;

        public MainPage()
        {
            this.InitializeComponent();

            //Add a connection to the resource file
            var loader = new ResourceLoader();

            //Use loader to grab strings base on their "Name" value in the resource file
            var addTaskLocalizedText = loader.GetString("welcome");
            var currentCultureLocalizedText = loader.GetString("currentCultureLocalized");
            //https://docs.microsoft.com/en-us/windows/uwp/app-resources/localize-strings-ui-manifest

            CultureInfo currentCulture = CultureInfo.CurrentCulture;//get current culture information of user
            //https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.currentculture?view=netframework-4.8

            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 1);
            dtClockTime.Tick += new EventHandler<object>(DtClockTime_Tick);
            dtClockTime.Start();
            
            DispatcherTimer alarmClockTime = new DispatcherTimer();
            alarmClockTime.Interval = new TimeSpan(0, 0, 10);//alarm will start within 10 seconds of selected time
            //and play for 10 seconds before repeating until minute changes to no longer match
            alarmClockTime.Tick += new EventHandler<object>(AlarmClockTime_Tick);
            alarmClockTime.Start();
            
            ReadPersonObjects();
            ClearScreen();
            
            InitialLoad(addTaskLocalizedText, currentCulture, currentCultureLocalizedText);
        }

        private void InitialLoad(string title, CultureInfo currentCulture, string currentCultureLocalizedText)
        {
            PersonAndDate.Text = "";
            MainTextBlock.Visibility = Visibility.Visible;
            CollapseAddRemoveButtons();
            MainTextBlock.Text = title + "\n\n" + currentCultureLocalizedText + "\n" + currentCulture.Name;
        }

        private void PlayAlarm(DateTime currentTime, DateTime taskAlarmDateTime, Uri soundSourceFileLocation)
        {
            if (currentTime.Year == taskAlarmDateTime.Year && currentTime.Month == taskAlarmDateTime.Month
            && currentTime.Day == taskAlarmDateTime.Day && currentTime.Hour == taskAlarmDateTime.Hour
            && currentTime.Minute == taskAlarmDateTime.Minute)
            {
                //play sound looping until click button to stop
                SoundPlayer.Source = soundSourceFileLocation;

                //you can stop alarm loop by checking task
            }
        }

        private void ShowTaskList(List<Task> tasks)//need to pass active person.tasks in here
        {
            //task check boxes need to be generated dynamically
            //the code below adds a checkbox for each task in the tasklist
            //and an alarm button on the side of it
            ClearScreen();
            ButtonStackPanel.Visibility = Visibility.Collapsed;
            ShowAddRemoveButtons();
            AddButtonText.Text = "Add Task";
            RemoveButtonText.Text = "Remove Checked Task(s)";
            RefreshTaskAlarmCheckboxList(tasks);
            RefreshTaskAlarmButtonsList(tasks);
        }

        private void RefreshTaskAlarmCheckboxList(List<Task> tasks)
        {
            CheckBoxStackPanel.Children.Clear();
            foreach (Task task in tasks)
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Name = task.TaskName;
                checkbox.Content = task.TaskName;
                checkbox.Height = 31;
                checkbox.FontFamily = new FontFamily("Segoe UI");
                checkbox.FontSize = 20;
                checkbox.Checked += TaskCheckBox_Checked;

                CheckBoxStackPanel.Children.Add(checkbox);
            }
        }

        private void RefreshTaskAlarmButtonsList(List<Task> tasks)
        {
            TaskAlarmsStackPanel.Children.Clear();
            foreach (Task task in tasks)
            {
                Button button = new Button();
                button.Name = task.TaskName;
                button.Content = task.AlarmTime;
                button.Height = 32;
                button.Foreground = new SolidColorBrush(Colors.White);
                button.Background = this.Resources["ButtonGradient"] as LinearGradientBrush;
                button.Click += new RoutedEventHandler(TaskAlarmButton_Click);

                TaskAlarmsStackPanel.Children.Add(button);
            }
        }

        private void RefreshPersonButtonsList(List<Person> slateUsers)
        {
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

        private void ClearScreen()
        {
            CheckBoxStackPanel.Children.Clear();
            ButtonStackPanel.Children.Clear();
            TaskAlarmsStackPanel.Children.Clear();
            
            TaskSlateCalendar.Visibility = Visibility.Collapsed;
            AddTextRelativePanel.Visibility = Visibility.Collapsed;
            MainTextBlock.Visibility = Visibility.Collapsed;
            AlarmTimePickerGrid.Visibility = Visibility.Collapsed;
            MainTextBlock.Visibility = Visibility.Collapsed;
            CalendarAlarmTimePickerGrid.Visibility = Visibility.Collapsed;
            AlarmSettingsStackPanel.Visibility = Visibility.Collapsed;
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
        
        private void DtClockTime_Tick(object sender, object e)
        {
            DateTime currentTime = DateTime.Now;
            CurrentTimeText.Text = currentTime.ToString("hh:mm tt");

            //if person in person list is active person than put name at top of screen         
            foreach (Person person in slateUsers)
            {
                if (person.IsActivePerson)
                {
                    PersonAndDate.Text = person.Name.ToString() + " - " + DateTime.Now.ToString("G");//Get a string that displays the date and time in the current culture's short date and time format
                    //https://docs.microsoft.com/en-us/dotnet/api/system.datetime.tostring?view=netframework-4.8
                }
            }
        }

        private void AlarmClockTime_Tick(object sender, object e)
        {
            DateTime currentTime = DateTime.Now;

            foreach (Person person in slateUsers)
            {
                //Alarm to go off whether person is active or not
                foreach (Task task in person.Tasks)
                {
                    if (task.AlarmSet)
                    {
                        PlayAlarm(currentTime, task.AlarmDateTime, person.AlarmFileSetting);
                    }
                }
            }
        }
        
        public void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ClearScreen();
            CheckBoxStackPanel.Visibility = Visibility.Visible;
            ShowAddRemoveButtons();

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

            RefreshPersonButtonsList(slateUsers);
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            ClearScreen();
            TaskSlateCalendar.Visibility = Visibility.Visible;
            CollapseAddRemoveButtons();
        }

        private void AlarmButton_Click(object sender, RoutedEventArgs e)
        {
            ClearScreen();
            CollapseAddRemoveButtons();
            AlarmSettingsStackPanel.Visibility = Visibility.Visible;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (addDateFromCalendar)
            {
                ClearScreen();
                AddTextRelativePanel.Visibility = Visibility.Visible;
            }
            else
            {
                AddTextRelativePanel.Visibility = Visibility.Visible;
            }            
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
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
                                if (checkBox.IsChecked ?? false)
                                {
                                    //remove task with task.name equal to checkBox.name from personTasksCopy
                                    foreach (Task task in personTasksCopy)
                                    {
                                        if ((checkBox.IsChecked ?? false) && checkBox.Name.Equals(task.TaskName))
                                        {
                                            person.Tasks.Remove(task);
                                        }
                                    }
                                }
                            }
                            ShowTaskList(person.Tasks);
                        }
                    }
                }
            }
            //regenerate list of users if buttonstackpanel visible
            if (ButtonStackPanel.Visibility == Visibility.Visible)
            {
                ClearScreen();
                ShowAddRemoveButtons();
                RefreshPersonButtonsList(slateUsers);

                //set new active person to index[0] of slate users if any person left
                if (slateUsers.Count >= 1)
                {
                    slateUsers[0].IsActivePerson = true;
                }
            }
            WritePersonObjects();
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
                    PersonAndDate.Text = senderButtonName + " - " + DateTime.Now.ToString("G");
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

            //clear text in addtextentrybox
            AddTextEntryBox.Text = clearText;
                        
            if (addDateFromCalendar)
            {
                //add task to person.Tasks if that person is active
                foreach (Person person in slateUsers)
                {
                    if (person.IsActivePerson)
                    {
                        Task task = new Task(text);
                        task.AlarmDateTime = futureAlarmDayAndTime;
                        task.AlarmTime = futureAlarmDayAndTime.ToString("MM/dd" + ", " + "hh:mm tt");
                        task.AlarmSet = true;
                        person.Tasks.Add(task);
                        //refresh tasks
                        ShowTaskList(person.Tasks);
                    }
                }
                addDateFromCalendar = false;
            }
            else if (ButtonStackPanel.Visibility == Visibility.Visible)
            {
                //add person to slateUsers
                Person personNew = new Person(text);
                slateUsers.Add(personNew);

                //refresh person buttons
                ClearScreen();
                ShowAddRemoveButtons();
                RefreshPersonButtonsList(slateUsers);
            }
            else if (CheckBoxStackPanel.Visibility == Visibility.Visible)
            {
                //add task to person.Tasks if that person is active
                foreach (Person person in slateUsers)
                {
                    if (person.IsActivePerson)
                    {
                        Task task = new Task(text);
                        person.Tasks.Add(task);
                        //refresh tasks
                        ShowTaskList(person.Tasks);
                    }
                }
            }            
            AddTextEntryBox.Visibility = Visibility.Visible;
            AddTextEntryButton.Visibility = Visibility.Visible;
            WritePersonObjects();
        }

        private void TaskAlarmButton_Click(object sender, RoutedEventArgs e)
        {
            AlarmTimePickerGrid.Visibility = Visibility.Visible;
            buttonFlow = (sender as Button).Name;
            TimePicker alarmTimePicker = new TimePicker();
        }

        private void AlarmTimePickerButton_Click(object sender, RoutedEventArgs e)
        {
            TaskSlateCalendar.Visibility = Visibility.Collapsed;

            //get time from time picker
            TimeSpan timeSpan = AlarmTimePicker.SelectedTime.Value;
            DateTime time = DateTime.Today.Add(timeSpan);
            string selectedTimeString = time.ToString("hh:mm tt");

            foreach (Button button in TaskAlarmsStackPanel.Children)
            {
                foreach (Person person in slateUsers)
                {
                    if (person.IsActivePerson)
                    {
                        //search through active person's task list for match to alarmTaskName
                        foreach (Task task in person.Tasks)
                        {
                            //button name gets reset upon initial opening
                            //This is why buttonFlow is used to designate the last button clicked
                            if (buttonFlow.Equals(task.TaskName))//if the green alarm button's name matches the task name
                            {
                                task.AlarmTime = selectedTimeString;
                                task.AlarmDateTime = time;
                                task.AlarmSet = true;
                            }
                        }                        
                        ClearScreen();
                        ShowTaskList(person.Tasks);
                    }
                }
            }
            WritePersonObjects();
        }

        private void CalendarAlarmTimePickerButton_Click(object sender, RoutedEventArgs e)
        {

            TimeSpan timeSpan = CalendarAlarmTimePicker.SelectedTime.Value;
            DateTime time = DateTime.Today.Add(timeSpan);
            string selectedTimeString = time.ToString("hh:mm tt");
            CalendarTextBlock2.Text = "at " + selectedTimeString + ".";

            //Add time span to global variable that was acquired by Calendar_SelectedDatesChanged
            futureAlarmDayAndTime = futureAlarmDate.Add(timeSpan);
            
            AddButtonText.Text = "Add Task";
            AddButton.Visibility = Visibility.Visible;
            AddButtonText.Visibility = Visibility.Visible;

            addDateFromCalendar = true;
        }

        private void Calendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            //https://www.tutorialspoint.com/xaml/xaml_calender.htm
            var calendar = (sender as CalendarView);

            // ... See if a date is selected.
            if (calendar.SelectedDates.Count != 0)
            {
                CalendarAlarmTimePickerGrid.Visibility = Visibility.Visible;

                //Get the selected date 
                DateTimeOffset dateTimeOffset = calendar.SelectedDates[0];
                DateTime datePicked = dateTimeOffset.DateTime;
                futureAlarmDate = datePicked.Date;

                //turn on timepicker                                
                TimePicker alarmTimePicker = new TimePicker();                
                
                //https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
                string selectedDateString = datePicked.ToString("D");
                CalendarTextBlock1.Text = "Your selected date is: " + selectedDateString + ",";                
            }            
        }

        private void TaskCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (Person person in slateUsers)
            {
                if (person.IsActivePerson)
                {
                    foreach (CheckBox checkBox in CheckBoxStackPanel.Children)
                    {
                        foreach (Task task in person.Tasks)
                        {
                            if ((checkBox.IsChecked ?? false) && checkBox.Name.Equals(task.TaskName) && !task.AlarmTime.Equals("OFF"))
                            {
                                //stop alarm playing ringtone at end of ringtone
                                task.AlarmSet = false;
                                task.AlarmTime = "OFF";
                            }
                            foreach (Button button in TaskAlarmsStackPanel.Children)
                            {
                                if (checkBox.Name.Equals(button.Name) && button.Name.Equals(task.TaskName) && task.AlarmTime.Equals("OFF"))
                                {
                                    //stop alarm playing ringtone at end of ringtone
                                    button.Content = "OFF";                                                                       
                                }
                            }
                        }
                    }
                    RefreshTaskAlarmButtonsList(person.Tasks);
                }
            }
            WritePersonObjects();
        }

        private void TaskCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (Person person in slateUsers)
            {                
                if (person.IsActivePerson)
                {
                    ClearScreen();
                    ShowTaskList(person.Tasks);
                }
            }
            WritePersonObjects();
        }

        private void MacGuyver_Click(object sender, RoutedEventArgs e)
        {
            foreach (Person person in slateUsers)
            {
                if (person.IsActivePerson)
                {
                    person.AlarmFileSetting = new Uri("ms-appx:///Assets/macguyver.mp3");
                }
            }
        }

        private void ZorasDomain_Click(object sender, RoutedEventArgs e)
        {
            foreach (Person person in slateUsers)
            {
                if (person.IsActivePerson)
                {
                    person.AlarmFileSetting = new Uri("ms-appx:///Assets/zoras_domain.mp3");
                }
            }
        }

        private void Tropical_Click(object sender, RoutedEventArgs e)
        {
            foreach (Person person in slateUsers)
            {
                if (person.IsActivePerson)
                {
                    person.AlarmFileSetting = new Uri("ms-appx:///Assets/tropical_iphone.mp3");
                }
            }
        }

        public async void WritePersonObjects()
        {
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

        public async void ReadPersonObjects()
        {
            slateUsers.Clear();
            List<Person> slateUsersReadList = new List<Person>();
            var Serializer = new DataContractSerializer(typeof(List<Person>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("SlateUsers"))
            {
                slateUsersReadList = (List<Person>)Serializer.ReadObject(stream);
            }

            slateUsers.AddRange(slateUsersReadList);

            if (slateUsers.Count != 0)
            {
                for (int i=0; i < slateUsers.Count; i++)
                {
                    if (i==0)
                    {
                        slateUsers[i].IsActivePerson = true;
                    }
                    else
                    {
                        slateUsers[i].IsActivePerson = false;
                    }
                }
            }
            else if (slateUsers.Count == 0)
            {
                //Start off with default person with a default task - if no person exists
                //upon load during start of app - who could then be renamed by rename button
                Person defaultPerson = new Person("Default Person");

                defaultPerson.IsActivePerson = true;

                Task defaultTask = new Task("Task1");

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
        [DataMember]
        public Uri AlarmFileSetting { get; set; }

        public Person(string name, List<Task> tasks = null, bool activePerson=false)
        {
            Name = name;
            Tasks = tasks ?? new List<Task>();//set constructor to generate a new empty task list
            IsActivePerson = activePerson;
            AlarmFileSetting = new Uri("ms-appx:///Assets/macguyver.mp3");
        }
    }

    [DataContract]
    public class Task
    {
        [DataMember]
        public string TaskName { get; set; }
        
        [DataMember]
        public bool IsComplete { get; set; }
        //reocurrence could add new instances of object

        [DataMember]
        public string AlarmTime { get; set; }

        [DataMember]
        public DateTime AlarmDateTime { get; set; }

        [DataMember]
        public bool AlarmSet { get; set; }

        public Task(string taskName, string alarmTime = "OFF", bool isCompleted=false, bool alarmSet=false)
        {
            TaskName = taskName;
            IsComplete = isCompleted;
            AlarmTime = alarmTime;
            AlarmSet = alarmSet;            
        }
    }
}

//UNICODE SUPPORT:
//https://www.codeproject.com/Articles/885262/Reading-and-writing-Unicode-data-in-NET
//".NET framework has a built-in support for Unicode characters; the char object in .NET framework represents a unicode character; UTF-16."