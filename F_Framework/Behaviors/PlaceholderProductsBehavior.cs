namespace F_Framework.Behaviors
{
    public class PlaceholderProductsBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var result = StaticData.ProductNames.Where(x => x.Contains(args.NewTextValue)).ToList();
            ((Entry)sender).Placeholder = string.Join(" ", result);
            //((Entry)sender).Focus();
        }
    }
}