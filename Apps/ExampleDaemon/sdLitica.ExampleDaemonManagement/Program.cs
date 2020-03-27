using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using sdLitica.Messages.Abstractions;
using sdLitica.TimeSeries.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vibrant.InfluxDB.Client;
using Vibrant.InfluxDB.Client.Rows;

namespace sdLitica.ExampleDaemonManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            //influx client setup
            TimeSeriesService _timeSeriesService = new TimeSeriesService();



            //rabbitmq queue setup
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare("example.request.queue", true, false, false, null);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("example.request.queue", true, consumer);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;

                try
                {
                    var strMessage = Encoding.UTF8.GetString(body);
                    Console.WriteLine(strMessage);
                    var message = JsonConvert.DeserializeObject<Message>(strMessage);
                    if (message == null) throw new Exception("Could not deserialize message object");


                    var @event = (Models.TimeSeriesAnalysisEvent)JsonConvert.DeserializeObject(message.Body, typeof(Models.TimeSeriesAnalysisEvent));
                    Console.WriteLine(@event);

                    Task<InfluxResult<DynamicInfluxRow>> task = _timeSeriesService.ReadMeasurementById(@event.Operation.TsId);
                    task.Wait();
                    List<DynamicInfluxRow> rows = task.Result.Series[0].Rows;
                    double[] series = new double[rows.Count];
                    for (int i = 0; i < rows.Count; i++)
                        series[i] = (double)rows[i].Fields["cpu"];

                    Console.WriteLine(ExampleDaemonAnalysis.ExampleFunctions.Mean(series));

                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                }
                finally
                {

                }
            };

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
