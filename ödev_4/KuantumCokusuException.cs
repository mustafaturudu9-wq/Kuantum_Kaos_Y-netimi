using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ödev_4
{
    internal class KuantumCokusuException:Exception
    {
        public KuantumCokusuException(string id): base($"KUANTUM ÇÖKÜŞÜ GERÇEKLEŞTİ! Patlayan nesne ID: {id}") { }
    }
}
