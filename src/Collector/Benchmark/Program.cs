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

            List<Page> pages = new List<Page>();

            using (TextReader reader = new StreamReader(@"D:\plwiki-20170720-stub-meta-history1.xml"))
            using (XmlStream stream = new XmlStream(reader))
            {
                watch.Start();

                foreach (dynamic row in stream.Open("page"))
                {
                    //pages.Add(GetPage(row));
                    collectible.Add(serializer, GetPage(row));

                    //if (pages.Count % 1000 == 0)
                    if (collectible.Count % 1000 == 0)
                    {
                        GC.Collect();

                        //Console.WriteLine($"{pages.Count} {Process.GetCurrentProcess().PrivateMemorySize64} {watch.Elapsed.TotalSeconds:F2}");
                        Console.WriteLine($"{collectible.Count} {collectible.UsedSize} {collectible.TotalSize} {watch.Elapsed.TotalSeconds:F2}");
                    }
                }
            }

            for (int i = 0; i < collectible.Count; i++)
            {
                Page page = collectible.At(serializer, i);

                Console.WriteLine($"{i} {page.Id} {page.Revisions.Length} {page.Title}");
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