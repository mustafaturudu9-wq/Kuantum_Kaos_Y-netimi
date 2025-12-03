using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ödev_4
{
    public class KaranlikMadde:KuantumNesnesi,IKritik
    {
        public KaranlikMadde(string id, double stabilite, int tehlikeSeviyesi) : base(id, stabilite, tehlikeSeviyesi)
        {
        }

        public override void AnalizEt()
        {
            Stabilite -= 15;
            Console.WriteLine("Karanlık Madde Analiz Edildi");
            CokusuKontrolEt();
        }
        public void AcilDurumSogutmasi() 
        {
            Stabilite += 50;
            Sicaklik = Math.Max(0, Sicaklik - 25);
            Console.WriteLine("Karanlık Madde Soğutuldu.");
        }


    }
}
