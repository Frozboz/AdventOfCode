using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NoSpace;

public class Program
{
    public static void Main()
    {
        int part = 2;

        const int BufferSize = 128;
        using (var fileStream = File.OpenRead("../../Files/info.txt"))
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
        {

            Item start = new Item("/");
            Item item = start;

            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                if (line[0].Equals('$'))  //command
                {
                    string[] cmds = line.Split(' ');

                    //0 - $
                    //1 - cd
                    //2 - directory

                    if (cmds[1].Equals("cd"))
                    {
                        if (cmds[2].Equals("/")) continue;
                            // up
                        if (cmds[2].Equals(".."))
                            item = item?.Parent;
                        else
                            item = item?.Folders.First(x => x.Name == cmds[2]);
                    }

                }
                else
                {
                    //directory listing
                    if (line.Substring(0, 3).Equals("dir"))
                    {
                        string subDirectory = line.Substring(line.IndexOf(' ')+1);

                        item.AddFolders(new Item(subDirectory), item);
                        Console.WriteLine($"adding folder {subDirectory}");
                    }
                    else //file
                    {
                        int size = 0;
                        int.TryParse(line.Substring(0, line.IndexOf(' ')+1), out size);
                        Console.WriteLine($"Filesize {size}");
                        item.AddFiles(size);
                    }
                }


            }

            if (part == 1)
            {
                var combined = DepthFirstTraversal(start);
                var combinedsize = SumSizes(combined, 100000);
                Console.WriteLine($"Part 1 solution: {combinedsize}");
            }
            else
            {
                //thanks Gary
                var filesystem = 70000000;
                var updateSpace = 30000000;

                var combined = DepthFirstTraversal(start);

                var maxSize = combined.Max(x => x.Size());
                var unusedSpace = filesystem - maxSize;
                var spaceToFree = updateSpace - unusedSpace;

                var smallestSpaceToFree = combined.Where(x => x.Size() > spaceToFree).Min(x => x.Size());

                Console.WriteLine(smallestSpaceToFree);
                
            }

        }

        Console.ReadKey();

    }

    public static int SumSizes(IEnumerable<Item> items, int maxsize)
    {
        int sum = 0;
        foreach (Item i in items)
        {
            int size = i.Size();
            sum += size <= maxsize ? i.Size() : 0;
            Console.WriteLine($"---- {size <= maxsize}");
        }

        return sum;

    }

    public class FileItem
    {
        public int Size { get; set; }
        public string Name { get; set; }
    }

    public FileItem AddChild(string name, int size)
    {
        FileItem child = new FileItem();
        child.Name = name;
        child.Size = size;

        return child;
    }

    static IEnumerable<Item> DepthFirstTraversal(Item item)
    {
        var items = new Stack<Item>();
        items.Push(item);

        while (items.Count > 0)
        {
            var n = items.Pop();
            yield return n;

            for (var i = n.Folders.Count - 1; i >= 0; i--)
                items.Push(n.Folders[i]);
        }
    }

    public class Item
    {
        public List<int> FileSizes { get; set; }
        public List<Item> Folders { get; set; }
        public Item Parent { get; set; }
        public string Name { get; set; }

        public Item(string name)
        {
            Name = name;
            FileSizes = new List<int>();
            Folders = new List<Item>();
        }

        public void AddFiles(int size)
        {
            FileSizes.Add(size);
        }

        public void AddFolders(Item child, Item parent)
        {
            Folders.Add(child);
            child.Parent = parent;
        }

        public int Size()
        {
            var totalSize = 0;
            foreach (var file in FileSizes)
            {
                totalSize += file;
            }

            foreach (var folder in Folders)
            {
                var folderSize = folder.Size();
                totalSize += folderSize;
            }

            return totalSize;
        }
    }

}

