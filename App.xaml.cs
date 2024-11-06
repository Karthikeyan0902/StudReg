using System;
using System.Windows;
using Registration.Views;

namespace Registration
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            
                var studentRegistrationView = new StudentsRegistrationView();
                studentRegistrationView.Show();
        }

    }
}
