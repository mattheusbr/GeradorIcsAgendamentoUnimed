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
        const string PATH = @"C:\git\AgendaUnimed\";
        
        public void Main()
        {
            var options = new ChromeOptions();
            options.AddArguments("--test-type", "--start-maximized");
            options.AddArguments("--test-type", "--ignore-certificate-errors");
            IWebDriver driver = new ChromeDriver(PATH, options);

            try
            {
                string usuario;
                string senha;
                string url_agenda_com_data = @$"https://agendaonline.unimedmaringa.com.br/meus-agendamentos/de/{DateTime.Now:yyyy-mm-dd}/ate/{DateTime.Now.AddYears(1):yyyy-mm-dd}/situacao/agendado";

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
                    Console.ReadLine(); 
                    driver.Dispose();
                }

                driver.Navigate().GoToUrl(url_agenda_com_data);
                Thread.Sleep(1);
                var elementAgendamento = driver.FindElements(By.CssSelector("agendamentoBox"));
                if (elementAgendamento.Count() <= 0)
                {
                    Console.WriteLine("Nenhum agendamento encontrado.");
                    Console.ReadLine();
                    driver.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao gerar evento para o calendario: " + e.Message);
                Console.ReadLine();
                driver.Dispose();
            }            
        }        
    }
}
