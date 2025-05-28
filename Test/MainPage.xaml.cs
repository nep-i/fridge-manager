namespace Test
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
            //ImageView1.Source = new ImageSource.FromResource("beta.jpg");
            //ImageView1.Source =   ImageSource.FromResource("CWON22.Resources.Images.test.png");
            // OR
            //ImageView1.Source = new ImageSource.FromResource("Resources/Images/beta.jpg");
        public MainPage()
        {

            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";
            MyImage.Source = ImageSource.FromFile("fridge_color.png");
            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}