using AgendaUnimed.Model;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Newtonsoft.Json;
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
        private const string PATH_CHROMEDRIVE = @"C:\git\AgendaUnimed\";
        private const string PATH_JSON_LOGIN = @"../../../Json/login.json";

        public void Main()
        {
            var options = new ChromeOptions();
            options.AddArguments("--test-type", "--headless", "--ignore-certificate-errors");            
            IWebDriver driver = new ChromeDriver(PATH_CHROMEDRIVE, options);

            try
            {
                string url_agenda_com_data = @$"https://agendaonline.unimedmaringa.com.br/meus-agendamentos/de/{DateTime.Now:yyyy-MM-dd}/ate/{DateTime.Now.AddYears(1):yyyy-MM-dd}/situacao/todos";
                List<CalendarEvent> calendarEvents = new List<CalendarEvent>();
                
                driver.Navigate().GoToUrl("https://agendaonline.unimedmaringa.com.br/");
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

                var login = BusarLoginJson();
                driver.FindElement(By.Name("cpf")).SendKeys(login.Usuario);
                driver.FindElement(By.Name("password")).SendKeys(login.Senha);
                driver.FindElement(By.XPath("//*[@id=\"loginForm\"]/div[2]/button[2]")).Click();

                if (driver.FindElements(By.Name("cpf")).Count() > 0)
                {
                    Console.WriteLine("Erro ao logar!");
                    Console.ReadLine(); 
                    driver.Dispose();
                }

                driver.Navigate().GoToUrl(url_agenda_com_data);
                Thread.Sleep(1);
                var elementAgendamento = driver.FindElements(By.CssSelector("div[class='agendamentoBox']"));
                
                if (elementAgendamento.Count() <= 0)
                {
                    Console.WriteLine("Nenhum agendamento encontrado.");
                    Console.ReadLine();
                    driver.Dispose();
                }

                foreach (var item in elementAgendamento)
                {                    
                    var descricao = item.FindElement(By.XPath("div[2]/div[1]/div[2]/h4")).Text;
                    var data = Convert.ToDateTime(item.FindElement(By.XPath("div[2]/div[2]/div[1]/h4")).Text);
                    var hora = item.FindElement(By.XPath("div[2]/div[2]/div[3]/h4")).Text;
                    var numeroConsulta = item.FindElement(By.XPath("div[2]/div[2]/div[6]/h4")).Text;
                    data = data.AddHours(Convert.ToDouble(hora.Substring(0, 2)));
                    data = data.AddMinutes(Convert.ToDouble(hora.Substring(hora.Length - 2, 2)));

                    var reminder = new Alarm
                    {
                        Action = AlarmAction.Audio,
                        Trigger = new Trigger(TimeSpan.FromHours(-1))
                    };

                    var evento = new CalendarEvent
                    {
                        Summary = "Consulta " + descricao,
                        Start = new CalDateTime(data),
                        End = new CalDateTime(data.AddHours(1)),
                        Description = "Sessão " + numeroConsulta
                    };

                    evento.Alarms.Add(reminder);
                    calendarEvents.Add(evento);
                }

                new Calendario().CriarInviteCalendario(calendarEvents);
                Console.WriteLine("Finalizado");
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao gerar evento para o calendario: " + e.Message);
                Console.ReadLine();
            }
            finally
            {
                if (driver != null)
                    driver.Dispose();
                Environment.Exit(-1);
            }
        }  
        
        private LoginModel BusarLoginJson()
        {            
            StreamReader r = new StreamReader(PATH_JSON_LOGIN);
            string jsonString = r.ReadToEnd();
            return JsonConvert.DeserializeObject<LoginModel>(jsonString);
        }

    }
}
