using System;
using System.Windows;
using Registration.ViewModels;

namespace Registration.Views
{
    public partial class StudentsRegistrationView : Window
    {
        private StudentViewModel viewModel;

        public StudentsRegistrationView()
        {
            InitializeComponent();
            viewModel = new StudentViewModel();
            this.DataContext = viewModel;
        }

    }
}
