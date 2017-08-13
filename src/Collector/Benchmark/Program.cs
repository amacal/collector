using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Collector;
using Xenon;

namespace Benchmark
{
    public class Program
    {
        public static void Main()
        {
            Stopwatch watch = new Stopwatch();
            Reflector reflector = new Reflector();

            Collectible collectible = new Collectible(16 * 1024 * 1024);
            Serializer<Page> serializer = reflector.GetSerializer<Page>();

            using (TextReader reader = new StreamReader(@"D:\plwiki-20170720-stub-meta-history1.xml"))
            using (XmlStream stream = new XmlStream(reader))
            {
                watch.Start();

                foreach (dynamic row in stream.Open("page"))
                {
                    collectible.Enqueue(serializer, GetPage(row));

                    if (collectible.Count % 1000 == 0)
                    {
                        GC.Collect();
                        Console.WriteLine($"{collectible.Count} {collectible.UsedSize} {collectible.TotalSize} {watch.Elapsed.TotalSeconds:F2}");
                    }
                }
            }

            Serializer<Revision> byRevision = reflector.GetSerializer<Revision>();
            collectible = Select.Table(collectible, Select.Many(serializer, byRevision, x => x.Revisions));

            GC.Collect();
            Console.WriteLine($"{collectible.Count} {collectible.UsedSize} {collectible.TotalSize} {watch.Elapsed.TotalSeconds:F2}");

            collectible = Sort.Table(collectible, Sort.By(byRevision, x => x.Timestamp).Inverse());

            GC.Collect();
            Console.WriteLine($"{collectible.Count} {collectible.UsedSize} {collectible.TotalSize} {watch.Elapsed.TotalSeconds:F2}");

            for (int i = 0; i < Math.Min(10, collectible.Count); i++)
            {
                Revision revision = collectible.At(byRevision, i).AsDynamic();

                Console.WriteLine($"{i} {revision.Id} {revision.Comment?.Length} {revision.Comment}");
            }

            Console.ReadLine();
        }

        private static Page GetPage(dynamic row)
        {
            return new Page
            {
                Id = row.id.ToInt64(),
                Title = row.title.ToString(),
                Revisions = Enumerable.ToArray(GetRevisions(row.revision))
            };
        }

        private static IEnumerable<Revision> GetRevisions(dynamic row)
        {
            foreach (dynamic item in row)
            {
                yield return GetRevision(item);
            }
        }

        private static Revision GetRevision(dynamic row)
        {
            return new Revision
            {
                Id = row.id.ToInt64(),
                ParentId = row.parentid?.ToInt64(),
                Hash = row.sha1.ToString(),
                Format = row.format.ToString(),
                Comment = row.comment?.ToString(),
                Timestamp = row.timestamp.ToDateTime(),
                User = row.contributor?.username?.ToString(),
                IP = row.contributor?.ip?.ToString()
            };
        }

        private class Page
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public Revision[] Revisions { get; set; }
        }

        private class Revision
        {
            public long Id { get; set; }
            public long? ParentId { get; set; }
            public string Hash { get; set; }
            public string Format { get; set; }
            public string Comment { get; set; }
            public DateTime Timestamp { get; set; }
            public string User { get; set; }
            public string IP { get; set; }
        }
    }
}