import threading
import time
import json
import sys
import os
import uuid
from pathlib import Path
repo_root = Path().resolve()
sys.path.append(str(repo_root))
sys.path.append(os.path.abspath(".."))

import pika

from utils.callbacks import approximation_methods

class SignalSender():
    def __init__(self, interval=5, daemon = True):
        self.interval = interval
        self.make_message()
        print(self.message_body)
        self.connection = pika.BlockingConnection(
            pika.ConnectionParameters(host='localhost', port=5672))
        self.channel = self.connection.channel()
        self.channel.exchange_declare(exchange='ModuleRegistrationExchange', exchange_type='topic', durable=True)

        thread = threading.Thread(target=self.run, args=())
        thread.daemon = daemon
        thread.start()

    def make_message(self):
        self.message_body = json.dumps({
            "Type":"sdLitica.Events.Integration.AnalyticModuleRegistrationRequestEvent",
            "Body": json.dumps({
                "Id": str(uuid.uuid4()),#newguid
                "DateTime": "2021-04-19T09:25:30.9613795Z",#let it be
                "Name": "sdLitica.Events.Integration.AnalyticModuleRegistrationRequestEvent",
                "Module": {
                    "ModuleGuid": str(uuid.uuid4()),#newguid
                    "Operations": [{
                        "RoutingKey": key, "Description": value.__name__, "Name": value.__name__
                        } for key,value in approximation_methods.items()]
                }
            })         
        })

    def run(self):
        while True:
            print('signal')
            self.channel.basic_publish(
                exchange='ModuleRegistrationExchange', routing_key='', body=self.message_body)
            time.sleep(self.interval)

if __name__ == '__main__':
    SignalSender(5, False)
{"Type": "sdLitica.Events.Integration.AnalyticModuleRegistrationRequestEvent", "Body": {"ModuleGuid": "8d379f81-cb42-4629-b269-9a37faab5661", "Operations": []}, "Id": "f149b075-213e-4f6e-95b7-ca12587be42d", "DateTime": "2021-04-19T09:25:30.9613795Z", "Name": "sdLitica.Events.Integration.AnalyticModuleRegistrationRequestEvent"}