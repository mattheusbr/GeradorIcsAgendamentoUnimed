using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaUnimed
{
    public class Calendario
    {
        public void CriarInviteCalendario(List<CalendarEvent> evento)
        {
            var calendar = new Calendar();
            var reminder = new Alarm
            {
                Action = AlarmAction.Audio,
                Trigger = new Trigger(TimeSpan.FromHours(-1))
            };

            var icalEvent = new CalendarEvent
            {
                Summary = "Consulta",
                Start = new CalDateTime(2022, 5, 22, 12, 0, 0),
                End = new CalDateTime(2022, 5, 22, 13, 0, 0),
                Description = "Teste"                
            };



            var calendario = new CalendarSerializer().SerializeToString(calendar);
            File.WriteAllText(@"F:\calendar.ics", calendario);
        }
    }
}
