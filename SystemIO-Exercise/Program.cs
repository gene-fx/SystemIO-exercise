using System;
using System.IO;
using System.Globalization;

namespace ExercicioFixacaoArquivos
{
    class Program
    {
        static void Main(string[] args)
        {
            string initPath = Path.GetTempPath();//capta o caminho da pasta temporaria do sistema

            try
            {
                Console.WriteLine(initPath);

                using (FileStream sourceFile = new FileStream(initPath + "source.txt", FileMode.Create)) //cria o aquivo original com o nome, preço e qtd dos produtos
                {
                    using (StreamWriter swSource = new StreamWriter(sourceFile))
                    {
                        Console.Write("How many itens do you wanna record?:");
                        int opt = int.Parse(Console.ReadLine());

                        for (int i = 1; i <= opt; i++)
                        {
                            Console.WriteLine();
                            Console.WriteLine("------------------------------------------------");
                            Console.Write($"Product {i}: ");
                            string prodName = Console.ReadLine();

                            Console.WriteLine();
                            Console.Write($"Price of product {i}: ");
                            double prodPrice = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

                            Console.WriteLine();
                            Console.Write($"Quantity of product {i}: ");
                            int prodQtd = int.Parse(Console.ReadLine());

                            swSource.WriteLine(prodName + ", " + prodPrice.ToString("F2", CultureInfo.InvariantCulture) + ", " + prodQtd);//escreve as informações coletadas no console no arquivo criado acima
                        }
                    }

                    Directory.CreateDirectory(initPath + "out");//cria a pasta onde sera criado o arquivo de saída

                    using (FileStream targetFile = new FileStream(initPath + @"\out\summary.txt", FileMode.Create))//cria o arquivo de saída com as mesmas infos da entrada + o valor total por item
                    {
                        using (StreamWriter swTarget = new StreamWriter(targetFile))
                        {
                            string[] lines = File.ReadAllLines(sourceFile.Name); //lê o arquivo 'source' e transcre as linhas em um vertor string

                            foreach (string line in lines)//percorre cada linha do vertor com as informações do arquivo 'source'
                            {
                                /*estabelece os parametros para recorte das 'substrings'*/
                                int position = line.IndexOf(", ");
                                int lastPosition = line.LastIndexOf(", ");
                                int length = lastPosition - position;

                                /*recorta a linha lida em 'substrings' com os parametros estabelecido acima */
                                string prodName = line.Substring(0);
                                double prodPrice = double.Parse(line.Substring(position + 1, length - 1), CultureInfo.InvariantCulture);
                                int prodQtd = int.Parse(line.Substring(lastPosition + 1));

                                double totalPrice = prodPrice * prodQtd;

                                swTarget.WriteLine(prodName + ", " + totalPrice.ToString("F2", CultureInfo.InvariantCulture));//grava no arquivo de saída
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
