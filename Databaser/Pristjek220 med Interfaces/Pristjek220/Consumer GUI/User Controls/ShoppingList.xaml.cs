﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoComplete;
using Consumer;
using Pristjek220Data;

namespace Consumer_GUI.User_Controls
{
    /// <summary>
    /// Interaction logic for ShoppingList.xaml
    /// </summary>
    public partial class ShoppingList : UserControl
    {
        public ShoppingList()
        {
            InitializeComponent();
        }
        
        private void BtnShowGeneratedShoppingList_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow win = (MainWindow) Window.GetWindow(this);
            win.openGeneretedShoppinglist();
        }
    }
}
