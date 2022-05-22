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
    internal class Calendario
    {
        private const string PATH_ICS = @"F:\calendar.ics";
        public void CriarInviteCalendario(List<CalendarEvent> evento)
        {
            Calendar calendar = new Calendar();
            calendar.Events.AddRange(evento);
            string calendario = new CalendarSerializer().SerializeToString(calendar);
            File.WriteAllText(PATH_ICS, calendario);
        }
    }
}
