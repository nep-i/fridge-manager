

namespace F_Framework.Helpers
{
    public class Login
    {
        IToast _messagesService;
        ISettingsViewVisibility _settingsVisibility;
        public Login(IToast messagesService, ISettingsViewVisibility settingsVisibility)
        {
            _messagesService = messagesService;
            _settingsVisibility = settingsVisibility;
        }
        public async Task LoginAsync()
        {
            var automaticLogin = Preferences.Default.Get("AutomaticLogin", false);
            var isRegistered = Preferences.Default.Get("IsRegistered", false);
            var isLogedIn = Preferences.Default.Get("IsLoggedin", false);
            if (automaticLogin == true && isLogedIn)
            {
                var email = Preferences.Default.Get("Email", string.Empty);
                var pass = Preferences.Default.Get("Password", string.Empty);
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(pass))
                {
                    try
                    {
                        var authProvider = new FirebaseAuthProvider(new FirebaseConfig(StaticData.secret));
                        var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, pass);
                        if (auth != null)
                        {
                            string user = auth.User.LocalId;
                            string name = auth.User.DisplayName;
                            Settings.UserName = name;
                            Settings.Email = email;
                            Preferences.Default.Set("User", user);
                            Preferences.Default.Set("Email", email);
                            Preferences.Default.Set("Password", pass);
                            Preferences.Default.Set("IsLoggedin", true);
                            _settingsVisibility.VisibilityChanged(true);

                        }
                    }
                    catch (Exception ex)
                    {
                        Settings.SyncAllow = false;
                        Preferences.Default.Set("AutomaticLogin", false);
                        Preferences.Default.Set("IsLoggedin", false);
                        Preferences.Default.Set("User", string.Empty);
                        Preferences.Default.Set("Email", string.Empty);
                        Preferences.Default.Set("Password", string.Empty);
                        SettingVisibilityAndSync(false);
                        _messagesService.MakeToast("There was something wrong with login");
                        Console.WriteLine(ex.Message);
                    }

                }
                else
                {
                    SettingVisibilityAndSync(false);
                }
            }
            else
            {
                SettingVisibilityAndSync(false);
            }

            


        }

        private void SettingVisibilityAndSync(bool onOf)
        {
            Settings.SyncAllow = onOf;
            _settingsVisibility.VisibilityChanged(onOf);
        }
    }
}
