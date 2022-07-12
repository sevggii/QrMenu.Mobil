using Newtonsoft.Json;
using QrMenu.Mobil.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace QrMenu.Mobil.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Action DisplayInvalidLoginPrompt;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                //PropertyChanged(this, new PropertyChangedEventArgs("Email"));
                NotifyPropertyChanged();
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                // PropertyChanged(this, new PropertyChangedEventArgs("Password"));
                NotifyPropertyChanged();
            }
        }
        private User _userr { get; set; }
        public User Userr
        {
            get
            {
                return _userr;
            }
            set
            {
                if (value != _userr)
                {
                    _userr = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private bool _mySwitch { get; set; }
        public bool MySwitch
        {
            get
            {
                return _mySwitch;
            }
            set
            {
                if (value != _mySwitch)
                {
                    _mySwitch = value;
                    NotifyPropertyChanged();
                }
            }

        }

        public ICommand SubmitCommand { protected set; get; }
        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
        }
        public void OnSubmit()
        {
            if (email != "" && password != "")
            {
                Login(Email, Password);
            }

            else
            {
                DisplayInvalidLoginPrompt();
            }
        }

        private async void Login(string emaill, string passwordd)
        {
            var LoginUrl = "http://api.kodegitimi.com/api/login?email=";
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(LoginUrl + emaill + "&password=" + passwordd);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (content != "null")
                {
                    var tables = JsonConvert.DeserializeObject<User>(content);
                    Userr = new User
                    {
                        ID = tables.ID,
                        PartnerID = tables.PartnerID,
                        BranchID = tables.BranchID,
                        Name = tables.Name,
                        IsActive = tables.IsActive,
                        Email = tables.Email,
                        Phone = tables.Phone,
                        Password = tables.Password,
                        AuthTypeID = tables.AuthTypeID
                    };

                    // https://docs.microsoft.com/tr-tr/xamarin/android/platform/files/

                    if (MySwitch) 
                        SaveLogin(Userr.PartnerID.ToString(), Userr.BranchID.ToString());

                    else
                        //data goes to viewmodel from here and homepage opens?
                        App.Current.MainPage = new TablePage(Userr.PartnerID, Userr.BranchID); //no back buttons

                        //There is a back button.
                        //App.Current.MainPage.Navigation.PushAsync(new TablePage(Userr.PartnerID, Userr.BranchID));
                        //App.Current.MainPage.DisplayAlert("BAŞARILI GİRİŞ ;)", "Sayfaya Yönlendiriliyor..", "OK");
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("HATA", "Email veya Şifre yanlış :(" + "Lütfen Tekrar Deneyin.", "OK");
                }
            }

            else
            {
                Debug.WriteLine("An error occured while loading data");
            }
        }
        public async Task SaveLogin(string PartnerIDD, string BrancIDD)
        {
            var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DENEMEEEEEEEEEEE.txt");
            using (var writer = File.CreateText(backingFile))
            {
                await writer.WriteAsync(PartnerIDD + " " + BrancIDD);
            }


            //reads data
            //string content = "";
            using (var reader = new StreamReader(backingFile, true))
            {
                string text = File.ReadAllText(backingFile);

                //splits and routes data
                string[] content;
                content = text.Split(' ');
                App.Current.MainPage = new TablePage(int.Parse(content[0]), int.Parse(content[1]));
            }
        }
    }
    public class User
    {
        public int ID { get; set; }
        public int PartnerID { get; set; }
        public int BranchID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int AuthTypeID { get; set; }
    }
}
