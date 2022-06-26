using QrMenu.Mobil.Views;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QrMenu.Mobil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            #region 

            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DENEMEEEEEEEEEEE.txt");

            if (backingFile == null || !File.Exists(backingFile)) //DOSYA YOKSA!!!!!
            {
                MainPage = new NavigationPage(new LoginPage());
            }

            else  //DOSYA VARSA, OKU. ( PARTNERID VE BRANCHID )
            {
                string icerik = "";
                using (var reader = new StreamReader(backingFile, true))
                {
                    string text = File.ReadAllText(backingFile);
                    //split ve yolla.
                    string[] bolunecekIcerik;
                    bolunecekIcerik = text.Split(' ');
                    MainPage = new NavigationPage(new TablePage(int.Parse(bolunecekIcerik[0]), int.Parse(bolunecekIcerik[1])));
                }

            }
            #endregion
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
