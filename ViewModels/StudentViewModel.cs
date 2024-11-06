using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows;

namespace Registration.ViewModels
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Student> students;
        private Student selectedStudent;
        private SqlConnection con;

        public StudentViewModel()
        {
            try
            {
                string connectionString = @"Data Source=DESKTOP-A29AI91\SQLSERVER;Initial Catalog=Student;Integrated Security=True";
                con = new SqlConnection(connectionString);
                Students = new ObservableCollection<Student>();

                LoadStudentsFromDatabase();

                AddStudentCommand = new RelayCommand(AddStudent, CanExecuteAddStudent);
                UpdateStudentCommand = new RelayCommand(UpdateStudent, CanExecuteUpdateStudent);
                DeleteStudentCommand = new RelayCommand(DeleteStudent, CanExecuteDeleteStudent);
                ClearInputFieldsCommand = new RelayCommand(ClearInputFields);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public ObservableCollection<Student> Students
        {
            get => students;
            set
            {
                if (students != value)
                {
                    students = value;
                    OnPropertyChanged(nameof(Students));
                }
            }
        }

        public Student SelectedStudent
        {
            get => selectedStudent;
            set
            {
                if (selectedStudent != value)
                {
                    selectedStudent = value;
                    OnPropertyChanged(nameof(SelectedStudent));
                }
            }
        }
        public ICommand AddStudentCommand { get; }
        public ICommand UpdateStudentCommand { get; }
        public ICommand DeleteStudentCommand { get; }
        public ICommand ClearInputFieldsCommand { get; }

        public void AddStudent(object parameter)
        {
            if (string.IsNullOrEmpty(selectedStudent?.Name) || string.IsNullOrEmpty(selectedStudent?.Email))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            var student = new Student
            {
                Name = selectedStudent.Name,
                Email = selectedStudent.Email,
                Phno = selectedStudent.Phno
            };

            try
            {
                SaveStudentToDatabase(student);
                Students.Add(student);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding student: {ex.Message}");
            }
        }
        public void UpdateStudent(object parameter)
        {
            if (SelectedStudent == null)
            {
                MessageBox.Show("No student selected.");
                return;
            }

            try
            {
                UpdateStudentInDatabase(SelectedStudent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating student: {ex.Message}");
            }
        }
        public void DeleteStudent(object parameter)
        {
            if (SelectedStudent == null)
            {
                MessageBox.Show("No student selected.");
                return;
            }

            try
            {
                DeleteStudentFromDatabase(SelectedStudent);
                Students.Remove(SelectedStudent);
                SelectedStudent = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting student: {ex.Message}");
            }
        }

        public void ClearInputFields(object parameter)
        {
            SelectedStudent = new Student();
        }
        public bool CanExecuteAddStudent(object parameter)
        {
            return true;
        }
        public bool CanExecuteUpdateStudent(object parameter)
        {
            return SelectedStudent != null;
        }

        public bool CanExecuteDeleteStudent(object parameter)
        {
            return SelectedStudent != null;
        }
        private void LoadStudentsFromDatabase()
        {
            try
            {
                con.Open();
                string query = "SELECT id, name, email, phno FROM StudRegistration";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Students.Add(new Student
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Phno = reader.GetInt32(3)
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error while loading data: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }
        private void SaveStudentToDatabase(Student student)
        {
            try
            {
                con.Open();
                string query = "INSERT INTO StudRegistration (name, email, phno) VALUES (@name, @email, @phno)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@name", student.Name);
                    cmd.Parameters.AddWithValue("@email", student.Email);
                    cmd.Parameters.AddWithValue("@phno", student.Phno);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error inserting student: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        private void UpdateStudentInDatabase(Student student)
        {
            try
            {
                con.Open();
                string query = "UPDATE StudRegistration SET name = @name, email = @email, phno = @phno WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", student.Id);
                    cmd.Parameters.AddWithValue("@name", student.Name);
                    cmd.Parameters.AddWithValue("@email", student.Email);
                    cmd.Parameters.AddWithValue("@phno", student.Phno);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error updating student: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        private void DeleteStudentFromDatabase(Student student)
        {
            try
            {
                con.Open();
                string query = "DELETE FROM StudRegistration WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", student.Id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error deleting student: {ex.Message}");
            }
            finally
            {
                con.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
