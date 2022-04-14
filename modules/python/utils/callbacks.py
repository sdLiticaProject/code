from types import TracebackType
import pika
from pika.adapters.blocking_connection import BlockingChannel
from pika.spec import Channel
from configs.config import AMQPConfig, InfluxConfig
import sys
import os
import json
from pathlib import Path
import traceback
repo_root = Path().resolve()
sys.path.append(str(repo_root))
sys.path.append(os.path.abspath(".."))

from utils.db_utils import sdLiticaInfluxClient

class Callback:
    def __init__(self, influx_config) -> None:
        self.influx_client = sdLiticaInfluxClient(influx_config)
    
    def process(self, analysis_method, body, channel: BlockingChannel):
        try:
            message_body = json.loads(json.loads(body)['Body'])
            analysis_method(self.influx_client, message_body)
            message_body['Operation']['Status'] = 1
            print('success')
        except Exception as e:
            print(e)
            traceback.print_exc()
            message_body['Operation']['Status'] = -1
        finally:
            channel.basic_publish(
                exchange='DiagnosticsInfoExchange', routing_key='', body=json.dumps({                
                    "Type":"sdLitica.Events.Integration.DiagnosticsResponseEvent",
                    "Body":json.dumps(message_body)
                })
            )
