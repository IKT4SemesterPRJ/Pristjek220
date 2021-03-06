﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Pristjek220Data;

namespace Consumer
{
    public interface IConsumer
    {
        ObservableCollection<StoreProductAndPrice> GeneratedShoppingListData { get; }
        ObservableCollection<ProductInfo> ShoppingListData { get; set; }
        ObservableCollection<ProductInfo> NotInAStore { get; }
        Store FindCheapestStore(string productName);
        bool DoesProductExist(string productName);
        List<ProductAndPrice> FindStoresAssortment(string storeName);
        List<StoreAndPrice> FindStoresThatSellsProduct(string productName);
        void CreateShoppingList();
        void ReadFromJsonFile();
        void WriteToJsonFile();
        void ClearGeneratedShoppingListData();
        void ClearNotInAStore();
    }
}