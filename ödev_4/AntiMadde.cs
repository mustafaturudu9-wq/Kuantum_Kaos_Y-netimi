using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ödev_4
{
    public  class AntiMadde:KuantumNesnesi,IKritik

    {
        public AntiMadde(string id, double stabilite, int tehlikeSeviyesi) : base(id, stabilite, tehlikeSeviyesi) 
        {
        }
        public override void AnalizEt()
        {
            Stabilite -= 25;
            Console.WriteLine("Evrenin Dokusu Titriyor");

            CokusuKontrolEt();
        }
        public void AcilDurumSogutmasi()
        {
            Stabilite += 50;

            Sicaklik = Math.Max(0, Sicaklik - 25);
            Console.WriteLine("Anti Madde Soğutuldu");
        }
    }
}
