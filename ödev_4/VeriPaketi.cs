using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ödev_4
{
    public class VeriPaketi : KuantumNesnesi
    {
        public VeriPaketi(string id, double stabilite)
      : base(id, stabilite, tehlikeSeviyesi: 1)
        {
        }
        public override void AnalizEt()
        {
            Stabilite -= 5;

            Console.WriteLine("veri içeriği okundu");

            CokusuKontrolEt();

        }
    }
}
