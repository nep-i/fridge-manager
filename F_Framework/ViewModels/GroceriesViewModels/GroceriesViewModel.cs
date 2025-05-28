namespace F_Framework.ViewModels.GroceriesViewModels
{
    public partial class GroceriesViewModel : BaseViewModel
    {
        public IRelayCommand ReUseCommand { get; set; }
        public IRelayCommand<object> EditGroceryCommand { get; set; }
        public IRelayCommand<object> IsDoneCommand { get; set; }
        public IRelayCommand<object> DeleteGroceryCommand { get; set; }
        public IRelayCommand GetAllGroceriesCommand { get; set; }
        public IRelayCommand<object> AddSelectedGroceriesFullCommand { get; set; }

        [ObservableProperty]
        public GroceryFull addedGrocery;

        [ObservableProperty]
        public ObservableCollection<GroceryFull> allFullGroceries;

        public ObservableCollection<GroceryShopProduct> GroceryShopProducts { get; set; }

        [ObservableProperty]
        public bool isRefreshing;

        private IUnitOfWork _service;
        private ICustomNavigations _navigations;
        private IToast _messages;
        private Result<GroceryFull> _result { get; set; }

        public GroceriesViewModel(IUnitOfWork service, ICustomNavigations navigations, IToast messages)
        {
            _messages = messages;
            _service = service;
            _navigations = navigations;
            AsigningLocals();
        }

        private void AsigningLocals()
        {
            Title = "My Grocery Lists";
            ToDetailsView = new AsyncRelayCommand(GoToDetailsView);
            GetAllGroceriesCommand = new AsyncRelayCommand(GetAllGroceries);
            DeleteGroceryCommand = new AsyncRelayCommand<object>(DeleteGroceries);
            AllFullGroceries = new ObservableCollection<GroceryFull>();
            addedGrocery = new GroceryFull();
            GroceryShopProducts = new ObservableCollection<GroceryShopProduct>();
            isRefreshing = false;
            ReUseCommand = new AsyncRelayCommand(MakeGroceryActive);
            AddSelectedGroceriesFullCommand = new RelayCommand<object>(AddSelected);
        }

        private async Task MakeGroceryActive()
        {
            var result = new Result<Grocery>();
            var groceryFull = AddedGrocery;
            try
            {
                var isOnlyActive = await _service.Groceries.Value.GetAllByFilterAsync(x => x.IsDone == false);
                if (isOnlyActive.elements.Count() > 0)
                    foreach (var item in isOnlyActive.elements)
                    {
                        item.IsDone = true;
                        await _service.Groceries.Value.Update(item);
                    }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "getactive", ex.InnerException);
            }

            try
            {
                var isOnlyActiveBridges = await _service.GroceryShopProducts.Value.GetAllByFilterAsync(x => x.IsActive == true);
                if (isOnlyActiveBridges.elements.Count() > 0)
                    foreach (var item in isOnlyActiveBridges.elements)
                    {
                        if (item.Key != "")
                        {
                            item.IsActive = false;
                            await _service.GroceryShopProducts.Value.Update(item);
                        }
                    }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "getallbyfilter", ex.InnerException);
            }

            try
            {
                var isOnlyActiveProducts = await _service.Products.Value.GetAllByFilterAsync(x => x.IsDone == false);
                if (isOnlyActiveProducts.elements.Count() > 0)
                    foreach (var item in isOnlyActiveProducts.elements)
                    {
                        if (item.Key != "")
                        {
                            item.IsDone = true;
                            await _service.Products.Value.Update(item);
                        }
                    }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "getallproducts", ex.InnerException);
            }


            var grocery = await _service.Groceries.Value.GetFirstOrDefautAsync(x => x.Id == groceryFull.Id);
            grocery.element.IsDone = false;
            grocery.element.UsedAmount = +1;
            grocery.element.DateAdded = DateTime.Now;
            try
            {
                result = await _service.Groceries.Value.Update(grocery.element);
                var newActiveConnections = await _service.GroceryShopProducts.Value.GetAllByFilterAsync(x => x.GroceryId == groceryFull.Id);
                var connectionsActive = newActiveConnections.elements;
                foreach (var item in connectionsActive)
                {
                    if (item.Key != "" || Settings.SyncAllow == false)
                    {
                        try
                        {
                            item.IsActive = true;
                            await _service.GroceryShopProducts.Value.Update(item);
                            var activeProducts = await _service.Products.Value.GetFirstOrDefautAsync(x => x.Id == item.ProductId);

                            activeProducts.element.IsDone = false;
                            activeProducts.element.UsedAmount =+ 1;
                            await _service.Products.Value.Update(activeProducts.element);
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(ex.Message + "updateitem", ex.InnerException);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "updateactive", ex.InnerException);
            }



            await Task.Delay(2200);
            _messages.MakeToast("Grocery Activated!");

            await _navigations.GoToGroceryDetails(addedGrocery);
        }

        public async Task DeleteGroceries(object item)
        {
            isRefreshing = true;
            var grocery = item as GroceryFull;
            var result = new Result<GroceryShopProduct>();
            result = await _service.GroceryShopProducts.Value.GetAllByFilterAsync(x => x.GroceryId == grocery.Id);
            var lst = result.elements.Select(x => x.Id).ToList();
            isRefreshing = false;
            var keys = result.elements.Select(x => x.Key).ToList();
            _ = await _service.GroceryShopProducts.Value.RemoveRange(lst, keys);
            _ = await _service.Groceries.Value.Remove(grocery.Id, grocery.Key);
            addedGrocery = new GroceryFull();
            AllFullGroceries.Remove(grocery);
            var groceries = new ObservableCollection<GroceryFull>();
            groceries = AllFullGroceries;
            AllFullGroceries.Clear();
            foreach (var element in groceries)
                AllFullGroceries.Add(element);
        }

        public async Task<Result<GroceryFull>> GoToDetailsView()
        {
            _result = new Result<GroceryFull>();
            if (SelectedProducts != null && AllFullGroceries.Count == 1)
            {
                _result.element = AllFullGroceries.FirstOrDefault();
                SelectedProducts.Clear();
                await _navigations.GoToGroceryDetails(_result.element);
            }
            _result.Succeeded = true;
            return _result;
        }

        public void AddSelected(object item)
        {
            var grocery = item as GroceryFull;
            addedGrocery = grocery;
        }

        public async Task GetAllGroceries()
        {
            IsRefreshing = true;
            var result = new Result<Grocery>();
            result = await _service.Groceries.Value.GetAll();
            var count = result.elements.Count();
            if (count != AllFullGroceries.ToList().Count())
            {
                AllFullGroceries.Clear();
                var getter = new GroceryFullGetter(_service);
                var lst = await getter.GetAllGroceriesFull();
                foreach (var item in lst)
                    AllFullGroceries.Add(item);
                addedGrocery = null;
            }

            IsRefreshing = false;
        }
    }
}