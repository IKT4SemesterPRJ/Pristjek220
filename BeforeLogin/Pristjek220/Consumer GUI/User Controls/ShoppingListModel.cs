﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Consumer;
using GalaSoft.MvvmLight.Command;
using Pristjek220Data;
using SharedFunctionalities;

namespace Consumer_GUI.User_Controls
{
    internal class ShoppingListModel : ObservableObject, IPageViewModel
    {
        private readonly UnitOfWork _unit = new UnitOfWork(new DataContext());
        private readonly IConsumer _user;
        public IConsumer User => _user;
        private ICommand _addToShoppingListCommand;
        private ICommand _deleteFromShoppingListCommand;
        private ICommand _enterKeyPressedCommand;
        private ICommand _generatedShoppingListCommand;
        private ICommand _illegalSignShoppingListCommand;

        private string _oldtext = string.Empty;
        private ICommand _populatingShoppingListCommand;


        private ProductInfo _selectedItem;
        private string _shoppinglistItem;
        private string _error;

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }

        public ShoppingListModel(IConsumer user)
        {
            _user = user;
            ShoppingListData = new ObservableCollection<ProductInfo>();
            _user.ReadFromJsonFile();
        }

        public ICommand AddToShoppingListCommand
        {
            get
            {
                return _addToShoppingListCommand ?? (_addToShoppingListCommand = new RelayCommand(AddToShoppingList));
            }
        }

        public ICommand DeleteFromShoppingListCommand
        {
            get
            {
                return _deleteFromShoppingListCommand ??
                       (_deleteFromShoppingListCommand = new RelayCommand(DeleteFromShoppingList));
            }
        }

        public ICommand PopulatingShoppingListCommand
        {
            get
            {
                return _populatingShoppingListCommand ??
                       (_populatingShoppingListCommand = new RelayCommand(PopulatingListShoppingList));
            }
        }

        public ICommand IllegalSignShoppingListCommand
        {
            get
            {
                return _illegalSignShoppingListCommand ??
                       (_illegalSignShoppingListCommand = new RelayCommand(IllegalSignFindProductShoppingList));
            }
        }

        public ICommand GeneratedShoppingListCommand
        {
            get
            {
                return _generatedShoppingListCommand ??
                       (_generatedShoppingListCommand = new RelayCommand(GeneratedShoppingListFromShoppingList));
            }
        }

        public ICommand EnterKeyPressedCommand
        {
            get
            {
                return _enterKeyPressedCommand ??
                       (_enterKeyPressedCommand = new RelayCommand<KeyEventArgs>(EnterKeyPressed));
            }
        }

        public ObservableCollection<ProductInfo> ShoppingListData
        {
            get
            {
                return _user.ShoppingListData; 
            }
            set
            {
                _user.ShoppingListData = value;
            }
        }

        public ObservableCollection<string> AutoCompleteList { get; } = new ObservableCollection<string>();

        public string ShoppingListItem
        {
            set
            {
                _oldtext = _shoppinglistItem;
                _shoppinglistItem = value;
            }
            get { return _shoppinglistItem; }
        }

        public ProductInfo SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private void AddToShoppingList()
        {
            if (ShoppingListItem != null)
            {
                if (ShoppingListItem != "")
                {
                    if (
                        ShoppingListData.Any(
                            x => x.Name == char.ToUpper(ShoppingListItem[0]) + ShoppingListItem.Substring(1).ToLower()))
                    {
                        var item = _user.ShoppingListData.First(
                            s => s.Name == char.ToUpper(ShoppingListItem[0]) + ShoppingListItem.Substring(1).ToLower());
                        if (item != null)
                        {
                            var intQuantity = int.Parse(item.Quantity);
                            intQuantity++;
                            item.Quantity = intQuantity.ToString();
                            OnPropertyChanged("Quantity");
                        }
                    }
                    else
                    {
                        ShoppingListData.Add(new ProductInfo(ShoppingListItem));
                        _user.WriteToJsonFile();
                    }
                }
            }
        }

        private void DeleteFromShoppingList()
        {
            if (SelectedItem == null)
                Error = "Du skal markere at produkt før du kan slette";

            else if (_user.ShoppingListData.Count == 0)
                Error = "Der er ikke tilføjet nogen produkter";

            else
            {
                _user.ShoppingListData.Remove(SelectedItem);
                _user.WriteToJsonFile();
                Error = string.Empty;
            }
        }


        private void PopulatingListShoppingList()
        {
            IAutocomplete autocomplete = new SharedFunctionalities.Autocomplete(_unit);
            AutoCompleteList?.Clear(); // not equal null
            foreach (var item in autocomplete.AutoCompleteProduct(ShoppingListItem))
            {
                AutoCompleteList?.Add(item);
            }
            OnPropertyChanged("AutoCompleteList");
        }


        private void IllegalSignFindProductShoppingList()
        {
            if (!ShoppingListItem.All(chr => char.IsLetter(chr) || char.IsNumber(chr) || char.IsWhiteSpace(chr)))
            {
                Error = "Der kan kun skrives bogstaverne fra a til å og tallene fra 0 til 9";
                ShoppingListItem = _oldtext;
            }
            else
            {
                Error = string.Empty;
            }
        }


        private void GeneratedShoppingListFromShoppingList()
        {
            ClearGeneratedLists();
            _user.CreateShoppingList();
        }

        private void ClearGeneratedLists()
        {
            _user.ClearGeneratedShoppingListData();
            _user.ClearNotInAStore();
        }

        private void EnterKeyPressed(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                AddToShoppingList();
            }
        }
    }
}