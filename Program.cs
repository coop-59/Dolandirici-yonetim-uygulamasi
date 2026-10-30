using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

namespace TurkDevs_Liste_Yönetimi
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string path = "C:\\Data\\DolandiricilarListesi.txt";

            while (true)
            {
                Console.Clear();

                Console.WriteLine("Hoş geldiniz Frank bey ne tarz bir işlem yapmak istiyorsunuz?");
                Console.WriteLine("1-Yeni dolandırıcı ekle \n2-Listeyi görüntüle");
                ConsoleKeyInfo key = Console.ReadKey(true);

                Console.Clear();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("Lütfen kullanıcının idsini ve ardından sebebini giriniz. \nFormat: kullanıcı_Id:Sebep");
                        string? mesaj = Console.ReadLine();

                        if (mesaj != null)
                        {
                            string[] splitted = mesaj.Split(" ");

                            if (splitted.Length >= 2)
                            {
                                string id = splitted[0];
                                
                                if (ulong.TryParse(id, out ulong userId))
                                {
                                    string reason = string.Join(" ", splitted.Skip(1));

                                    string oldJson;

                                    try
                                    {
                                        oldJson = File.ReadAllText(path);
                                    }
                                    catch (FileNotFoundException)
                                    {
                                        // Console.WriteLine("Mevcut dosya bulunamadı. yenisi oluşturuldu.");
                                        Directory.CreateDirectory("C:\\Data");
                                        File.WriteAllText(path, "");

                                        oldJson = File.ReadAllText(path);
                                    }

                                    Dictionary<ulong, string>? oldFile;

                                    try
                                    {
                                        oldFile = JsonSerializer.Deserialize<Dictionary<ulong, string>>(oldJson);
                                    }
                                    catch (JsonException)
                                    {
                                        oldFile = new();
                                    }

                                    if (oldFile != null)
                                    {
                                        if (oldFile.ContainsKey(userId))
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Bu kullanıcı zaten listede mevcut. üzerine yazılmasını istiyor musunuz?");
                                            Console.WriteLine("1-Evet \n2-Hayır");
                                            ConsoleKeyInfo yesNo = Console.ReadKey(true);

                                            if (yesNo.Key == ConsoleKey.D1)
                                            {
                                                oldFile.Remove(userId);
                                                oldFile.Add(userId, reason);

                                                Console.Clear();
                                                Console.WriteLine("Başarıyla üzerine yazıldı.");
                                                Console.WriteLine($"Kullanıcı Id: {userId} Sebep: {reason}");
                                                Console.WriteLine("Devam etmek için herhangi bir tuşa basınız.");
                                            }
                                            else if (yesNo.Key == ConsoleKey.D2)
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            oldFile.Add(userId, reason);

                                            Console.Clear();
                                            Console.WriteLine("Başarıyla eklendi.");
                                            Console.WriteLine($"Kullanıcı Id: {userId} Sebep: {reason}");
                                            Console.WriteLine("Devam etmek için herhangi bir tuşa basınız.");
                                        }

                                        var options = new JsonSerializerOptions
                                        {
                                            WriteIndented = true,
                                            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                                        };

                                        File.WriteAllText(path, JsonSerializer.Serialize(oldFile, options));
                                        Console.ReadKey(true);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Lütfen geçerli bir format giriniz.");
                                    Console.WriteLine("Geri dönmek için herhangi bir tuşa basınız");
                                    Console.ReadKey(true);
                                }
                            }
                        }
                        break;

                    case (ConsoleKey.D2):
                        string Json;

                        try
                        {
                            Json = File.ReadAllText(path);
                        }
                        catch (FileNotFoundException)
                        {
                            // Console.WriteLine("Mevcut dosya bulunamadı. yenisi oluşturuldu.");
                            Directory.CreateDirectory("C:\\Data");
                            File.WriteAllText(path, "");

                            Json = File.ReadAllText(path);
                        }

                        Dictionary<ulong, string> list;

                        try
                        {
                            list = JsonSerializer.Deserialize<Dictionary<ulong, string>>(Json);
                        }
                        catch (JsonException)
                        {
                            Console.WriteLine("Mevcut bir liste bulunmamaktadır.");
                            Console.WriteLine("Geri dönmek için herhangi bir tuşa basın");
                            Console.ReadKey(true);
                            break;
                        }

                        if (list != null)
                        {
                            foreach (var item in list) {
                                Console.WriteLine(item.Key + " " + item.Value);
                            }

                            Console.WriteLine("\nGeri dönmek için herhangi bir tuşa basın");

                            Console.ReadKey(true);
                        }

                        break;
                }
            }
        }
    }
}
