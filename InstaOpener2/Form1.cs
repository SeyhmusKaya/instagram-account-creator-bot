using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace InstaOpener2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();

        }

        public static int RandNumber(int start, int finish)
        {
            Random rnd = new Random();
            return rnd.Next(start, finish);
        }
        public static string RandomString(int length)
        {
            Random rnd = new Random();
            const string chars = "abcdefghjklmnoprstyouvyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
        public static string RandomCharacter(int length)
        {
            Random rnd = new Random();
            const string chars = "abcdefghjklmnoprstyouvyxz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
        public static string RandomNumber(int length)
        {
            Random rnd = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        public static string CreateUsername(String mail)
        {
            String username = "";
            String[] words = mail.Split('@');
            username = RandomCharacter(2) + words[0] + RandomNumber(2);
            return username;
        }
        private static void WriteFileSuccess(String mail, String password)
        {
            /*
            string filePath = Application.StartupPath.ToString() + "\\successMail.txt"; 
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            sw.Flush(); // write
            sw.Close();
            fs.Close();
            */
            File.AppendAllText(Application.StartupPath.ToString() + "\\successMail.txt", mail + ":" + password + Environment.NewLine);

        }
        private static void WriteFileFail(String mail, String password)
        {
            File.AppendAllText(Application.StartupPath.ToString() + "\\failMail.txt", mail + ":" + password + Environment.NewLine);

        }
        private static void WriteInstaAccount(String username, String password,String mail,String fullname)
        {
            File.AppendAllText(Application.StartupPath.ToString() + "\\instaUser.txt", username + ":" + password + ":" + mail + ":" + fullname + Environment.NewLine);

        }
        public void Deneme(String yandexMail, String yandexPass,String instaFullName, String instaPassword,String instaUsername)
        {
            label1.Text = "Tarayıcı başlatılıyor...";

            ChromeDriverService _chromeDriverService;
            ChromeOptions _chromeOptions;
            IWebDriver driver;
            IWebDriver yandexDriver;

            //get random proxy 
            label1.Text = "Rastgele proxy seçiliyor...";
            var lines = File.ReadAllLines("proxy.txt");
            var r = new Random();
            var randomLineNumber = r.Next(0, lines.Length - 1);
            String line = lines[randomLineNumber];
            String [] allProxy = line.Split(':');
            String proxyId = allProxy[0];
            int proxyPort = int.Parse(allProxy[1]);
            String proxyUsername = allProxy[2];
            String proxyPassword = allProxy[3];
            //end random proxy

            _chromeDriverService = ChromeDriverService.CreateDefaultService();
            _chromeDriverService.HideCommandPromptWindow = true;
            _chromeOptions = new ChromeOptions();
            //_chromeOptions.AddArguments("headless"); // for background

            // _chromeOptions.AddHttpProxy(proxyId, proxyPort, proxyUsername, proxyPassword); //--> if you want 
            _chromeOptions.AddArgument("ignore-certificate-errors");


            Proxy proxy = new Proxy();
            proxy.Kind = ProxyKind.Manual;
            proxy.IsAutoDetect = false;
            proxy.HttpProxy = proxyId + ":" + proxyPort;
            proxy.SocksUserName = proxyUsername;
            proxy.SocksPassword = proxyPassword;
            _chromeOptions.Proxy = proxy;

            driver = new ChromeDriver(_chromeDriverService, _chromeOptions);
            yandexDriver = new ChromeDriver(_chromeDriverService, _chromeOptions);

            try
            {
                label1.Text = "İnstagram kayıt sayfasına gidiliyor...";

                driver.Navigate().GoToUrl("https://www.instagram.com/accounts/emailsignup/?hl=tr");
                Thread.Sleep(10000);
                try
                {
                    WebElement cerez = (WebElement)driver.FindElement(By.XPath("//button[text()='Sadece temel çerezlere izin ver']"));
                    cerez.Click();
                    Thread.Sleep(5000);
                }
                catch
                {
                    Thread.Sleep(1000);
                }
                label1.Text = "Kayıt değerleri giriliyor...";
                try
                {

                    WebElement mail = (WebElement)driver.FindElement(By.Name("emailOrPhone"));
                    mail.SendKeys(yandexMail);
                }
                catch
                {
                    driver.Navigate().Refresh();
                    Thread.Sleep(8000);
                    WebElement mail = (WebElement)driver.FindElement(By.Name("emailOrPhone"));
                    mail.SendKeys(yandexMail);
                }
                label1.Text = "Mail adresi girildi...";

                Thread.Sleep(1256);
                WebElement fullName = (WebElement)driver.FindElement(By.Name("fullName"));
                fullName.SendKeys(instaFullName);
                label1.Text = "İsim girildi...";

                Thread.Sleep(2180);
                WebElement username = (WebElement)driver.FindElement(By.Name("username"));
                username.SendKeys(instaUsername);
                label1.Text = "Kullanıcı adı girildi...";

                Thread.Sleep(1354);
                WebElement pass = (WebElement)driver.FindElement(By.Name("password"));
                pass.SendKeys(instaPassword);
                label1.Text = "Şifre girildi...";

                Thread.Sleep(2856);
                try
                {
                    WebElement clickButtonNext1 = (WebElement)driver.FindElement(By.XPath("//button[text()='Kaydol']"));
                    clickButtonNext1.Click();
                }
                catch
                {
                    WebElement clickButtonNext2 = (WebElement)driver.FindElement(By.XPath("//button[text()='İleri']"));
                    clickButtonNext2.Click();
                }
                Thread.Sleep(5856);
                label1.Text = "Rastgele doğum tarihi giriliyor...";
                
                var allDate = driver.FindElements(By.ClassName("h144Z"));
                // allDate[0] --> month
                // allDate[1] --> day
                // allDate[2] --> year

                var selectElement0 = new SelectElement(allDate[0]);
                selectElement0.SelectByIndex(RandNumber(1, 11));
                Thread.Sleep(1116);

                var selectElement = new SelectElement(allDate[1]);
                selectElement.SelectByIndex(RandNumber(1, 28));
                Thread.Sleep(1240);

                var selectElement2 = new SelectElement(allDate[2]);
                selectElement2.SelectByIndex(RandNumber(22, 52));
                Thread.Sleep(1212);

                var button2 = driver.FindElement(By.ClassName("lC6p0"));
                button2.Click();
                Thread.Sleep(256);


                //Login Yandex
                //IWebDriver yandexDriver = new ChromeDriver(_chromeDriverService, _chromeOptions);
                label1.Text = "Yandexe giriş yapılıyor...";

                yandexDriver.Navigate().GoToUrl("https://passport.yandex.com.tr/auth?from=mail&origin=hostroot_homer_auth_tr&retpath=https%3A%2F%2Fmail.yandex.com.tr%2F&backpath=https%3A%2F%2Fmail.yandex.com.tr%3Fnoretpath%3D1");
                Thread.Sleep(15256);
                try
                {
                    var yandexMailLogin = yandexDriver.FindElement(By.Id("passp-field-login"));
                    yandexMailLogin.SendKeys(yandexMail);
                    Thread.Sleep(1231);
                }
                catch
                {
                    yandexDriver.Navigate().Refresh();
                    Thread.Sleep(15256);
                    var yandexMailLogin = yandexDriver.FindElement(By.Id("passp-field-login"));
                    yandexMailLogin.SendKeys(yandexMail);
                }

                label1.Text = "Yandex mail girildi...";

                var buttonLoginYandex = yandexDriver.FindElement(By.Id("passp:sign-in"));
                buttonLoginYandex.Click();
                Thread.Sleep(3531);

                var yandexMailPass = yandexDriver.FindElement(By.Id("passp-field-passwd"));
                yandexMailPass.SendKeys(yandexPass);
                Thread.Sleep(1538);
                label1.Text = "Yandex şifre girildi...";

                var buttonLoginYandexNext = yandexDriver.FindElement(By.Id("passp:sign-in"));
                buttonLoginYandexNext.Click();

                label1.Text = "Yandex mail'e gidiliyor...";
                Thread.Sleep(8000);

                //navigate social page 
                yandexDriver.Navigate().GoToUrl("https://mail.yandex.com.tr/#tabs/social");
                Thread.Sleep(11538);


                try
                {
                    var popupClose = yandexDriver.FindElement(By.CssSelector("div.b-popup__close.ns-action"));
                    popupClose.Click();
                }
                catch (Exception e)
                {
                    Thread.Sleep(1538);
                }
                label1.Text = "Yandexten onay kodu alınıyor...";

                var buttonSelectHrefCode = yandexDriver.FindElements(By.ClassName("mail-MessageSnippet-Wrapper"));
                buttonSelectHrefCode[0].Click();
                Thread.Sleep(4523);
                label1.Text = "Onay kodu alındı...";

                var yandexCodeGet = yandexDriver.FindElements(By.TagName("td"));
                //yandexCode[8]; -->> all text
                //yandexCode[9]; -->> all text
                //yandexCode[16]; -->> İnstagram confirmation code
                String yandexCode = yandexCodeGet[16].Text;
                Thread.Sleep(1000);
                //yandex done

                label1.Text = "Onay kodu giriliyor...";

                //send keys for instagram register
                WebElement emailConfirmationCode = (WebElement)driver.FindElement(By.Name("email_confirmation_code"));
                emailConfirmationCode.SendKeys(yandexCode);

                Thread.Sleep(1000);

                var registerButton = driver.FindElement(By.ClassName("L3NKy"));
                registerButton.Click();



                if(yandexCode.Length>4)
                {
                    label1.Text = "Sonuç başarılı..";

                    //add user to txt
                    WriteInstaAccount(instaUsername, instaPassword, yandexMail, instaFullName);
                    //set success text
                    String up = label4.Text;
                    int sayi = int.Parse(up) + 1;
                    label4.Text = sayi.ToString();

                    //add successMail.txt
                    WriteFileSuccess(yandexMail, yandexPass);
                }
                else
                {
                    label1.Text = "Sonuç başarısız..";
                    // set failed text
                    String up = label5.Text;
                    int sayi = int.Parse(up) + 1;
                    label5.Text = sayi.ToString();
                    //add failMail.txt
                    WriteFileFail(yandexMail, yandexPass);
                }

                Thread.Sleep(30000);


            }
            catch (Exception e)
            {
                label1.Text = "İnternet bağlantısı, proxy veya mailleri kontrol edin...";
                // set failed text
                String up = label5.Text;
                int sayi = int.Parse(up) + 1;
                label5.Text = sayi.ToString();
                //add failMail.txt
                WriteFileFail(yandexMail, yandexPass);
                MessageBox.Show(e.ToString());
            }
            finally
            {
                driver.Quit();
                yandexDriver.Quit();

            }
        }
        public async void Start(String yandexMail,String yandexPass)
        {
            String instaFullName="";
            try
            {
                string[] lines = File.ReadAllLines("fullname.txt");
                instaFullName = lines[RandNumber(0,490)].ToLower();

            }
            catch (IOException e)
            {
                MessageBox.Show(e.ToString());
            }
            String instaUsername = CreateUsername(yandexMail);
            String instaPassword = yandexPass;
            label1.Text = "Rastgele kullanıcı adı oluşturuldu...";

            Deneme(yandexMail, yandexPass, instaFullName, instaPassword,instaUsername);
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                label1.Text = "Bot durumu: Aktif..";
                string[] allMail = File.ReadAllLines("mail.txt");
                foreach(String mail in allMail)
                {
                    String[] splited = mail.Split(' ');
                    /* 
                     * Mail Adresi: seym4ali@yandex.com Şifre: ruvali8226 Güvenlik Sorusu Cevabı: gazapizm
                     */
                    //splited[0]= Mail 
                    //splited[1] = Adresi:  
                    //splited[2] = mail@yandex.com  
                    //splited[3] = Şifre:  
                    //splited[4] = password  
                    label1.Text = "Yandex mail alınıyor...";

                    String yandexMail = splited[2].Trim();
                    String yandexPass = splited[4].Trim();

                    //start create mail
                    Start(yandexMail, yandexPass);
                }
                MessageBox.Show("Tüm yandex hesaplar kullanıldı");
            }
            catch (Exception a)
            {
            }
        }
    }
}