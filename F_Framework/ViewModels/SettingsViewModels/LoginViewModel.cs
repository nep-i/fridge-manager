
using Newtonsoft.Json;
using System.Xml.Linq;


namespace F_Framework.ViewModels.SettingsViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private IToast _messageService;
        private ICustomNavigations _navigations;
        private ISettingsViewVisibility _visibilityService;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private bool automaticLogin;

        [ObservableProperty]
        private string userPassword;

        public IRelayCommand RegisterBtn { get; set; }
        public IRelayCommand LoginBtn { get; set; }

        public LoginViewModel(IToast messageService, ICustomNavigations navigations, ISettingsViewVisibility settingsViewVisibility)
        {
            Title = "Login";
            _navigations = navigations;
            _messageService = messageService;
            _visibilityService = settingsViewVisibility;
            RegisterBtn = new AsyncRelayCommand(RegisterBtnTappedAsync);
            LoginBtn = new AsyncRelayCommand(LoginBtnTappedAsync);
        }

        private async Task LoginBtnTappedAsync()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(StaticData.secret));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(UserName, UserPassword);
                if (auth != null)
                {
                    string user = auth.User.LocalId;
                    string name = auth.User.DisplayName;

                    if (automaticLogin == true)
                    {
                        Preferences.Default.Set("Email", UserName);
                        Preferences.Default.Set("Password", UserPassword);
                        Preferences.Default.Set("User", user);
                        Preferences.Default.Set("AutomaticLogin", automaticLogin);
                        Preferences.Default.Set("IsLoggedin", true);
                        Preferences.Default.Set("AuthenticationLink", auth);
                        Settings.UserName = name;
                        Settings.Email = UserName;
                    }
                    _visibilityService.VisibilityChanged(true);
                    _messageService.MakeToast("You've logged in!");
                    await _navigations.GoToFridge();
                }
                
            }
            catch (Exception ex)
            {
                _messageService.MakeToast("Wrong password or email");
                Console.WriteLine(ex.Message);
            }
        }

        private async Task RegisterBtnTappedAsync()
        {
            await _navigations.GoToRegistration();
        }
    }
}
