using Newtonsoft.Json;
using System.Text.Json;

namespace oxViewToOxDNA.src
{
    public static class Converter
    {
        public static void Convert(string oxViewFile, string topFile, string datFile)
        {
            string oxViewFileJSONData = File.ReadAllText(oxViewFile);
            var oxViewModel = JsonConvert.DeserializeObject<OxViewModel>(oxViewFileJSONData);

            if (oxViewModel == null)
            {
                throw new ArgumentException("Please Import a oxView file");
            }

            List<Strand> strands = oxViewModel.Systems.First().Strands;

            if (strands == null)
            {
                throw new ArgumentException("oxView File missing strands");
            }

            int numStrands = strands.Count;
            int numNucleotides = 0;

            foreach (Strand strand in strands)
            {
                numNucleotides += strand.Monomers.Count;
            }

            using (StreamWriter topWriter = new StreamWriter(topFile))
            using (StreamWriter datWriter = new StreamWriter(datFile))
            {
                // File metadata

                // Write number of nucleotides and strands to top file
                topWriter.WriteLine($"{numNucleotides} {numStrands}");

                // dat file metadata
                datWriter.WriteLine("t = 0");
                datWriter.WriteLine("b = " + string.Join(" ", oxViewModel.Box));
                datWriter.WriteLine("E = 0 0 0");

                int strandCounter = 1;
                int globalNucleotideIndex = -1;
                foreach (Strand strand in strands)
                {
                    for (int i = strand.Monomers.Count - 1; i >= 0; i--)
                    {
                        int prime5 = globalNucleotideIndex;
                        int prime3 = globalNucleotideIndex + 2;

                        // Beginning and end of strand edge cases
                        if (i == strand.Monomers.Count - 1)
                        {
                            prime5 = -1;
                        }
                        else if (i == 0)
                        {
                            prime3 = -1;
                        }

                        Monomer monomer = strand.Monomers[i];

                        // Top file - last nucleotide should have -1 as its prime3 value
                        topWriter.WriteLine($"{strandCounter} {monomer.Type} {prime5} {prime3}");

                        // Dat file
                        datWriter.WriteLine($"{string.Join(" ", monomer.P)} {string.Join(" ", monomer.A1)} {string.Join(" ", monomer.A3)}" + " 0 0 0 0 0 0");

                        globalNucleotideIndex++;
                    }
                    strandCounter++;
                }
            }
        }
    }
}
