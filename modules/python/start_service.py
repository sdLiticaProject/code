from configs.config import AMQPConfig, AliveSignalConfig, InfluxConfig
import pika
import sys

from utils.db_utils import sdLiticaInfluxClient
from utils.callbacks import Callback
from analysis.approximation import approximation_methods
from alive_signal import SignalSender

# interpolation_binding_keys = [
#     '*.interpolation.splines',
#     '*.interpolation.lagrange',
#     '*.interpolation.newton'
# ]

influx_config = InfluxConfig()
amqp_config = AMQPConfig()
alive_signal_config = AliveSignalConfig()


connection = pika.BlockingConnection(pika.ConnectionParameters(
    host=amqp_config.host, 
    port=amqp_config.port
))
channel = connection.channel()
diagnostics_channel = connection.channel()

channel.exchange_declare(exchange='TimeSeriesExchange', exchange_type='topic', durable=True)
channel.exchange_declare(exchange='DiagnosticsInfoExchange', exchange_type='topic', durable=True)

callback = Callback(influx_config)

for binding_key, method in approximation_methods.items():
    approximation_queue = channel.queue_declare(binding_key, exclusive=False) # binding_key is just a name for a queue here, not means anything
    queue_name = approximation_queue.method.queue
    channel.queue_bind(exchange='TimeSeriesExchange', queue=queue_name, routing_key=binding_key)

    channel.basic_consume(
        queue=queue_name, 
        on_message_callback= lambda ch, _1, _2, body: callback.process(method, body, ch), 
        auto_ack=True
    )

print('waiting for messages')
    
SignalSender(alive_signal_config, amqp_config)

 
channel.start_consuming()

