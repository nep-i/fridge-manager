
namespace F_Framework.ViewModels.SettingsViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private ICustomNavigations _navigations;
        private IToast _messageService;
        private ISettingsViewVisibility _visibilityService;
        [ObservableProperty]
        string email;
        [ObservableProperty]
        string name;
        [ObservableProperty]
        string password;
        public IRelayCommand RegisterUser { get; set; }


        public RegisterViewModel(IToast messageService, ICustomNavigations navigations, ISettingsViewVisibility settingsViewVisibility)
        {
            Title = "Registration";
            _navigations = navigations;
            _messageService = messageService;
            RegisterUser = new AsyncRelayCommand(RegisterUserTappedAsync);
            _visibilityService = settingsViewVisibility;
        }

        private async Task RegisterUserTappedAsync()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(StaticData.secret));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password, Name);
                string token = auth.FirebaseToken;
                if (token != null)
                    _messageService.MakeToast("User Registered successfully");

                Preferences.Default.Set("Email", Email);
                Preferences.Default.Set("Password", Password);
                Preferences.Default.Set("Name", Name);
                Settings.UserName = Name;
                Settings.Email = Email;
                Preferences.Default.Set("AutomaticLogin", true);
                Preferences.Default.Set("IsLoggedin", true);
                Preferences.Default.Set("AuthenticationLink", auth);
                _messageService.MakeToast("You've registered!");
                _visibilityService.VisibilityChanged(true);
                await _navigations.GoToFridge();
            }
            catch (Exception ex)
            {
                _messageService.MakeToast(ex.Message);
                throw;
            }
        }
    }
}
