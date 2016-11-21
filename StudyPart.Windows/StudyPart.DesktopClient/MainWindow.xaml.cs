using System;
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
            AdjustListViewColumns(GroupsLV, SubjectsLV, StudentsLV, MarksLV, AccountsLV, DepartmentsLV, SpecialtiesLV, TeachersLV);
            SetGridCells(5, 2, GroupEditorGrid);
            SetGridCells(3, 2, SpecialtyEditorGrid, TeacherEditorGrid);
            UpdateDisplayData();
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
                    Set((Group)GroupsLV.SelectedItem);
                    break;
                case nameof(SetSpecialtyButton):
                    Set((Specialty)SpecialtiesLV.SelectedItem);
                    break;
                case nameof(SetTeacherButton):
                    Teacher teacher = (Teacher)TeachersLV.SelectedItem;
                    teacher.FullName = TeacherNameTB.Text;
                    Set(teacher);
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
                    Remove((Group)GroupsLV.SelectedItem);
                    break;
                case nameof(RemoveSpecialtyButton):
                    Remove((Specialty)SpecialtiesLV.SelectedItem);
                    break;
                case nameof(RemoveTeacherButton):
                    Remove((Teacher)TeachersLV.SelectedItem);
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

        private void LVSelection_Changed(object sender, SelectionChangedEventArgs e)
        {
            SetButtons();
            ListView senderLV = ((ListView)sender);
            if (senderLV.SelectedIndex > -1)
                switch (senderLV.Name)
                {
                    case nameof(GroupsLV):
                        break;
                    case nameof(SpecialtiesLV):
                        break;
                    case nameof(TeachersLV):
                        TeacherNameTB.Text = ((Teacher)TeachersLV.SelectedItem).FullName;
                        break;
                }
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
            FillComboBoxes(GroupDepartmentCB, GetGroups().ToArray(),
                GroupSpecialtyCB, GetSpecialties().ToArray());
            SetButtons();
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

        void FillComboBoxes(params object[] comboBoxesAndValues)
        {
            if (comboBoxesAndValues.Length % 2 != 0)
                throw new ArgumentException("Parameters should be like comboBox1, valueCollection1, comboBox2, valueCollection2...");
            for (int i = 0; i < comboBoxesAndValues.Length; i += 2)
            {
                ComboBox currentListView = (ComboBox)comboBoxesAndValues[i];
                currentListView.Items.Clear();
                foreach (var current in ((Array)comboBoxesAndValues[i + 1]))
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

        void SetGridCells(int columns, int rows, params Grid[] grids)
        {
            foreach (var grid in grids)
            {
                for (int i = 0; i < columns; i++)
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = i >= columns - 2 ? GridLength.Auto : new GridLength(1, GridUnitType.Star) });
                for (int i = 0; i < rows; i++)
                    grid.RowDefinitions.Add(new RowDefinition());
            }
        }

        void SetButtons()
        {
            bool groupValidated = Validate(typeof(Group));
            bool groupSelected = GroupsLV.SelectedIndex > -1;
            AddGroupButton.IsEnabled = groupValidated;
            SetGroupButton.IsEnabled = groupValidated && groupSelected;
            RemoveGroupButton.IsEnabled = groupSelected;

            bool specialtyValidated = Validate(typeof(Specialty));
            bool specialtySelected = SpecialtiesLV.SelectedIndex > -1;
            AddSpecialtyButton.IsEnabled = specialtyValidated;
            SetSpecialtyButton.IsEnabled = specialtyValidated && specialtySelected;
            RemoveSpecialtyButton.IsEnabled = specialtySelected;

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
                case nameof(Specialty):
                    return !string.IsNullOrWhiteSpace(SpecialtyNameTB.Text);
                case nameof(Teacher):
                    return !string.IsNullOrWhiteSpace(TeacherNameTB.Text);
                default:
                    return false;
            }
        }
    }
}
