using System.Collections.Generic;

namespace Collector
{
    public interface SelectBy
    {
        IEnumerable<dynamic> Extract(Collectible source);

        IEnumerable<dynamic> Convert(dynamic item);

        void Enqueue(Collectible destination, dynamic value);
    }
}