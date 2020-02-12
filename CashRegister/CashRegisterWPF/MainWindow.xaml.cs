using CashRegisterAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
namespace CashRegisterWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public ObservableCollection<ReceiptLine> rls { get; set; } = new ObservableCollection<ReceiptLine>();

        private decimal totalPriceValue;
        public decimal TotalPrice
        {
            get => totalPriceValue;
            set
            {
                totalPriceValue = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }


        private HttpClient client = new HttpClient();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public MainWindow()
        {
            FetchProducts();
            InitializeComponent();
            this.DataContext = this;
        }

        public void FetchProducts()
        {
            var response = client.GetAsync("http://localhost:5000/products").Result;
            var result = JsonSerializer.Deserialize<List<Product>>(response.Content.ReadAsStringAsync().Result);
            foreach (Product p in result)
            {
                this.Products.Add(p);
            } 
        }

        private void Checkout(object sender, RoutedEventArgs e)
        {
            var response = client.GetAsync("http://localhost:5000/checkout").Result;
            var result = JsonSerializer.Deserialize<Receipt>(response.Content.ReadAsStringAsync().Result);
            if (result.ReceiptTimestamp != null)
            {
                this.Info.Content = "Thank you for you purchase";
            } else
            {
                this.Info.Content = "Error while Checkout";
            }
        }
        public void calculateAndSetTotalPriceOfReceipts()
        {
            decimal sum = 0;
            foreach (ReceiptLine rl in this.rls)
            {
                sum += rl.TotalPrice;
            }
            this.TotalPrice = sum;
        }

        private void Add_ReceiptLine(object sender, RoutedEventArgs e)
        {
            this.Info.Content = "";
            var name = (sender as Button).Content;
            var json = JsonSerializer.Serialize(name);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync("http://localhost:5000/receiptlines/add", data).Result;
            var result = JsonSerializer.Deserialize<List<ReceiptLine>>(response.Content.ReadAsStringAsync().Result);
            this.rls.Clear();
            foreach (ReceiptLine rl in result)
            {
                this.rls.Add(rl);
            }
            this.calculateAndSetTotalPriceOfReceipts();
        }
    }
}
