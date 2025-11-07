using SClarkC971PA.Services;
using SClarkC971PA.Models;
namespace SClarkC971PA.Views;
//This message is to ensure maintenance instructions are correct
public partial class LoginPage : ContentPage
{
    private User _user = new User();
    private bool _toSignIn = false;
    private int _currentUserId;
	public LoginPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        //TODO: Delete the lines for user count
        var userCount = await DatabaseService.GetUserCount();
        //UserCountLbl.Text = userCount.ToString();

        //Setup page view based on whether the user is signing up or signing in
        if (userCount == 0)
        {
            //Services.Settings.FirstRun = false;

            Title = "Account Set Up";
            InstructionLbl.Text = "Set up your username and password";
            SignInBtn.Text = "Sign up";
            _toSignIn = false;
        }
        else
        {
            Title = "Sign in";
            InstructionLbl.Text = "Sign in with your username and password";
            SignInBtn.Text = "Sign in";
            _toSignIn = true;
        }

        //await RefreshTermCollectionView();
        //await ShowCourseNotifications();
        //await ShowAssessmentNotifications();
    }
    private async void SignInBtn_Clicked(object sender, EventArgs e)
    {
        if (_toSignIn)
        {
            //Allow signing in if user already exists
            if (!String.IsNullOrEmpty(UsernameEntry.Text) && !String.IsNullOrEmpty(PasswordEntry.Text))
            {
                ErrorLbl.Text = "";
                ErrorLbl.IsVisible = false;

                string username = UsernameEntry.Text;
                string password = PasswordEntry.Text;

                _currentUserId = await DatabaseService.AuthenticateUser(username, password);

                if (_currentUserId == 0)
                {
                    ErrorLbl.Text = "Username or password incorrect.";
                    ErrorLbl.IsVisible = true;
                }
                else
                {
                    await Navigation.PushAsync(new TermList(_currentUserId));
                }

            }
            else
            {
                ErrorLbl.Text = "Username or password are blank. Please try again.";
                ErrorLbl.IsVisible = true;
            }
        }
        else
        {   //Ensure no null values for password or username and add user to db if cleared
            if (!String.IsNullOrEmpty(UsernameEntry.Text) && !String.IsNullOrEmpty(PasswordEntry.Text))
            {
                ErrorLbl.Text = "";
                ErrorLbl.IsVisible = false;

                string username = UsernameEntry.Text;
                string password = PasswordEntry.Text;

                _currentUserId = await DatabaseService.AddUser(username, password);

                await Navigation.PushAsync(new TermList(_currentUserId));
            }
            else
            {
                ErrorLbl.Text = "Username and password must both contain values.";
                ErrorLbl.IsVisible = true;
            }
        }

    }

    //private void ClearUserBtn_Clicked(object sender, EventArgs e)
    //{
    //    DatabaseService.ClearAllTables();
    //}
}