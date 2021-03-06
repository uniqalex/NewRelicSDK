﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewRelic.Platform.Sdk;
using NewRelic.Platform.Sdk.Processors;

namespace uniqPlugin
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                Runner run = new Runner();
                run.Add(new UniqAgentFactory());
                run.SetupAndRun();

            }
            catch (Exception e) {
                Console.WriteLine("Exception occurred, unable to continue.\n"+e.Message);
                return -1;
            }

            return 0;
        }

        
    }

    class UniqAgent : Agent
    { 
        private string name;
        private string host;
        private int port;
        private string user;
        private string password;

        //Processors
        private IProcessor connectionsProcessor = new EpochProcessor();



        public override string Guid
        {
            get { return "ru.pravo.uniqplugin"; }
        }

        public override string Version
        {
            get { return "0.0.1"; }
        }

        public UniqAgent(string name, string host, int port)
        {
            this.name = name;
            this.host = host;
            this.port = port;
        }

        public override string GetAgentName()
        {
            return this.name;
        }

        public override void PollCycle()
        {
            int conn = 1;
            ReportMetric("Connections/Count", "connections", conn);
            ReportMetric("Connections/Rate", "connections/sec", connectionsProcessor.Process(conn));
        }
    }

    class UniqAgentFactory : AgentFactory
    {

        public override Agent CreateAgentWithConfiguration(IDictionary<string, object> properties)
        {
            string name = (string)properties["name"];
            string host = (string)properties["host"];
            //int port = (int)properties["port"];
            int port = Convert.ToInt32(properties["port"]);

            return new UniqAgent(name, host, port);
        }
    }
}
