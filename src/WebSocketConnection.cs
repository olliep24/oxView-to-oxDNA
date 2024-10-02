using Newtonsoft.Json.Linq;
using System.Formats.Tar;
using WebSocketSharp;

namespace oxViewToOxDNA.src
{
    public class WebSocketConnection
    {
        private readonly WebSocket _ws;
        private readonly string _topFile;
        private readonly string _datFile;

        public WebSocketConnection(string topFile, string datFile)
        {
            _ws = new WebSocket("wss://nanobase.org:8989/");
            _topFile = topFile;
            _datFile = datFile;
        }

        public void Connect()
        {
            _ws.OnMessage += (sender, e) =>
            {
                Console.WriteLine(e.Data.Substring(0, 150));
            };

            _ws.OnError += (sender, e) =>
            {
                Console.WriteLine($"Error: {e.Message}");
            };

            _ws.OnClose -= (sender, e) =>
            {
                Console.WriteLine("Websocket closed");
            };

            _ws.Connect();

            string topFileContent = File.ReadAllText(_topFile).Replace("\r\n", "\n");
            string datFileContent = File.ReadAllText(_datFile).Replace("\r\n", "\n");

            JObject initialMessage = new JObject(
                new JProperty("top_file", topFileContent),
                new JProperty("dat_file", datFileContent),
                new JProperty("settings", new JObject(
                    new JProperty("T", "20C"),
                    new JProperty("steps", "1000000"),
                    new JProperty("salt_concentration", "1"),
                    new JProperty("interaction_type", "DNA2"),
                    new JProperty("print_conf_interval", "10000"),
                    new JProperty("print_energy_every", "10000"),
                    new JProperty("thermostat", "brownian"),
                    new JProperty("dt", "0.003"),
                    new JProperty("diff_coeff", "2.5"),
                    new JProperty("max_density_multiplier", "10"),
                    new JProperty("sim_type", "MD"),
                    new JProperty("T_units", "C"),
                    new JProperty("backend", "CUDA"),
                    new JProperty("backend_precision", "mixed"),
                    new JProperty("time_scale", "linear"),
                    new JProperty("verlet_skin", 0.5),
                    new JProperty("use_average_seq", 0),
                    new JProperty("refresh_vel", 1),
                    new JProperty("CUDA_list", "verlet"),
                    new JProperty("restart_step_counter", 1),
                    new JProperty("newtonian_steps", 103),
                    new JProperty("CUDA_sort_every", 0),
                    new JProperty("use_edge", 1),
                    new JProperty("edge_n_forces", 1),
                    new JProperty("cells_auto_optimisation", "true"),
                    new JProperty("reset_com_momentum", "true"),
                    new JProperty("max_backbone_force", "5"),
                    new JProperty("max_backbone_force_far", "10")
                ))
            );

            var payload = initialMessage.ToString();

            Console.WriteLine($"Payload: {payload} ... END");

            _ws.Send(payload);

            // Run always to keep connection going
            while (true)
            {
                
            }
        }
    }
}
