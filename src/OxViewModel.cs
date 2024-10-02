namespace oxViewToOxDNA.src
{
    public class Monomer
    {
        public int Id { get; set; }
        public string Type { get; set; } = "";
        public string Class { get; set; } = "";
        public List<double> P { get; set; } = new List<double>();
        public List<double> A1 { get; set; } = new List<double>();
        public List<double> A3 { get; set; } = new List<double>();
        public int? N3 { get; set; }
        public int? N5 { get; set; }
        public int Cluster { get; set; }
    }

    public class Strand
    {
        public int Id { get; set; }
        public List<Monomer> Monomers { get; set; } = new List<Monomer>();
        public int End3 { get; set; }
        public int End5 { get; set; }
        public string Class { get; set; } = "";
    }

    public class System
    {
        public int Id { get; set; }
        public List<Strand> Strands { get; set; } = new List<Strand>();
    }

    public class OxViewModel
    {
        public string Date { get; set; } = "";
        public List<double> Box { get; set; } = new List<double> { };
        public List<System> Systems { get; set; } = new List<System>();
        public List<string> Forces { get; set; } = new List<string>();
        public List<string> Selections { get; set; } = new List<string>();
    }
}
