using Newtonsoft.Json;
using QrMenu.Mobil.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace QrMenu.Mobil.ViewModels
{
    public class TableViewModel : BaseViewModel
    { 
        public class Product
        {
            public int CategoryID { get; set; }
            public int SupplierID { get; set; }
            public int StockType { get; set; }
            public string Barcode { get; set; }
            public string StockCode { get; set; }
            public string Description { get; set; }
            public double PricePurchase { get; set; }
            public double PriceSales { get; set; }
            public int TaxRate { get; set; }
            public int PreparationTime { get; set; }
            public List<object> Images { get; set; }
            public object Category { get; set; }
            public object ProductImage { get; set; }
            public object CategoryName { get; set; }
            public object SupplierName { get; set; }
            public int Amount { get; set; }
            public int OrderDetailID { get; set; }
            public int ID { get; set; }
            public int PartnerID { get; set; }
            public int BranchID { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }
        }

        public class OrderDetail
        {
            public int ID { get; set; }
            public int PartnerID { get; set; }
            public int BranchID { get; set; }
            public int OrderID { get; set; }
            public int ProductID { get; set; }
            public double UnitPrice { get; set; }
            public int Amount { get; set; }
            public bool IsPrint { get; set; }
            public double Total { get; set; }
            public Product Product { get; set; }
        }

        public class Order
        {
            public int ID { get; set; }
            public int PartnerID { get; set; }
            public int BranchID { get; set; }
            public int TableID { get; set; }
            public int EmployeeID { get; set; }
            public DateTime Date { get; set; }
            public int Status { get; set; }
            public bool IsActive { get; set; }
            public int MinutesOpen { get; set; }
            public object SStatus { get; set; }
            public List<OrderDetail> OrderDetails { get; set; }
            public object OrderDetail { get; set; }
            public object OrderPayments { get; set; }
        }

        public class Table
        {
            public int SectionID { get; set; }
            public string QRCode { get; set; }
            public int Status { get; set; }
            public int? OrderID { get; set; }
            public object OrderTotal { get; set; }
            public DateTime? OrderDate { get; set; }
            public object ReservationStartDate { get; set; }
            public object ReservationEndDate { get; set; }
            public object SectionName { get; set; }
            public object CStatus { get; set; }
            public Order Order { get; set; }
            public int ID { get; set; }
            public int PartnerID { get; set; }
            public int BranchID { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }

            public string BgColor { get; set; }
            public string HeaderName { get; set; }
        }

        public class MyArray
        {
            public int TableAmount { get; set; }
            public List<Table> Tables { get; set; }
            public int ID { get; set; }
            public int PartnerID { get; set; }
            public int BranchID { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }
        }

        public class Root
        {
            public List<MyArray> MyArray { get; set; }
        }

        #region Properties
        private IList<Root> _root { get; set; }
        public IList<Root> TableRoot
        {
            get
            {
                return _root;
            }

            set
            {
                if (value != _root)
                {
                    _root = value;
                    NotifyPropertyChanged();
                }
            }
        } //trial
        private IList<Table> _tableList { get; set; }
        public IList<Table> TableList
        {
            get
            {
                return _tableList;
            }

            set
            {
                if (value != _tableList)
                {
                    _tableList = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ObservableCollection<Grouping<string, Table>> _groupedlist { get; set; }
        public ObservableCollection<Grouping<string, Table>> GroupedList
        {
            get
            {
                return _groupedlist;
            }

            set
            {
                if (value != _groupedlist)
                {
                    _groupedlist = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public ICommand DeleteCommand { protected set; get; }
        #endregion

        public TableViewModel(int PartnerID, int BranchID)
        {
            DeleteCommand = new Command(OnSubmitFileDelete);
            GetTableData(PartnerID, BranchID);
        }

        public void OnSubmitFileDelete() //deleting file 
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DENEMEEEEEEEEEEE.txt");
            File.Delete(backingFile);
            App.Current.MainPage = new LoginPage();
        }

        public async void GetTableData(int PartnerID, int BranchID)
        {
            var url = "http://api.kodegitimi.com/api/TableList?PartnerID=" + PartnerID + "&BranchID=" + BranchID;
            #region GetStringAsync
            HttpClient httpClient = new HttpClient();
            #endregion

            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tables = JsonConvert.DeserializeObject<List<Table>>(content);
            
                var arrayy= JsonConvert.DeserializeObject<List<Root>>(content);

              /*  for (int i = 0; i < arrayy.Count; i++)
                {
                    string[] HeaderName;
                    HeaderName = arrayy[i].MyArray[i].Tables[i].Name.Split(' ');
                    tables[i].HeaderName = splitHeaderName[0].ToString();
                }*/

                for (int i = 0; i < tables.Count; i++)
                {
                    switch (tables[i].Status)
                    {
                        case 1:
                            tables[i].BgColor = "White";
                            break;
                        case 2:
                            tables[i].BgColor = "Yellow";
                            break;
                        case 3:
                            tables[i].BgColor = "Red";
                            break;
                    }

                    string[] splitHeaderName;
                    splitHeaderName = tables[i].Name.Split(' ');
                    tables[i].HeaderName = splitHeaderName[0].ToString();
                }
                TableList = new ObservableCollection<Table>(tables);
            }
            else
            {
                App.Current.MainPage.DisplayAlert("HATA", "VERİ YÜKLEMEDE HATA", "OK");
            }

            BindingWithGrouping();
        }

        public void BindingWithGrouping() //when bind this method will now be used.
        {
            var result = TableList;

            GroupedList = new ObservableCollection<Grouping<string, Table>>
              (result.
              OrderBy(c => c.HeaderName).
              GroupBy(c => c.HeaderName.ToString()).Select
              (k => new Grouping<string, Table>(k.Key, k)));
        }

        #region post,delete methods
        private async void PostDataAsync()
        {
            var url = "http://api.kodegitimi.com/api/TableList?PartnerID=2&BranchID=2";
            IsLoading = true;
            HttpClient httpClient = new HttpClient();
            var newTable = new Table()
            {
                SectionID = 1,
                QRCode = "wfewgfweg",
                //...

            };

            //similary 'PutAsync()' to update the data.

            var jsonObject = JsonConvert.SerializeObject(newTable);
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Data saved successfully!");
            }
            else
            {
                Debug.WriteLine("An error occured while loading data");
            }
            IsLoading = false;
        }

        // 'DeleteAsync()' to delete the data - is similar to 'GetAsync()'
        private async void DeleteDataAsync()
        {
            var url = "http://api.kodegitimi.com/api/TableList?PartnerID=2&BranchID=2";
            IsLoading = true;
            HttpClient httpClient = new HttpClient();

            var id = 4;
            var uri = new Uri(string.Format(url, id));

            var response = await httpClient.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Item deleted successfully!");
            }
            else
            {
                Debug.WriteLine("An error occured while loading data");
            }
            IsLoading = false;
        }
        #endregion       
    }
}
