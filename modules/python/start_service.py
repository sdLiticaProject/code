import pika
import sys

from utils.db_utils import sdLiticaInfluxClient
from utils.callbacks import approximation_methods
from alive_signal import SignalSender

# interpolation_binding_keys = [
#     '*.interpolation.splines',
#     '*.interpolation.lagrange',
#     '*.interpolation.newton'
# ]

connection = pika.BlockingConnection(
    pika.ConnectionParameters(host='localhost', port=5672))
channel = connection.channel()

channel.exchange_declare(exchange='TimeSeriesExchange', exchange_type='topic', durable=True)



for binding_key, method in approximation_methods.items():
    approximation_queue = channel.queue_declare(binding_key, exclusive=False) # binding_key is just a name for a queue here, not means anything
    queue_name = approximation_queue.method.queue
    channel.queue_bind(exchange='TimeSeriesExchange', queue=queue_name, routing_key=binding_key)

    channel.basic_consume(
        queue=queue_name, on_message_callback=method, auto_ack=True)

print('waiting for messages')



    
SignalSender(5)

#channel.basic_consume(
#    queue=queue_name, on_message_callback=callback, auto_ack=True)
 
channel.start_consuming()
#добавить routing key в шарповый код. слать в exchange Analysis с routing_key. очереди игнорить? использовать регистр как щас

