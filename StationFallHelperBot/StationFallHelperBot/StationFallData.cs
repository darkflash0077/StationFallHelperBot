using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationFallHelperBot
{
    public class StationFallData
    {
        public Character[] Characters { get; set; }
        public Ability[] Abilitys { get; set; }
    }

    public class Character
    {
        public string name { get; set; }
        public string starts { get; set; }
        public string bonusPoints { get; set; }
        public string influencLimit { get; set; }
        public string characterType { get; set; }
        public string[] abilitys { get; set; }
        public string[] hiddenAbilitys { get; set; }
        public Agendum[] agenda { get; set; }
        public string story { get; set; }
        public string[] tips { get; set; }
        public Ability[] Abilitys => MainBOt.data.Abilitys.Where(x => abilitys.Contains(x.name)).ToArray();
        public Ability[] HiddenAbilitys => MainBOt.data.Abilitys.Where(x => hiddenAbilitys.Contains(x.name)).ToArray();
        public string ToHTMLString() {
            return
                $@"<u><b>Имя:</b></u> {name}
<u><b>Начальная локация:</b></u> {starts}
<u><b>Бонусные очки:</b></u> {bonusPoints}
<u><b>Предел влияния:</b></u> {influencLimit}
<u><b>Вид персонажа:</b></u> {characterType}

<u><b>Способности:</b></u> 
{string.Join(Environment.NewLine, Abilitys.Select(x => $"<b><i>{x.name}:</i></b> {x.effect}"))}

<u><b>Способности раскрытия:</b></u> 
{string.Join(Environment.NewLine, HiddenAbilitys.Select(x => $"<b><i>{x.name}:</i></b> {x.effect}"))}

<u><b>Цели:</b></u> 
{string.Join(Environment.NewLine, agenda.Select(x => $"{x}"))}

<u><b>Советы:</b></u>
{string.Join(Environment.NewLine, tips.Select(x => $"❗️ {x}") )}
";
        }
    }

    public class Agendum
    {        
        public string main { get; set; }
        public string[] bonus { get; set; }
        public override string ToString()
        {
            return
                @$"🎯 {main}" + String.Join("", bonus.Select(x => Environment.NewLine + "  " + $"<i>{x}</i>"));
        }
    }

    public class Ability
    {
        public string name { get; set; }
        public string effect { get; set; }
    }

}
