﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Administration;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using SharedFunctionalities;

namespace Administration_GUI.User_Controls
{
    class DeleteProductModel : ObservableObject, IPageViewModel
    {
        private readonly IAutocomplete _autocomplete;
        private readonly IStoremanager _manager;
        private ICommand _deleteFromStoreDatabaseCommand;
        private ICommand _populatingDeleteProductCommand;
        private ICommand _illegalSignDeleteProductCommand;
        private ICommand _enterPressedCommand;

        private string _oldtext = string.Empty;
        
        public DeleteProductModel(Store store, IUnitOfWork unit)
        {
            _manager = new Storemanager(unit, store);
            _autocomplete = new SharedFunctionalities.Autocomplete(unit);
        }
        public ICommand DeleteFromStoreDatabaseCommand => _deleteFromStoreDatabaseCommand ?? (_deleteFromStoreDatabaseCommand = new RelayCommand(DeleteFromStoreDatabase));

        public ICommand PopulatingDeleteProductCommand => _populatingDeleteProductCommand ??
                                                          (_populatingDeleteProductCommand = new RelayCommand(PopulatingListDeleteProduct));

        public ICommand IllegalSignDeleteProductCommand => _illegalSignDeleteProductCommand ??
                                                           (_illegalSignDeleteProductCommand = new RelayCommand(IllegalSignDeleteProduct));

        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();

        

        private void PopulatingListDeleteProduct()
        {

            AutoCompleteList?.Clear();
            foreach (var item in _autocomplete.AutoCompleteProductForOneStore(_manager.Store.StoreName, ShoppingListItem))
            {
                AutoCompleteList?.Add(item);
            }
            OnPropertyChanged("AutoCompleteList");
        }

        private void IllegalSignDeleteProduct()
        {
            if (ShoppingListItem != null)
            {
                if (!ShoppingListItem.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
                {
                    ConfirmText = ($"Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9");
                    ShoppingListItem = _oldtext;
                }
            }
        }

        private void DeleteFromStoreDatabase()
        {
            if (string.IsNullOrEmpty(ShoppingListItem)) return;
            
            string productName = char.ToUpper(ShoppingListItem[0]) + ShoppingListItem.Substring(1).ToLower();

            var product = _manager.FindProduct(productName);
            if (product != null)
            {
                if (_manager.RemoveProductFromMyStore(product) != 0)
                {
                    ConfirmText = ($"Produktet {productName} findes ikke i {_manager.Store.StoreName}");
                    return;
                }

                ConfirmText = ($"{ShoppingListItem} er fjernet fra butikken {_manager.Store.StoreName}");
            }
            else
            {
                ConfirmText = ($"Produktet {productName} findes ikke");
                return;
            }
        }

        private string _shoppingListItem;
        public string ShoppingListItem
        {
            set
            {
                _oldtext = _shoppingListItem;
                _shoppingListItem = value;
                OnPropertyChanged();
            }
            get { return _shoppingListItem; }
        }

        private string _confirmText;

        public string ConfirmText
        {
            set
            {
                _confirmText = value;
                OnPropertyChanged();
            }
            get { return _confirmText; }
        }

        public ICommand EnterKeyPressedCommand => _enterPressedCommand ?? (_enterPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                DeleteFromStoreDatabase();
            }
        }
    }
}
