namespace kafka
{
    public class Snapshot
    {
        public int source_port { get; set; }
        public int dest_port { get; set; }
        public string source_ip { get; set; }
        public string dest_ip { get; set; }
        public string source_mac { get; set; }
        public string dest_mac { get; set; }

    }
}
