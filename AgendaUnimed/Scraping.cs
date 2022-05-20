using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaUnimed
{
    internal class Scraping
    {
        public void Main()
        {
            string usuario;
            string senha;
            var options = new ChromeOptions();
            options.AddArguments("--test-type", "--start-maximized");
            options.AddArguments("--test-type", "--ignore-certificate-errors");

            IWebDriver driver = new ChromeDriver(@"C:\git\AgendaUnimed\", options);
            driver.Navigate().GoToUrl("https://agendaonline.unimedmaringa.com.br/");
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            Thread.Sleep(2000);            
            Console.WriteLine("Informe seu login (000.000.000-00)");
            usuario = Console.ReadLine();
            Console.WriteLine("Informe sua senha");
            senha = Console.ReadLine();
            driver.FindElement(By.Name("cpf")).SendKeys(usuario);
            driver.FindElement(By.Name("password")).SendKeys(senha);
            driver.FindElement(By.XPath("//*[@id=\"loginForm\"]/div[2]/button[2]")).Click();

            if (driver.FindElements(By.Name("cpf")).Count() > 0)
            {
                Console.WriteLine("Erro ao logar!");
            }                
        }
    }
}
