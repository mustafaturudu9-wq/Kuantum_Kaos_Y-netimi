using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ödev_4
{
    public abstract class KuantumNesnesi
    {
        public string ID { get; private set; }

        // Stabilite 0–100 arası olmalı → Kapsülleme uygulanıyor
        private double _stabilite;

        public double Stabilite
        {
            get { return _stabilite; }
            protected set
            {
                // Girilen değer 0 altı ise 0'a sabitlenir
                if (value < 0)
                    _stabilite = 0;

                // Girilen değer 100 üstü ise 100'e sabitlenir
                else if (value > 100)
                    _stabilite = 100;

                // Aralıktaysa direkt değer atanır
                else
                    _stabilite = value;
            }
        }

        // Tehlike seviyesi 1–10 arası olmalı
        private int _tehlikeSeviyesi;
        public int TehlikeSeviyesi
        {
            get { return _tehlikeSeviyesi; }
            protected set
            {
                if (value < 1)
                    _tehlikeSeviyesi = 1;
                else if (value > 10)
                    _tehlikeSeviyesi = 10;
                else
                    _tehlikeSeviyesi = value;
            }
        }
        private static Random r = new Random();

        public int Sicaklik { get; protected set; }
        // Constructor
        public KuantumNesnesi(string id, double stabilite, int tehlikeSeviyesi)
        {
            ID = id;
            Stabilite = stabilite;
            TehlikeSeviyesi = tehlikeSeviyesi;


            Sicaklik = r.Next(20,81);

        }

        // Soyut metot → Alt sınıflar kesinlikle override edecek
        public abstract void AnalizEt();

        // Durum bilgisi → polimorfik olarak çalışır
        public string DurumBilgisi()
        {
            return $"{ID} | Stabilite: {Stabilite} | Tehlike: {TehlikeSeviyesi}|Sıcaklık: {Sicaklik }"
            ;
        }

        // Stabilite 0 veya altına düştüğünde exception fırlatılır
        protected void CokusuKontrolEt()
        {
            if (Stabilite <= 0)
                throw new KuantumCokusuException(ID);
        }
       

    }


}

