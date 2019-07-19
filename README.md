# TaskSlateApp
EdX Capstone Project - Windows 10 UWP App

This is a project for Edx Capstone project to finish out the Entry Level Software Development path of courses.

Task Slate App README
=====================
This is a C# / XAML UWP app for Windows 10.  See last part of readme to see explanation for each requirement and how it was met to my understanding.
I have included the original scenarios, as well as modified ones that changed with iteration and personal testing.  This is my first UWP app if you
look at all my commits, you can tell it was a learning process for me to say the least.  Thanks for taking the time to review my app.  Feel free to remove /
add users, tasks, and play around with it.  I've also included a link to a video at the bottom of the app in use on my computer to help show the functionality
of this simple app.

Note:  This app has only been written for use on Windows 10 computer currently, as I had no way to test on a phone.
Some of the design changed during design/iteration to eliminate unnecessary features, features not needed to provide solution:
such as strike through text (tasks can simply be removed one or several at a time while checkboxed),
save button was not needed (saved automatically during any changes), active user at startup (everyone's alarms will activate regardless of who is active within
10 seconds of alarm time).

How to install / installation:
------------------------------
////*****************************************ADD INSTALLATION INSTRUCTIONS HERE******************************************\\\\

If you'd like to not install this way, the code is located here if you want to clone and run yourself: https://github.com/winget82/TaskSlateApp

SCENARIOS
=========

(Original scenarios):
.....................
Zack
----
1.	Zack opens the app and it opens on Ellen’s home screen because she was the last person to use it.  Zack clicks the person icon at the top.
2.	Zack then clicks his own name to make him active and see his own task slate
3.	He clicks the check box next to Task 1 to mark it complete.  Then he clicks add to add a task.
4.	Then he populates the task and sets the reminder time.  Then clicks save to save it.
5.	Zack clicks the calendar icon to see his calendar.
6.	He clicks April 5th, but decides he doesn’t want to add a task.  He wants to see the reminder alarm settings, so he clicks the alarm icon to see the settings.
7.	He decides not to change anything and clicks the home icon to return to his current day’s task slate.

Ellen
-----
1.	Ellen picks up the phone later to find it is on Zack’s tasks, so she clicks the person icon to select a person to add a task for.
2.	She then chooses her son Joe.
3.	Joe’s home screen task slate appears, and Ellen clicks add to add a task for him.
4.	She populates the task, time of the reminder, and saves.  Then she clicks the home icon to return to Joe’s task slate to see what tasks he has.


(Modified scenarios from iteration):
************************************
Zack
----
1.	Zack opens the app and it opens on Ellen’s home screen.  Zack clicks the person icon at the top.
2.	Zack then clicks his own name to make himself active and then clicks the home button to see his own task slate
3.	He clicks the check box next to Task 1 (Change oil) to mark it complete and clear the alarm.  He clicks remove while the task is checked to remove it.
4.	Then he clicks add to add a task and populates it choosing a time and text, and sets the reminder time.
5.	Zack clicks the calendar icon to see his calendar.
6.	He clicks April 5th, picks a time, confirms the date and time below the calendar match what he wants, clicks add, then he clicks add task,
	and populates a new task.
7. 	He wants to see his reminder alarm setting, so he clicks the alarm icon to see the settings.
7.	He decides to change to the MacGuyver alarm and clicks it, and then clicks the home icon to return to his current day’s task slate.

Ellen
-----
1.	Ellen opens the app later and she clicks the person icon to select a person to add a task for.
2.	She then chooses her son Joe and clicks the home button to see his tasks.
3.	Joe’s home screen task slate appears, and Ellen clicks add to add a task for him.
4.	She populates the task and time of the reminder alarm.  Then she clicks the person icon to change to herself and then clicks the home button to see
	what tasks she has herself.



Project Requirements:
---------------------

Version 1:
----------
1. At least 2 different UI pages - 4 different page elements 1) the home/task screen 2) the person/user screen 3) calendar screen 4) alarm settings screen

2. At least 3 different user input elements - buttons, text entry, check boxes - alarm settings, person/user choice, adding/removing person/user,
adding/removing tasks, picking date from calendar and time from time pickers, etc.

3. At least 1 UI element is affected by the user's input - clicking buttons affects panels - grid panels, relative panels, stack panels, etc.
Clicking checkbox sets alarm time to OFF.  Clicking time from time picker adds to alarm time.  Clicking date on calendar affects text.  Plus more.

4. After the user submits their input a decision is made based on that input that can cause atleast 2 different results - once a user sets alarm, if the time
matches it plays an alarm, if it does not match it continues to check to see if the datetime matches.  Also if user closes app with people/users in their list,
when app is opened it will set the 0 index to be the active user. If no one is in their list, it will make a new default person object.

5. A user's information is collected together in a single code structure - all users are store as person objects, which together are all store in a list called
slateUsers which is written to a storage file when various buttons are clicked during usage of the app (basically whenever a change occurs)

Version 2:
----------
1. Fixing code smells
	a. Redundant code - eliminated to the best of my current understanding
	b. Incorrect or consisting naming conventions - followed C# conventions for PascalCase and camelCase
	c. Poor control flow - the best I could with my current understanding
	d. Entangled code - the best I could with my current understanding
	e. Left behind code - eliminated

2. Internationalization
	a. Support for UNICODE characters in all user text input - UWP supports this out of the box
	b. Date and time formats reflect the user's local settings - each page has the current user's name with the date and time showing per user's settings,
	calendar page should also reflect user settings per UWP documentation, utilizing datetime objects, timespan objects, etc
	c. At least 1 string is localizable - opening screen has a string that is localizable and tells you what your current culture setting is, uses
	resource loader and .resw files to support English, French, and German for a handful of strings.


I uploaded a youtube demo here (does not necessarily follow scenarios, but shows that the functionality is there, also doesn't show mouse pointer so hover effect
makes it appear that I am clicking on things more times than I am.  Everything is single click):
https://youtu.be/EKgRVVx0fcc

Original youtube video of paper demo here:
https://www.youtube.com/watch?v=xZqa2MluVi8&feature=youtu.be
