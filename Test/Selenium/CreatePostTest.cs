using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Test.Selenium
{
    internal class CreatePostTest
    {
        IWebDriver driver;
        WebDriverWait wait;
        Actions actions;
        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver("D:\\For study\\Ki-5\\SWT_2nd\\Tools\\chromedriver-win32");
            //driver.Manage().Window.Maximize();
            driver.Manage().Window.Size = new System.Drawing.Size(1151, 1040);
            //Set thời gian chờ ngầm định cho tất cả các Step
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            // Set thời gian load trang 
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Navigate().GoToUrl("http://localhost:3000/login");
            actions = new Actions(driver);
        }

        [Test]
        public void CreatePost_01_Case_PostOnlyText()
        {
            // Login process
            IWebElement loginButton = wait.Until(d => d.FindElement(By.CssSelector(".text-white")));
            loginButton.Click();
            Thread.Sleep(3000);

            IWebElement textEmail = wait.Until(d => d.FindElement(By.Id("Username")));
            textEmail.SendKeys("freuquojouneuzu-6294@yopmail.com");


            IWebElement textPassword = wait.Until(d => d.FindElement(By.Id("Password")));
            textPassword.SendKeys("Ft3@Qd2@");
            Thread.Sleep(3000);

            IWebElement selectNextStep2Button = wait.Until(d => d.FindElement(By.Name("button")));
            selectNextStep2Button.Click();
            Thread.Sleep(3000);
            // Post creation
            IWebElement profileButton = wait.Until(d => d.FindElement(By.CssSelector(".w-full:nth-child(1) > .capitalize")));
            profileButton.Click();
            Thread.Sleep(10000);

            IWebElement newPostField = wait.Until(d => d.FindElement(By.CssSelector(".max-sm\\3Atext-sm")));
            Thread.Sleep(3000);
            newPostField.Click();
            Thread.Sleep(3000);

            IWebElement textArea = wait.Until(d => d.FindElement(By.CssSelector(".is-empty")));
            textArea.Click();
            textArea.SendKeys("Hay quá bà con ơi!?");
            Thread.Sleep(3000);

            // Submit post
            IWebElement submit = wait.Until(d => d.FindElement(By.CssSelector(".h-9")));
            submit.Click();
            Thread.Sleep(10000);

            IWebElement postDescription = wait.Until(d => d.FindElement(By.CssSelector("div[class='flex flex-col items-center gap-5'] div:nth-child(1) div:nth-child(2) div:nth-child(1) p:nth-child(1)")));
            Assert.That(postDescription.Text, Is.EqualTo("Hay quá bà con ơi!?"));

            IWebElement postStatus = wait.Until(d => d.FindElement(By.CssSelector("body > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > span:nth-child(2)")));
            Assert.That(postStatus.Text, Is.EqualTo("Public"));
        }

        [Test]
        public void CreatePost_02_Case_PostOnePicWithText()
        {
            // Login process
            IWebElement loginButton = wait.Until(d => d.FindElement(By.CssSelector(".text-white")));
            loginButton.Click();
            Thread.Sleep(3000);

            IWebElement textEmail = wait.Until(d => d.FindElement(By.Id("Username")));
            textEmail.SendKeys("freuquojouneuzu-6294@yopmail.com");


            IWebElement textPassword = wait.Until(d => d.FindElement(By.Id("Password")));
            textPassword.SendKeys("Ft3@Qd2@");
            Thread.Sleep(3000);

            IWebElement selectNextStep2Button = wait.Until(d => d.FindElement(By.Name("button")));
            selectNextStep2Button.Click();
            Thread.Sleep(3000);
            // Post creation
            IWebElement profileButton = wait.Until(d => d.FindElement(By.CssSelector(".w-full:nth-child(1) > .capitalize")));
            profileButton.Click();
            Thread.Sleep(10000);

            IWebElement newPostField = wait.Until(d => d.FindElement(By.CssSelector(".max-sm\\3Atext-sm")));
            Thread.Sleep(3000);
            newPostField.Click();
            Thread.Sleep(3000);

            IWebElement textArea = wait.Until(d => d.FindElement(By.CssSelector(".is-empty")));
            textArea.Click();
            textArea.SendKeys("Hay quá bà con ơi!?");
            Thread.Sleep(3000);

            // Add photo
            IWebElement photoButton = wait.Until(d => d.FindElement(By.CssSelector(".tabler-icon-photo-up")));
            photoButton.Click();
            Thread.Sleep(3000);

            IWebElement fileInput = driver.FindElement(By.Id("fileInput"));
            fileInput.SendKeys(@"C:\Users\Admin\Pictures\Img4Test\459201817_513018834812292_966986495180073725_n.jpg");
            Thread.Sleep(3000);

            // Submit post
            IWebElement submitButton = wait.Until(d => d.FindElement(By.CssSelector(".MuiButton-contained")));
            submitButton.Click();
            Thread.Sleep(3000);

            IWebElement submit = wait.Until(d => d.FindElement(By.CssSelector(".h-9")));
            submit.Click();
            Thread.Sleep(10000);

            IWebElement postDescription = wait.Until(d => d.FindElement(By.CssSelector("div[class='flex flex-col items-center gap-5'] div:nth-child(1) div:nth-child(2) div:nth-child(1) p:nth-child(1)")));
            Assert.That(postDescription.Text, Is.EqualTo("Hay quá bà con ơi!?"));

            IWebElement postImg = wait.Until(d => d.FindElement(By.CssSelector("body > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(1) > a:nth-child(1) > img:nth-child(1)")));
            Assert.That(postImg.TagName, Is.EqualTo("img"));

            IWebElement postStatus = wait.Until(d => d.FindElement(By.CssSelector("body > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > span:nth-child(2)")));
            Assert.That(postStatus.Text, Is.EqualTo("Public"));
        }

        [Test]
        public void CreatePost_03_Case_PostTwoPicWithText()
        {
            // Login process
            IWebElement loginButton = wait.Until(d => d.FindElement(By.CssSelector(".text-white")));
            loginButton.Click();
            Thread.Sleep(3000);

            IWebElement textEmail = wait.Until(d => d.FindElement(By.Id("Username")));
            textEmail.SendKeys("freuquojouneuzu-6294@yopmail.com");


            IWebElement textPassword = wait.Until(d => d.FindElement(By.Id("Password")));
            textPassword.SendKeys("Ft3@Qd2@");
            Thread.Sleep(3000);

            IWebElement selectNextStep2Button = wait.Until(d => d.FindElement(By.Name("button")));
            selectNextStep2Button.Click();
            Thread.Sleep(3000);
            // Post creation
            IWebElement profileButton = wait.Until(d => d.FindElement(By.CssSelector(".w-full:nth-child(1) > .capitalize")));
            profileButton.Click();
            Thread.Sleep(10000);

            IWebElement newPostField = wait.Until(d => d.FindElement(By.CssSelector(".max-sm\\3Atext-sm")));
            Thread.Sleep(3000);
            newPostField.Click();
            Thread.Sleep(3000);

            IWebElement textArea = wait.Until(d => d.FindElement(By.CssSelector(".is-empty")));
            textArea.Click();
            textArea.SendKeys("Hay gấp đôi rồi bà con ơi :3");
            Thread.Sleep(3000);

            // Add photo
            IWebElement photoButton = wait.Until(d => d.FindElement(By.CssSelector(".tabler-icon-photo-up")));
            photoButton.Click();
            Thread.Sleep(3000);

            IWebElement fileInput = driver.FindElement(By.Id("fileInput"));
            fileInput.SendKeys(@"C:\Users\Admin\Pictures\Img4Test\459201817_513018834812292_966986495180073725_n.jpg" + "\n" +
                                @"C:\Users\Admin\Pictures\Img4Test\mppydthsd4pmoghwkuh7.jpg");
            Thread.Sleep(3000);

            // Submit post
            IWebElement submitButton = wait.Until(d => d.FindElement(By.CssSelector(".MuiButton-contained")));
            submitButton.Click();
            Thread.Sleep(3000);

            IWebElement submit = wait.Until(d => d.FindElement(By.CssSelector(".h-9")));
            submit.Click();
            Thread.Sleep(10000);

            IWebElement postDescription = wait.Until(d => d.FindElement(By.CssSelector("div[class='flex flex-col items-center gap-5'] div:nth-child(1) div:nth-child(2) div:nth-child(1) p:nth-child(1)")));
            Assert.That(postDescription.Text, Is.EqualTo("Hay gấp đôi rồi bà con ơi :3"));

            IWebElement postImg = wait.Until(d => d.FindElement(By.CssSelector("body > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(1) > a:nth-child(1) > img:nth-child(1)")));
            Assert.That(postImg.TagName, Is.EqualTo("img"));

            IWebElement postImg2 = wait.Until(d => d.FindElement(By.CssSelector("a:nth-child(2) img:nth-child(1)")));
            Assert.That(postImg2.TagName, Is.EqualTo("img"));

            IWebElement postStatus = wait.Until(d => d.FindElement(By.CssSelector("body > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > span:nth-child(2)")));
            Assert.That(postStatus.Text, Is.EqualTo("Public"));
        }

        // Kiểm tra bằng mắt thui :))))
        [Test]
        public void CreatePost_04_Case_PostTwoPicWithText_SwitchPicLocation()
        {
            // Login process
            IWebElement loginButton = wait.Until(d => d.FindElement(By.CssSelector(".text-white")));
            loginButton.Click();
            Thread.Sleep(3000);

            IWebElement textEmail = wait.Until(d => d.FindElement(By.Id("Username")));
            textEmail.SendKeys("freuquojouneuzu-6294@yopmail.com");


            IWebElement textPassword = wait.Until(d => d.FindElement(By.Id("Password")));
            textPassword.SendKeys("Ft3@Qd2@");
            Thread.Sleep(3000);

            IWebElement selectNextStep2Button = wait.Until(d => d.FindElement(By.Name("button")));
            selectNextStep2Button.Click();
            Thread.Sleep(3000);
            // Post creation
            IWebElement profileButton = wait.Until(d => d.FindElement(By.CssSelector(".w-full:nth-child(1) > .capitalize")));
            profileButton.Click();
            Thread.Sleep(10000);

            IWebElement newPostField = wait.Until(d => d.FindElement(By.CssSelector(".max-sm\\3Atext-sm")));
            Thread.Sleep(3000);
            newPostField.Click();
            Thread.Sleep(3000);

            IWebElement textArea = wait.Until(d => d.FindElement(By.CssSelector(".is-empty")));
            textArea.Click();
            textArea.SendKeys("Hay gấp đôi rồi bà con ơi :3");
            Thread.Sleep(3000);

            // Add photo
            IWebElement photoButton = wait.Until(d => d.FindElement(By.CssSelector(".tabler-icon-photo-up")));
            photoButton.Click();
            Thread.Sleep(3000);

            IWebElement fileInput = driver.FindElement(By.Id("fileInput"));
            fileInput.SendKeys(@"C:\Users\Admin\Pictures\Img4Test\459201817_513018834812292_966986495180073725_n.jpg" + "\n" +
                                @"C:\Users\Admin\Pictures\Img4Test\mppydthsd4pmoghwkuh7.jpg");
            Thread.Sleep(3000);

            
            IWebElement submitButton = wait.Until(d => d.FindElement(By.CssSelector(".MuiButton-contained")));
            submitButton.Click();
            Thread.Sleep(3000);


            //IWebElement editPostItemButton = wait.Until(d => d.FindElement(By.CssSelector(".right-0 > .bg-white:nth-child(1)")));
            IWebElement editPostItemButton = wait.Until(d => d.FindElement(By.CssSelector(".right-0 > .bg-white:nth-child(1)")));
            //IWebElement editPostItemButton = wait.Until(d => d.FindElement(By.CssSelector(".hover\\3A bg-orangeFpt:nth-child(1)")));
            //IWebElement editPostItemButton = wait.Until(d => d.FindElement(By.XPath("//div[3]/div/div/div[2]/div/div/div/div")));
            editPostItemButton.Click();
            Thread.Sleep(3000);

            IWebElement editPostItem = wait.Until(d => d.FindElement(By.CssSelector(".col-span-12:nth-child(1) .h-\\[80\\%\\]")));
            actions.DragAndDropToOffset(editPostItem, 150, 20)
                   .Build()
                   .Perform();
            Thread.Sleep(3000);

            IWebElement editPostItemBackButton = wait.Until(d => d.FindElement(By.CssSelector(".tabler-icon-chevron-left")));
            editPostItemBackButton.Click();
            Thread.Sleep(3000);


            // Submit post
            IWebElement submit = wait.Until(d => d.FindElement(By.CssSelector(".h-9")));
            submit.Click();
            Thread.Sleep(10000);

            IWebElement postDescription = wait.Until(d => d.FindElement(By.CssSelector("div[class='flex flex-col items-center gap-5'] div:nth-child(1) div:nth-child(2) div:nth-child(1) p:nth-child(1)")));
            Assert.That(postDescription.Text, Is.EqualTo("Hay gấp đôi rồi bà con ơi :3"));

            IWebElement postImg = wait.Until(d => d.FindElement(By.CssSelector("body > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(1) > a:nth-child(1) > img:nth-child(1)")));
            Assert.That(postImg.TagName, Is.EqualTo("img"));

            IWebElement postImg2 = wait.Until(d => d.FindElement(By.CssSelector("a:nth-child(2) img:nth-child(1)")));
            Assert.That(postImg2.TagName, Is.EqualTo("img"));

            IWebElement postStatus = wait.Until(d => d.FindElement(By.CssSelector("body > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > span:nth-child(2)")));
            Assert.That(postStatus.Text, Is.EqualTo("Public"));
        }

        [Test]
        public void CreatePost_05_Case_PostOnlyText_SwitchPostStatusToFriend()
        {
            // Login process
            IWebElement loginButton = wait.Until(d => d.FindElement(By.CssSelector(".text-white")));
            loginButton.Click();
            Thread.Sleep(3000);

            IWebElement textEmail = wait.Until(d => d.FindElement(By.Id("Username")));
            textEmail.SendKeys("freuquojouneuzu-6294@yopmail.com");


            IWebElement textPassword = wait.Until(d => d.FindElement(By.Id("Password")));
            textPassword.SendKeys("Ft3@Qd2@");
            Thread.Sleep(3000);

            IWebElement selectNextStep2Button = wait.Until(d => d.FindElement(By.Name("button")));
            selectNextStep2Button.Click();
            Thread.Sleep(3000);
            // Post creation
            IWebElement profileButton = wait.Until(d => d.FindElement(By.CssSelector(".w-full:nth-child(1) > .capitalize")));
            profileButton.Click();
            Thread.Sleep(10000);

            IWebElement newPostField = wait.Until(d => d.FindElement(By.CssSelector(".max-sm\\3Atext-sm")));
            Thread.Sleep(3000);
            newPostField.Click();
            Thread.Sleep(3000);

            IWebElement textArea = wait.Until(d => d.FindElement(By.CssSelector(".is-empty")));
            textArea.Click();
            textArea.SendKeys("Chúc AE sức khoẻ, bình an, 8386! Mãi Đỉnh, Mãi Đỉnh!! :3");
            Thread.Sleep(3000);

            IWebElement postStatusDropDownList = wait.Until(d => d.FindElement(By.CssSelector("div[class='flex items-center gap-1 text-xs cursor-pointer bg-orangeFpt text-white font-bold py-1 px-2 w-fit rounded-lg'] span")));
            postStatusDropDownList.Click();
            Thread.Sleep(3000);

            IWebElement dropDownListFriendItem = wait.Until(d => d.FindElement(By.CssSelector("input[value='{\"userStatusId\":\"6ccc6163-667a-43c7-ad2f-7d414a26f402\",\"statusName\":\"Friend\"}']")));
            dropDownListFriendItem.Click();

            IWebElement lostFocus = wait.Until(d => d.FindElement(By.CssSelector(".MuiBackdrop-root.MuiBackdrop-invisible.MuiModal-backdrop.css-g3hgs1-MuiBackdrop-root-MuiModal-backdrop")));
            lostFocus.Click();
            Thread.Sleep(3000);
            //textArea.Click();



            // Submit post
            IWebElement submit = wait.Until(d => d.FindElement(By.CssSelector(".h-9")));
            submit.Click();
            Thread.Sleep(10000);

            //Check inthe current user
            IWebElement postDescription = wait.Until(d => d.FindElement(By.CssSelector("div[class='flex flex-col items-center gap-5'] div:nth-child(1) div:nth-child(2) div:nth-child(1) p:nth-child(1)")));
            Assert.That(postDescription.Text, Is.EqualTo("Chúc AE sức khoẻ, bình an, 8386! Mãi Đỉnh, Mãi Đỉnh!! :3"));

            IWebElement postStatus = wait.Until(d => d.FindElement(By.CssSelector("body > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > span:nth-child(2)")));
            Assert.That(postStatus.Text, Is.EqualTo("Friend"));



            IWebElement profileIcon = wait.Until(d => d.FindElement(By.CssSelector("div[class='rounded-3xl border border-white'] img[alt='avatar']")));
            profileIcon.Click();
            Thread.Sleep(3000);

            IWebElement logoutButton = wait.Until(d => d.FindElement(By.CssSelector("div[class='flex gap-3']")));
            logoutButton.Click();
            Thread.Sleep(3000);

            IWebElement loginButton2 = wait.Until(d => d.FindElement(By.CssSelector(".text-white")));
            loginButton2.Click();
            Thread.Sleep(3000);

            IWebElement textEmail2 = wait.Until(d => d.FindElement(By.Id("Username")));
            textEmail2.SendKeys("trafracamoipa-5403@yopmail.com");

            IWebElement textPassword2 = wait.Until(d => d.FindElement(By.Id("Password")));
            textPassword2.SendKeys("Xv7#Cq2&");
            Thread.Sleep(3000);
            IWebElement selectNextStep2Button2 = wait.Until(d => d.FindElement(By.Name("button")));
            selectNextStep2Button2.Click();
            Thread.Sleep(3000);


            IWebElement friendProfile = wait.Until(d => d.FindElement(By.CssSelector("a[class='w-full h-[52px] px-2 py-3 hover:bg-orangeFpt hover:text-white flex items-center gap-3 cursor-pointer rounded-md']")));
            friendProfile.Click();
            Thread.Sleep(10000);



            //Check with friend account
            IWebElement postDescription2 = wait.Until(d => d.FindElement(By.CssSelector("div[class='flex flex-col items-center gap-5'] div:nth-child(1) div:nth-child(2) div:nth-child(1) p:nth-child(1)")));
            Assert.That(postDescription2.Text, Is.EqualTo("Chúc AE sức khoẻ, bình an, 8386! Mãi Đỉnh, Mãi Đỉnh!! :3"));
            IWebElement postStatus2 = wait.Until(d => d.FindElement(By.CssSelector("body > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > span:nth-child(2)")));
            Assert.That(postStatus2.Text, Is.EqualTo("Friend"));

            IWebElement profileIcon2 = wait.Until(d => d.FindElement(By.CssSelector("div[class='rounded-3xl border border-white'] img[alt='avatar']")));
            profileIcon2.Click();
            Thread.Sleep(3000);

            IWebElement logoutButton2 = wait.Until(d => d.FindElement(By.CssSelector("div[class='flex gap-3']")));
            logoutButton2.Click();
            Thread.Sleep(3000);

            IWebElement loginButton3 = wait.Until(d => d.FindElement(By.CssSelector(".text-white")));
            loginButton3.Click();
            Thread.Sleep(3000);

            IWebElement textEmail3 = wait.Until(d => d.FindElement(By.Id("Username")));
            textEmail3.SendKeys("praseuquoffeje-2961@yopmail.com");
            IWebElement textPassword3 = wait.Until(d => d.FindElement(By.Id("Password")));
            textPassword3.SendKeys("Lb1@Wl3#");
            Thread.Sleep(3000);
            IWebElement selectNextStep2Button3 = wait.Until(d => d.FindElement(By.Name("button")));
            selectNextStep2Button3.Click();
            Thread.Sleep(3000);


            IWebElement searchBox = wait.Until(d => d.FindElement(By.CssSelector("div[class='relative flex items-center justify-start sm:justify-between']")));
            searchBox.Click();

            IWebElement searchBoxExtend = wait.Until(d => d.FindElement(By.CssSelector("input[class='w-full h-[24px] bg-fbWhite p-2 text-base focus:outline-none']")));
            searchBoxExtend.SendKeys("Kua Kua");
            Thread.Sleep(3000);


            IWebElement searchedItem = wait.Until(d => d.FindElement(By.CssSelector("a[class='h-[60px] w-full rounded-3xl flex items-center gap-3 group hover:bg-orangeFpt hover:text-white cursor-pointer p-2']")));
            searchedItem.Click();
            Thread.Sleep(10000);

            //Check with friend account
            IWebElement postDescription3 = wait.Until(d => d.FindElement(By.CssSelector("div[class='flex flex-col items-center gap-5'] div:nth-child(1) div:nth-child(2) div:nth-child(1) p:nth-child(1)")));
            Assert.That(postDescription3.Text, Is.Not.EqualTo("Chúc AE sức khoẻ, bình an, 8386! Mãi Đỉnh, Mãi Đỉnh!! :3"));
            IWebElement postStatus3 = wait.Until(d => d.FindElement(By.CssSelector("body > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > span:nth-child(2)")));
            Assert.That(postStatus3.Text, Is.Not.EqualTo("Friend"));

        }

        [TearDown]
        public void CloseTest()
        {
            driver.Quit();
        }
    }
}
