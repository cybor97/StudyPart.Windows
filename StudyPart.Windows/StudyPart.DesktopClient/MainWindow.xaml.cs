using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using StudyPart.Core.Data;
using static StudyPart.Core.Data.DBHolder;
namespace StudyPart.DesktopClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init(true);//Init DBHolder
        }

        void MainWindow_Loaded(object sender, EventArgs e)
        {
            AdjustListViewColumns(GroupsLV, SubjectsLV, StudentsLV, MarksLV, AccountsLV);
            SetGridCells(GroupEditorGrid, 5, 2);
            SetGridCells(TeacherEditorGrid, 3, 2);
            SetButtons();
        }

        void MenuButton_Click(object sender, EventArgs e)
        {
            if (sender is MenuItem)
                switch (((MenuItem)sender).Name)
                {
                    case nameof(ImportButton):
                        break;
                    case nameof(ExportButton):
                        break;
                    case nameof(ExitButton):
                        Process.GetCurrentProcess().Kill();
                        break;

                    case nameof(AboutButton):
                        break;
                }
        }

        void AddButton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case nameof(AddGroupButton):
                    break;
                case nameof(AddTeacherButton):
                    Add(new Teacher { FullName = TeacherNameTB.Text });
                    break;
                default:
                    break;
            }
            UpdateDisplayData();
        }

        void SetButton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case nameof(SetGroupButton):
                    break;
                case nameof(SetTeacherButton):
                    break;
                default:
                    break;
            }
            UpdateDisplayData();
        }

        void RemoveButton_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case nameof(RemoveGroupButton):
                    break;
                case nameof(RemoveTeacherButton):
                    break;
                default:
                    break;
            }
            UpdateDisplayData();
        }

        void RefreshButton_Click(object sender, EventArgs e)
        {
            UpdateDisplayData();
        }

        void Input_Changed(object sender, EventArgs e)
        {
            SetButtons();
        }

        void UpdateDisplayData()
        {
            FillLists(GroupsLV, GetGroups().ToArray(),
                SubjectsLV, GetSubjects().ToArray(),
                StudentsLV, GetStudents().ToArray(),
                MarksLV, GetMarks().ToArray(),
                AccountsLV, GetAccounts().ToArray(),
                DepartmentsLV, GetDepartments().ToArray(),
                SpecialtiesLV, GetSpecialties().ToArray(),
                TeachersLV, GetTeachers().ToArray());
        }

        void FillLists(params object[] listViewsAndValues)
        {
            if (listViewsAndValues.Length % 2 != 0)
                throw new ArgumentException("Parameters should be like list1, valueCollection1, list2, valueCollection2...");
            for (int i = 0; i < listViewsAndValues.Length; i += 2)
            {
                ListView currentListView = (ListView)listViewsAndValues[i];
                currentListView.Items.Clear();
                foreach (var current in ((Array)listViewsAndValues[i + 1]))
                    currentListView.Items.Add(current);
            }
        }

        void ClearListViews(params ListView[] listViews)
        {
            foreach (var current in listViews)
                current.Items.Clear();
        }

        void AdjustListViewColumns(params ListView[] listViews)
        {
            foreach (var listView in listViews)
                foreach (var current in ((GridView)listView.View).Columns)
                    current.Width = ActualWidth / ((GridView)listView.View).Columns.Count;
        }

        void SetGridCells(Grid grid, int columns, int rows)
        {
            while (columns-- > 0)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = columns < 2 ? GridLength.Auto : new GridLength(1, GridUnitType.Star) });
            while (rows-- > 0)
                grid.RowDefinitions.Add(new RowDefinition());
        }

        void SetButtons()
        {
            bool groupValidated = Validate(typeof(Group));
            bool groupSelected = GroupsLV.SelectedIndex > -1;
            AddGroupButton.IsEnabled = groupValidated;
            SetGroupButton.IsEnabled = groupValidated && groupSelected;
            RemoveGroupButton.IsEnabled = groupSelected;

            bool teacherValidated = Validate(typeof(Teacher));
            bool teacherSelected = TeachersLV.SelectedIndex > -1;
            AddTeacherButton.IsEnabled = teacherValidated;
            SetTeacherButton.IsEnabled = teacherValidated && teacherSelected;
            RemoveTeacherButton.IsEnabled = teacherSelected;
        }

        bool Validate(Type valueType)
        {
            switch (valueType.Name)
            {
                case nameof(Group):
                    return GroupDepartmentCB.SelectedIndex > -1 &&
                        GroupSpecialtyCB.SelectedIndex > -1 &&
                        !string.IsNullOrWhiteSpace(GroupNameTB.Text);
                case nameof(Teacher):
                    return !string.IsNullOrWhiteSpace(TeacherNameTB.Text);
                default:
                    return false;
            }
        }
    }
}
