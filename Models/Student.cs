using System.ComponentModel;
using System.Windows;

public class Student : INotifyPropertyChanged
{
    private int id;
    private string name;
    private string email;
    private int phno;

    public int Id
    {
        get => id;
        set
        {
            if (id != value)
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
    }

    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public string Email
    {
        get => email;
        set
        {
            if (email != value)
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }

    public int Phno
    {
        get => phno;
        set
        {
            if (phno != value)
            {
                phno = value;
                OnPropertyChanged(nameof(Phno));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

/*public class Student : DependencyObject
{

    public int Id
    {
        get { return (int)GetValue(IdProperty); }
        set { SetValue(IdProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IdProperty =
        DependencyProperty.Register("Id", typeof(int), typeof(Student),null );



    public string Name
    {
        get { return (string)GetValue(NameProperty); }
        set { SetValue(NameProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NameProperty =
        DependencyProperty.Register("Name", typeof(string), typeof(Student), null);



    public string Email
    {
        get { return (string)GetValue(EmailProperty); }
        set { SetValue(EmailProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Email.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EmailProperty =
        DependencyProperty.Register("Email", typeof(string), typeof(Student), null);



    public int Phno
    {
        get { return (int)GetValue(PhnoProperty); }
        set { SetValue(PhnoProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Phno.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PhnoProperty =
        DependencyProperty.Register("Phno", typeof(int), typeof(Student),null );


}*/
