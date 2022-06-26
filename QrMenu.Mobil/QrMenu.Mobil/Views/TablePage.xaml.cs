using QrMenu.Mobil.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QrMenu.Mobil.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TablePage : ContentPage
    {
        public TablePage(int PartnerID, int BranchID)
        {
            BindingContext = new TableViewModel(PartnerID, BranchID);

            InitializeComponent();
            //collectV.BindingContext = new TableViewModel(PartnerID, BranchID).GrupluListe; 
        }
    }
}