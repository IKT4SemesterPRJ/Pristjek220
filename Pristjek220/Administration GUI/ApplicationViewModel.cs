﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Administration;
using Administration_GUI.User_Controls;
using Administration_GUI;
using Pristjek220Data;

namespace Administration_GUI

{
    internal class ApplicationViewModel : ObservableObject
    {
       private IPageViewModel _currentPageViewModel;
        private ObservableCollection<IPageViewModel> _pageViewModels;
        private Store _store;

        public ApplicationViewModel(Store store, IUnitOfWork unit)
        {

            _store = store;
            // Add available pages

            PageViewModels.Add(new ChangePriceModel(_store, unit));
            PageViewModels.Add(new DeleteProductModel(_store, unit));
            PageViewModels.Add(new NewProductModel(_store, unit));

            // set startup page
            _currentPageViewModel = _pageViewModels[2];

        }

        public ObservableCollection<IPageViewModel> PageViewModels => _pageViewModels ?? (_pageViewModels = new ObservableCollection<IPageViewModel>());

        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (_currentPageViewModel == value) return;
                _currentPageViewModel = value;
                OnPropertyChanged();
            }
        }

        #region Commands

        
        private ICommand _changeWindowChangePriceCommand;

        public ICommand ChangeWindowChangePriceCommand => _changeWindowChangePriceCommand ?? (_changeWindowChangePriceCommand = new RelayCommand(ChangeWindowChangePrice));

        private void ChangeWindowChangePrice()
        {
            CurrentPageViewModel = PageViewModels[0];
        }

        private ICommand _changeWindowDeleteProductCommand;

        public ICommand ChangeWindowDeleteProductCommand => _changeWindowDeleteProductCommand ??
                                                            (_changeWindowDeleteProductCommand = new RelayCommand(ChangeWindowDeleteProduct));

        private void ChangeWindowDeleteProduct()
        {
            CurrentPageViewModel = PageViewModels[1];
        }

        private ICommand _changeWindowNewProductCommand;

        public ICommand ChangeWindowNewProductCommand => _changeWindowNewProductCommand ??
                                                         (_changeWindowNewProductCommand = new RelayCommand(ChangeWindowNewProduct));

        private void ChangeWindowNewProduct()
        {
            CurrentPageViewModel = PageViewModels[2];
        }

        private ICommand _logOutCommand;

        public ICommand LogOutCommand => _logOutCommand ??
                                         (_logOutCommand = new RelayCommand(LogOut));

        private void LogOut()
        {
            var CurrentWindow = Application.Current.MainWindow;
            var LogIn = new LogIn();
            LogIn.Show();
            CurrentWindow.Close();
            Application.Current.MainWindow = LogIn;

        }


        #endregion
    }

    public interface IPageViewModel
    {
    }
}